using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webbshop.Models;

namespace webbshop
{
    // Auto genererat av chatGPT
    public static class DatabaseSeeder
    {
        // =====================
        // Static seed data
        // =====================
        private static readonly string[] FirstNames =
            {
                "Erik","Anna","Johan","Maria","Fredrik","Emma","Oscar","Sara","Daniel","Elin",
                "Magnus","Sofia","Andreas","Johanna","Mikael","Linda","Alexander","Karin",
                "Peter","Therese","Henrik","Malin","Robert","Camilla","Sebastian","Ida",
                "Martin","Frida","Patrik","Louise","Niklas","Helena","Simon","Josefin",
                "David","Amanda","Mattias","Ebba","Jesper","Emelie","Viktor","Maja",
                "Jonathan","Linnea","Marcus","Olivia","Filip","Hanna"
            };

        private static readonly string[] LastNames =
            {
                "Andersson","Johansson","Karlsson","Nilsson","Eriksson","Larsson","Olsson",
                "Persson","Svensson","Gustafsson","Pettersson","Hansson","Jansson","Bengtsson",
                "Lindberg","Lindström","Lindqvist","Holm","Holmberg","Nyström","Berg","Bergström",
                "Berglund","Sandberg","Engström","Eklund","Sjöberg","Wallin","Hedlund",
                "Ström","Åberg","Isaksson","Pålsson","Löfgren","Blomqvist","Rosenberg",
                "Ekström","Norberg","Viklund","Falk","Hellström","Granlund","Sundberg",
                "Öberg","Claesson","Moberg","Arvidsson","Björk"
            };

        private static readonly string[] Streets =
        {
        "Storgatan","Parkvägen","Skolgatan","Ringvägen","Kyrkogatan",
        "Björkvägen","Industrigatan","Järnvägsgatan","Ängsvägen","Backgatan"
    };

        // =====================
        // Entry point
        // =====================
        public static async Task PopulateDatabase(ShopDbContext context)
        {
            if (context.Users.Any())
                return; // redan seedad

            // =====================
            // Payment & Delivery
            // =====================
            var paymentOptions = new List<PaymentOption>
        {
            new() { Name = "Kort" },
            new() { Name = "Klarna" }
        };

            var deliveryOptions = new List<DeliveryOption>
        {
            new() { Name = "Postnord", Price = 89 },
            new() { Name = "Instabox", Price = 69 }
        };

            context.AddRange(paymentOptions);
            context.AddRange(deliveryOptions);

            // =====================
            // Categories
            // =====================
            var categories = new List<Category>
        {
            new() { Name = "Processorer" },
            new() { Name = "Grafikkort" },
            new() { Name = "Lagring" }
        };
            context.AddRange(categories);

            // =====================
            // Suppliers
            // =====================
            var suppliers = new List<Supplier>
        {
            new() { Name = "Intel" },
            new() { Name = "AMD" },
            new() { Name = "Nvidia" },
            new() { Name = "Samsung" },
            new() { Name = "Western Digital" }
        };
            context.AddRange(suppliers);

            context.SaveChanges();

            // =====================
            // Products (3x5)
            // =====================
            var products = new List<Product>
        {
            // CPUs
            new()
        {
            Name = "Intel i5-13600K",
            Category = categories[0],
            Supplier = suppliers[0],
            Price = 3499,
            InventoryBalance = 50,
            IsSelected = true,
            Details = "Intel Core i5-13600K är en kraftfull processor för både gaming och produktivitet. Med en hybridarkitektur som kombinerar prestanda- och effektivitetskärnor levererar den hög bildfrekvens i spel samtidigt som den klarar krävande multitasking. Ett utmärkt val för moderna stationära datorer."
        },

        new()
        {
            Name = "Intel i7-13700K",
            Category = categories[0],
            Supplier = suppliers[0],
            Price = 4599,
            InventoryBalance = 30,
            Details = "Intel Core i7-13700K erbjuder mycket hög prestanda för entusiaster och avancerade användare. Processorn passar perfekt för streaming, videoredigering och tunga arbetsflöden samtidigt som den levererar stabil spelprestanda i de senaste titlarna."
        },

        new()
        {
            Name = "AMD Ryzen 5 7600X",
            Category = categories[0],
            Supplier = suppliers[1],
            Price = 3299,
            InventoryBalance = 40,
            Details = "AMD Ryzen 5 7600X är en modern processor baserad på Zen 4-arkitekturen. Den är framtagen för hög spelprestanda och låg latens, med stöd för de senaste plattformarna. Ett bra val för dig som vill bygga en balanserad och framtidssäker dator."
        },

        new()
        {
            Name = "AMD Ryzen 7 7700X",
            Category = categories[0],
            Supplier = suppliers[1],
            Price = 4399,
            InventoryBalance = 25,
            Details = "Ryzen 7 7700X kombinerar hög klockfrekvens med flera kärnor, vilket gör den idealisk för både gaming och produktivt arbete. Processorn passar användare som vill ha stark prestanda i både vardagliga och mer krävande applikationer."
        },

        new()
        {
            Name = "AMD Ryzen 9 7950X",
            Category = categories[0],
            Supplier = suppliers[1],
            Price = 7499,
            InventoryBalance = 15,
            Details = "AMD Ryzen 9 7950X är en toppmodell för entusiaster och professionella användare. Med ett stort antal kärnor och trådar är den optimerad för tung rendering, simulering och avancerade arbetsuppgifter där maximal prestanda krävs."
        },

            // GPUs
                    new()
        {
            Name = "RTX 4070",
            Category = categories[1],
            Supplier = suppliers[2],
            Price = 7499,
            InventoryBalance = 20,
            IsSelected = true,
            Details = "Nvidia GeForce RTX 4070 är ett modernt grafikkort som levererar hög prestanda i 1440p-upplösning. Kortet passar perfekt för gaming med höga grafikinställningar och erbjuder stöd för moderna grafiktekniker som förbättrar både bildkvalitet och flyt."
        },

        new()
        {
            Name = "RTX 4080",
            Category = categories[1],
            Supplier = suppliers[2],
            Price = 12999,
            InventoryBalance = 10,
            Details = "RTX 4080 är ett kraftfullt grafikkort för entusiaster som vill spela i hög upplösning eller arbeta med grafiktunga applikationer. Det lämpar sig väl för både avancerad gaming och kreativa arbetsflöden som kräver hög beräkningskapacitet."
        },

        new()
        {
            Name = "RX 7800 XT",
            Category = categories[1],
            Supplier = suppliers[1],
            Price = 6999,
            InventoryBalance = 18,
            Details = "AMD Radeon RX 7800 XT är ett starkt grafikkort för modern gaming. Det är byggt för att leverera stabil prestanda i dagens spel och är ett attraktivt alternativ för användare som söker hög prestanda till ett balanserat pris."
        },

        new()
        {
            Name = "RX 7900 XTX",
            Category = categories[1],
            Supplier = suppliers[1],
            Price = 11499,
            InventoryBalance = 8,
            Details = "RX 7900 XTX är AMD:s flaggskeppsgrafikkort för krävande användare. Det är anpassat för spel i hög upplösning och tunga grafiska arbetsuppgifter, med fokus på rå prestanda och effektivitet."
        },

        new()
        {
            Name = "RTX 4090",
            Category = categories[1],
            Supplier = suppliers[2],
            Price = 19999,
            InventoryBalance = 5,
            Details = "RTX 4090 är ett extremt kraftfullt grafikkort för entusiaster som vill ha det bästa som finns tillgängligt. Kortet är avsett för avancerad gaming, rendering och professionella arbetsuppgifter där kompromisser inte är ett alternativ."
        },

            // Storage
                    new()
        {
            Name = "Samsung 980 Pro 1TB",
            Category = categories[2],
            Supplier = suppliers[3],
            Price = 1299,
            InventoryBalance = 60,
            IsSelected = true,
            Details = "Samsung 980 Pro är en snabb NVMe-SSD som ger korta laddningstider och snabb systemrespons. Den passar perfekt som systemdisk för både gamingdatorer och arbetsstationer där snabb åtkomst till data är viktigt."
        },

        new()
        {
            Name = "Samsung 990 Pro 2TB",
            Category = categories[2],
            Supplier = suppliers[3],
            Price = 2499,
            InventoryBalance = 40,
            Details = "Samsung 990 Pro erbjuder mycket hög prestanda och gott om lagringsutrymme. Den är utformad för användare som arbetar med stora filer, spelbibliotek eller krävande applikationer där hastighet och stabilitet är avgörande."
        },

        new()
        {
            Name = "WD Black SN850 1TB",
            Category = categories[2],
            Supplier = suppliers[4],
            Price = 1199,
            InventoryBalance = 50,
            Details = "WD Black SN850 är en högpresterande SSD riktad mot gamers och entusiaster. Den ger snabba laddningstider och förbättrad systemprestanda, vilket gör den idealisk för moderna spel och krävande program."
        },

        new()
        {
            Name = "WD Blue 2TB HDD",
            Category = categories[2],
            Supplier = suppliers[4],
            Price = 799,
            InventoryBalance = 70,
            Details = "WD Blue 2TB är en pålitlig mekanisk hårddisk som passar bra för lagring av stora mängder data. Den lämpar sig för arkivering, backup och filer som inte kräver extremt snabba läs- och skrivhastigheter."
        },

        new()
        {
            Name = "Samsung 870 EVO 1TB",
            Category = categories[2],
            Supplier = suppliers[3],
            Price = 999,
            InventoryBalance = 55,
            Details = "Samsung 870 EVO är en stabil och beprövad SATA-SSD med bra balans mellan prestanda och pris. Den passar utmärkt för uppgraderingar av äldre system eller som extra lagring i moderna datorer."
        },
        };

            context.AddRange(products);

            // =====================
            // Countries & Cities
            // =====================
            var countries = new List<Country>
        {
            new()
            {
                Name = "Sverige",
                Cities =
                {
                    new City { Name = "Stockholm" },
                    new City { Name = "Göteborg" },
                    new City { Name = "Malmö" }
                }
            },
            new()
            {
                Name = "Norge",
                Cities =
                {
                    new City { Name = "Oslo" },
                    new City { Name = "Bergen" },
                    new City { Name = "Trondheim" }
                }
            },
            new()
            {
                Name = "Finland",
                Cities =
                {
                    new City { Name = "Helsingfors" },
                    new City { Name = "Tammerfors" },
                    new City { Name = "Åbo" }
                }
            },
            new()
            {
                Name = "Danmark",
                Cities =
                {
                    new City { Name = "Köpenhamn" },
                    new City { Name = "Aarhus" },
                    new City { Name = "Odense" }
                }
            },
            new()
            {
                Name = "Tyskland",
                Cities =
                {
                    new City { Name = "Berlin" },
                    new City { Name = "Hamburg" },
                    new City { Name = "München" }
                }
            }
        };

            context.AddRange(countries);
            context.SaveChanges();

            // =====================
            // Users (20 per country)
            // =====================
            var users = new List<User>();
            users.Add(new User
            {
                FirstName = "admin",
                LastName = "",
                Email = $"admin@shop.com",
                Password = "Password123!",
                PhoneNumber = $"",
                StreetName = $"admin gatan",
                CityId = 1,
                BirthDate = new DateTime(1985, 1, 1),
                IsAdmin = true,
            });
            users.Add(new User
            {
                FirstName = "kund",
                LastName = "",
                Email = $"kund@shop.com",
                Password = "Password123!",
                PhoneNumber = $"",
                StreetName = $"kund gatan",
                CityId = 1,
                BirthDate = new DateTime(1985, 1, 1),
                IsAdmin = false,
            });
            int userIndex = 1;

            foreach (var country in countries)
            {
                var cities = country.Cities.ToList();

                for (int i = 0; i < 20; i++)
                {
                    var firstName = FirstNames[(userIndex + i) % FirstNames.Length];
                    var lastName = LastNames[(userIndex + i * 2) % LastNames.Length];
                    var street = Streets[i % Streets.Length];

                    users.Add(new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{userIndex}@shop.se",
                        Password = "Password123!",
                        PhoneNumber = $"07{userIndex % 10}-{100000 + userIndex}",
                        StreetName = $"{street} {i + 1}",
                        City = cities[i % cities.Count],
                        BirthDate = new DateTime(1985 + (i % 15), (i % 12) + 1, (i % 28) + 1),
                        IsAdmin = false,
                    });

                    userIndex++;
                }
            }

            context.AddRange(users);
            context.SaveChanges();

            // =====================
            // Orders (some shipped, some not)
            // =====================
            foreach (var user in users)
            {
                // 1–4 produkter per user
                int numberOfProductsBought = (user.Id % 4) + 1;

                int startIndex = (user.Id * 7) % products.Count;
                int step = (user.Id % 5) + 1;

                for (int i = 0; i < numberOfProductsBought; i++)
                {
                    int productIndex = (startIndex + i * step) % products.Count;
                    var product = products[productIndex];

                    context.PaymentHistories.Add(new PaymentHistory
                    {
                        User = user,
                        Product = product,
                        Amount = ((user.Id + i) % 3) + 1,
                        PaymentOption = paymentOptions[(user.Id + i) % paymentOptions.Count],
                        DeliveryOption = deliveryOptions[(user.Id + i) % deliveryOptions.Count],
                        DeliveryCity = user.City!,
                        DeliveryStreet = user.StreetName,
                        PayedDate = DateTime.Now.AddDays(-20 + i),
                        SendDate = (user.Id + productIndex) % 3 == 0
                            ? DateTime.Now.AddDays(-10 + i)
                            : DateTime.MinValue
                    });
                }
            }

            // =====================
            // Cart items
            // =====================
            foreach (var user in users.Skip(40).Take(20))
            {
                context.CartProducts.Add(new CartProduct
                {
                    User = user,
                    Product = products[user.Id % products.Count],
                    Amount = 2
                });
            }

            await context.SaveChangesAsync();
        }
    }

}
