using FinalProject.Models;

namespace FinalProject.Data;

public class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                context.Products.AddRange(new List<Product>()
                {
                    new Product()
                    {
                        Name = "Jeans trousers",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 15.00,
                        Description = "Description of a product",
                        Quantity = 20,
                        CategoryEnum = CategoryEnum.Men
                    },
                    new Product()
                    {
                        Name = "Jeans trousers",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 25.00, 
                        Description = "Description of a Jeans trousers",
                        Quantity = 19,
                        CategoryEnum = CategoryEnum.Men
                    },
                    new Product()
                    {
                        Name = "jacket",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 40.00,
                        Description = "Description of a jacket",
                        Quantity = 12,
                        CategoryEnum = CategoryEnum.Women
                    },
                    new Product()
                    {
                        Name = "Bag",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 35.00,
                        Description = "Description of a Bag",
                        Quantity = 17,
                        CategoryEnum = CategoryEnum.Women
                    },
                    new Product()
                    {
                        Name = "Bag",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 60.00,
                        Description = "Description of a Bag",
                        Quantity = 13,
                        CategoryEnum = CategoryEnum.Women
                    },
                    new Product()
                    {
                        Name = "Sneakers",
                        Image = "https://www.lilysilk.com/media/catalog/product/m2_custom/9890/41/1.jpg?quality=80&bg-color=255%2C255%2C255&fit=bounds&width=1800",
                        Price = 70.00,
                        Description = "Description of a Sneakers",
                        Quantity = 10,
                        CategoryEnum = CategoryEnum.Men
                    }
                });
                context.SaveChanges();
            }
        }
    }
}