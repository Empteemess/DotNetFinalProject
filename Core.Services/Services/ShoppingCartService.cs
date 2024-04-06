using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using MimeKit.Text;

namespace Core.Services.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly DependencyConfiguration _dependencyConfiguration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IShoppingCartRepository _repository;
    private readonly AppDbContext _context;

    public ShoppingCartService(DependencyConfiguration dependencyConfiguration,
        IHttpContextAccessor httpContextAccessor,IShoppingCartRepository repository,
        AppDbContext context)
    {
        _dependencyConfiguration = dependencyConfiguration;
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync()
    {
        var person = await _repository.GetCurrentUserAsync();
        if (person != null)
        {
            return person.CartProducts;
        }

        return null;
    }

    public async Task AddProductsInCartAsync(int id, int sellQuantity)
    {
        await _repository.AddProductsInCartAsync(id, sellQuantity);
    }

    public async Task<double> GetSelledProductPriceAsync()
    {
        var wholePrice = 0.0;
        var user = await _repository.GetCurrentUserAsync();
        foreach (var item in user.CartProducts)
        {
            wholePrice += item.SellQuantity * item.Price;
        }

        return wholePrice;
    }

    public async Task<CartProducts> GetItemByIdAsync(int id)
    {
        var user = await _repository.GetCurrentUserAsync();

        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == id);
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == exactItem.ProductId);
        exactItem.Quantity = product.Quantity;
        return exactItem;
    }

    public async Task<bool> UpdateEditedItemAsync(CartProducts model)
    {
        var user = await _repository.GetCurrentUserAsync();;

        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == model.Id);
        var item = await _context.Products.FirstOrDefaultAsync(x => x.Id == exactItem.ProductId);

        var itemQuantity = (item.Quantity + exactItem.SellQuantity);

        if (exactItem != null)
        {
            exactItem.SellQuantity = model.SellQuantity;
            item.Quantity = itemQuantity - model.SellQuantity;

            exactItem.Quantity = itemQuantity - model.SellQuantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var user = await _repository.GetCurrentUserAsync();;
        var item = user.CartProducts.FirstOrDefault(x => x.Id == id);

        await AddProductQuantity(item.ProductId, item.SellQuantity);

        if (item != null)
        {
            user.CartProducts.Remove(item);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task EmailSenderAsync()
    {
        var text = string.Empty;
        var user = await _repository.GetCurrentUserAsync();;
        var customerEmail = user.Email;
        var products = user.CartProducts;
        var purchasedItems = "";
        var totalPrice = 0.0;

        foreach (var item in products)
        {
            purchasedItems += $"{item.Name}:{item.Price}$ x {item.SellQuantity}\n";
            totalPrice += (item.Price * item.SellQuantity);
        }

        var subject = "Subject: Thank You for Your Recent Purchase!";

        text = $@"Thank you for choosing PixieStore for your recent purchase! 
Your order confirmation details are as follows:
Date of Purchase: {(DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString())}:{DateTime.Now.ToString("MM")}:{DateTime.Now.Year}
Item(s) Purchased: 
 {purchasedItems}
Total Price: {totalPrice}$";

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("giorgi", "treiser02@gmail.com"));
        email.To.Add(new MailboxAddress("Recipient", customerEmail));
        email.Subject = subject;

        var body = new TextPart(TextFormat.Plain)
        {
            Text = text
        };

        var multipart = new Multipart("mixed");
        multipart.Add(body);
        email.Body = multipart;

        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 465, true);
            smtp.Authenticate("treiser02@gmail.com", "njuj csjs vgie ztiv");

            try
            {
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

            smtp.Disconnect(true);
        }
    }

    public async Task EmailSenderAsync(string userEmail, string userText)
    {
        var text = userText;
        var customerEmail = userEmail;

        var subject = "Successfull registration";

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("giorgi", "treiser02@gmail.com"));
        email.To.Add(new MailboxAddress("Recipient", customerEmail));
        email.Subject = subject;

        var body = new TextPart(TextFormat.Plain)
        {
            Text = text
        };

        var multipart = new Multipart("mixed");
        multipart.Add(body);
        email.Body = multipart;

        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 465, true);
            smtp.Authenticate("treiser02@gmail.com", "njuj csjs vgie ztiv");

            try
            {
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

            smtp.Disconnect(true);
        }
    }

    public async Task BuyItemAsync()
    {
        await EmailSenderAsync();

        var user = await _repository.GetCurrentUserAsync();;
        var cartProducts = user.CartProducts;

        foreach (var item in cartProducts)
        {
            user.CartProducts.Remove(item);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
    }

    public async Task RemoveProductQuantity(int id, int sellQuantity)
    {
        await _repository.RemoveProductQuantity(id, sellQuantity);
    }

    public async Task AddProductQuantity(int id, int sellQuantity)
    {
        await _repository.AddProductQuantity(id, sellQuantity);
    }

    public CreditCartViewModel MapCartProductsToCreditCadViewModel(IEnumerable<CartProducts> cartProducts)
    {
        var creditCard = new CreditCartViewModel()
        {
            CartProducts = cartProducts
        };
        return creditCard;
    }
}