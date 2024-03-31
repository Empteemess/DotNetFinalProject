using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace Core.Services.Services;

public class ShoppingCartService : IShoppingService
{
    private readonly DependencyConfiguration _dependencyConfiguration;
    private readonly IShoppingCartRepository _repository;

    public ShoppingCartService(DependencyConfiguration dependencyConfiguration, IShoppingCartRepository repository)
    {
        _dependencyConfiguration = dependencyConfiguration;
        _repository = repository;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var personId = _dependencyConfiguration._httpContextAccessor.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var person = await _dependencyConfiguration._userManager.Users.FirstOrDefaultAsync(x => x.Id == personId);
        return person;
    }

    public async Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync()
    {
        var person = await GetCurrentUserAsync();
        if (person != null)
        {
            return person.CartProducts;
        }

        return null;
    }

    public async Task AddProductsInCartAsync(int id, int sellQuantity)
    {
        var exactItem = await _dependencyConfiguration._context.Products.FirstOrDefaultAsync(x => x.Id == id);

        var product = new CartProducts()
        {
            ProductId = exactItem.Id,
            Name = exactItem.Name,
            Description = exactItem.Description,
            Image = exactItem.Image,
            Price = exactItem.Price,
            Quantity = exactItem.Quantity,
            SellQuantity = sellQuantity,
        };

        var user = await GetCurrentUserAsync();

        if (exactItem.Quantity < sellQuantity && user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            var existedProduxt = user.CartProducts.FirstOrDefault(x => x.ProductId == id);
            existedProduxt.SellQuantity += exactItem.Quantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else if (exactItem.Quantity < sellQuantity && !user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            product.SellQuantity = exactItem.Quantity;
            user.CartProducts.Add(product);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else if (!user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            product.SellQuantity = sellQuantity;
            user.CartProducts.Add(product);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else
        {
            var existedProduxt = user.CartProducts.FirstOrDefault(x => x.ProductId == id);
            existedProduxt.SellQuantity += sellQuantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
    }

    public async Task<double> GetSelledProductPriceAsync()
    {
        var wholePrice = 0.0;
        var user = await GetCurrentUserAsync();
        foreach (var item in user.CartProducts)
        {
            wholePrice += item.SellQuantity * item.Price;
        }

        return wholePrice;
    }

    public async Task<CartProducts> GetItemByIdAsync(int id)
    {
        var user = await GetCurrentUserAsync();

        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == id);

        return exactItem;
    }

    public async Task<bool> UpdateEditedItemAsync(CartProducts model)
    {
        var user = await GetCurrentUserAsync();
        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == model.Id);
        if (exactItem != null)
        {
            exactItem.SellQuantity = model.SellQuantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var user = await GetCurrentUserAsync();
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

    private async Task EmailSenderAsync(string textInput)
    {
        var text = textInput;
        var user = await GetCurrentUserAsync();
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

        if (textInput == "")
        {
            text = $@"Thank you for choosing PixieStore for your recent purchase! 
Your order confirmation details are as follows:
Date of Purchase: {(DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString())}:{DateTime.Now.ToString("MM")}:{DateTime.Now.Year}
Item(s) Purchased: 
 {purchasedItems}
Total Price: {totalPrice}$";
        }

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
        await EmailSenderAsync(string.Empty);

        var user = await GetCurrentUserAsync();
        var cartProducts = user.CartProducts;

        foreach (var item in cartProducts)
        {
            user.CartProducts.Remove(item);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
    }

    public async Task RemoveProductQuantity(int id,int sellQuantity)
    {
        await _repository.RemoveProductQuantity(id,sellQuantity);
    }
    
    public async Task AddProductQuantity(int id,int sellQuantity)
    {
        await _repository.AddProductQuantity(id,sellQuantity);
    }
}