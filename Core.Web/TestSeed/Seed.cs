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

            // if (!context.Products.Any())
            // {
            context.Products.AddRange(new List<Product>()
            {
                new Product()
                {
                    Name = "Nike Air Max Dn",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/eb0ab02c-ba01-4953-9e45-8c18b2f03659/air-max-dn-shoes-dFBSQh.png",
                    Price = 160,
                    Description = "Say hello to the next generation of Air technology. The Air Max Dn features our Dynamic Air unit system of dual-pressure tubes, creating a reactive sensation with every step. This results in a futuristic design that’s comfortable enough to wear from day to night. Go ahead—Feel the Unreal.",
                    Quantity = 40,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Alphafly 3",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/477abbf8-b37a-4fe6-b516-08e294a8487a/alphafly-3-mens-road-racing-shoes-6Nc43S.png",
                    Price = 285,
                    Description = "Fine-tuned for marathon speed, the Alphafly 3 helps push you beyond what you thought possible. Three innovative technologies power your run: A double dose of Air Zoom units helps launch you into your next step; a full-length carbon fiber plate helps propel you forward with ease; and a heel-to-toe ZoomX foam midsole helps keep you fresh from start to 26.2. Time to leave your old personal records in the dust.",
                    Quantity = 30,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Air Jordan 3 Retro Craft \"Ivory\"",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco,u_126ab356-44d8-4a06-89b4-fcdcc8df0245,c_scale,fl_relative,w_1.0,h_1.0,fl_layer_apply/196ac49c-9b28-42d0-8538-0c1b8352d358/air-jordan-3-retro-craft-ivory-mens-shoes-R60dgp.png",
                    Price = 210,
                    Description = "Clean and supreme, the AJ3 returns. Pairing a smooth Ivory base with Grey Mist suede overlays and translucent accents on the heel tab and sole, this crafted edition ups the texture. It has dual branding on the heel and double Jumpman logos on the tongue—because twice the Jumpman is twice as nice.",
                    Quantity = 30,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Pegasus 40",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/67d84b61-0fa1-4f8e-ad1e-ea2822d9c09b/pegasus-40-mens-road-running-shoes-extra-wide-zD8H1c.png",
                    Price = 130,
                    Description = "Responsive cushioning provides a springy ride for any run. Experience lighter-weight energy return in this latest version with a combination of Zoom Air units and React foam. Plus, the redesigned midfoot and upper provides an improved, super comfortable fit.",
                    Quantity = 30,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Air Jordan 13 Retro \"Blue Grey\"",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco,u_126ab356-44d8-4a06-89b4-fcdcc8df0245,c_scale,fl_relative,w_1.0,h_1.0,fl_layer_apply/b4ae1fce-fe05-4288-991b-d7b66cc42f59/air-jordan-13-retro-blue-grey-mens-shoes-TBQf23.png",
                    Price = 200,
                    Description = "Throw it back with the AJ13, the shoe originally worn by MJ during his sixth championship season. A new colorway refreshes the legend with premium White tumbled leather and Blue Grey synthetic suede. Iconic details abound, like the unmistakable quilted overlay, panther paw-inspired outsole and holographic eye. Finishing it off, hits of Yellow Ochre make the classic branding pop.",
                    Quantity = 30,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Air Max Plus",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/4423a999-2e81-4bff-9e04-f0848c0f776b/air-max-plus-shoes-x9G2xF.png",
                    Price = 130,
                    Description = "Make some waves in the Nike Air Max Plus, a Tuned Air experience that offers premium stability and cushioning. A breathable knit upper works with wavy design lines, gradient colors and polished plastic accents around the toe to blend comfort with defiant style.\n\n",
                    Quantity = 40,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Tatum 2 \"Vortex\"",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco,u_126ab356-44d8-4a06-89b4-fcdcc8df0245,c_scale,fl_relative,w_1.0,h_1.0,fl_layer_apply/3190981f-a3f8-4196-a919-16fb51ee44dc/tatum-2-vortex-basketball-shoes-vxD2dS.png",
                    Price = 125,
                    Description = "Bright colors and big energy come together in the Tatum 2 \"Vortex.\" The lightweight, flexible design was created with energy return in mind, and this colorway is all about how Jayson helps energize his team. When you're wearing them on the court, your opponents won't be able to ignore all the moves you make—but that doesn't mean they can stop you from scoring.",
                    Quantity = 40,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Air Max 95 Essential",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/i1-fe4f44e2-cfd9-495e-9be1-e572868e350f/air-max-95-essential-mens-shoes-4Nzc1w.png",
                    Price = 175,
                    Description = "Taking inspiration from the human body and running DNA, the Nike Air Max 95 Essential mixes unbelievable comfort with head turning style. The iconic side panels represent muscles while visible Nike Air in the heel and forefoot cushions your every step.",
                    Quantity = 20,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Air Max 97",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/a47b2ef9-8239-4e82-99fd-e6159c0df489/air-max-97-mens-shoes-LJmK45.png",
                    Price = 175,
                    Description = "Featuring the original ripple design inspired by Japanese bullet trains, the Nike Air Max 97 lets you push your style full-speed ahead. Taking the revolutionary full-length Nike Air unit that shook up the running world and adding fresh colors and crisp details, it lets you ride in first-class comfort.",
                    Quantity = 45,
                    CategoryEnum = CategoryEnum.Men
                },
                new Product()
                {
                    Name = "Nike Air Max Plus",
                    Image = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/b434966d-c850-4aa7-be6c-e99c0e236362/air-max-plus-mens-shoes-3mH52P.png",
                    Price = 180,
                    Description = "Let your attitude have the edge in the Air Max Plus, featuring a Tuned Air experience for premium stability and unbelievable cushioning. It celebrates defiant style by combining the original wavy design lines, plastic accents and airy mesh.",
                    Quantity = 24,
                    CategoryEnum = CategoryEnum.Men
                },
            });
            context.SaveChanges();
            // }
        }
    }
}