using Catalog.API.Common.Helpers;
using Catalog.API.Entities;
using MongoDB.Driver;
using Platform.Constants;

namespace Catalog.API.Common.Extensions;

public static class InitializeDB
{
    // === CATEGORIES (5) ===
    private static readonly string CategoryElectronicsId = ModelHelpers.GenerateId();
    private static readonly string CategoryFashionId = ModelHelpers.GenerateId();
    private static readonly string CategoryHomeGardenId = ModelHelpers.GenerateId();
    private static readonly string CategorySportsId = ModelHelpers.GenerateId();
    private static readonly string CategoryBooksId = ModelHelpers.GenerateId();

    // === ELECTRONICS SUBCATEGORIES (8) ===
    private static readonly string SubCategorySmartphonesId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryLaptopsId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryTabletsId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryHeadphonesId = ModelHelpers.GenerateId();
    private static readonly string SubCategorySmartwatchesId = ModelHelpers.GenerateId();
    private static readonly string SubCameraId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryGamersId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryAccessoriesId = ModelHelpers.GenerateId();

    // === FASHION SUBCATEGORIES (8) ===
    private static readonly string SubCategoryMenClothingId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryWomenClothingId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryKidsClothingId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryShoesId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryAccessoriesClothingId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryBagsId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryJewelryId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryWatchesId = ModelHelpers.GenerateId();

    // === HOME & GARDEN SUBCATEGORIES (5) ===
    private static readonly string SubCategoryFurnitureId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryKitchenId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryBedroomId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryLivingRoomId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryGardenId = ModelHelpers.GenerateId();

    // === SPORTS SUBCATEGORIES (6) ===
    private static readonly string SubCategoryFitnessId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryOutdoorId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryTeamSportsId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryWaterSportsId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryYogaId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryCyclingId = ModelHelpers.GenerateId();

    // === BOOKS SUBCATEGORIES (3) ===
    private static readonly string SubCategoryFictionId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryNonfictionId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryEducationalId = ModelHelpers.GenerateId();
    
    public static async Task InitializePlatformDbContextsAsync(
        ConfigurationManager configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.ConnectionString));
        var database = client.GetDatabase(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.DatabaseName));
        
        var isRebuildSchema = bool.Parse(configuration["DatabaseSettings:MongoDb:IsRebuildSchema"] ?? "false");

        if (isRebuildSchema)
        {
            var cateCollection = database.GetCollection<Category>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Category);
            cateCollection.DeleteMany(Builders<Category>.Filter.Empty);
            await cateCollection.InsertManyAsync(GetPreconfiguratedCategories());

            var subCateCollection = database.GetCollection<SubCategory>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.SubCategory);
            subCateCollection.DeleteMany(Builders<SubCategory>.Filter.Empty);
            await subCateCollection.InsertManyAsync(GetPreconfiguratedSubCategories());

            var productCollection = database.GetCollection<Product>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Product);
            productCollection.DeleteMany(Builders<Product>.Filter.Empty);
            await productCollection.InsertManyAsync(GetPreconfiguratedProducts());
        }
    }
    
    private static IEnumerable<Category> GetPreconfiguratedCategories()
    {
        return new List<Category>()
        {
            new()
            {
                Id = CategoryElectronicsId,
                CategoryCode = "ELEC-001",
                Name = "Electronics",
                Description = "Latest electronic devices and gadgets including smartphones, laptops, tablets, and accessories"
            },
            new()
            {
                Id = CategoryFashionId,
                CategoryCode = "FASH-001",
                Name = "Fashion",
                Description = "Trendy clothing, accessories, shoes, bags, jewelry, and watches for men, women, and kids"
            },
            new()
            {
                Id = CategoryHomeGardenId,
                CategoryCode = "HOME-001",
                Name = "Home & Garden",
                Description = "Furniture, kitchen supplies, bedroom essentials, living room decor, and garden products"
            },
            new()
            {
                Id = CategorySportsId,
                CategoryCode = "SPORT-001",
                Name = "Sports & Outdoors",
                Description = "Fitness equipment, outdoor gear, team sports, water sports, yoga, and cycling products"
            },
            new()
            {
                Id = CategoryBooksId,
                CategoryCode = "BOOK-001",
                Name = "Books",
                Description = "Fiction, non-fiction, educational books, and reading materials"
            }
        };
    }

    private static IEnumerable<SubCategory> GetPreconfiguratedSubCategories()
    {
        return new List<SubCategory>()
        {
            // === ELECTRONICS SUBCATEGORIES (8) ===
            new() { Id = SubCategorySmartphonesId, SubCategoryCode = "ELEC-SP-001", Name = "Smartphones", Description = "Latest flagship and budget smartphones from top brands", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategoryLaptopsId, SubCategoryCode = "ELEC-LP-001", Name = "Laptops", Description = "High-performance laptops for work and gaming", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategoryTabletsId, SubCategoryCode = "ELEC-TB-001", Name = "Tablets", Description = "Portable tablets for entertainment and productivity", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategoryHeadphonesId, SubCategoryCode = "ELEC-HP-001", Name = "Headphones & Earbuds", Description = "Premium audio devices with wireless connectivity", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategorySmartwatchesId, SubCategoryCode = "ELEC-SW-001", Name = "Smartwatches", Description = "Wearable smartwatches with fitness tracking", CategoryId = CategoryElectronicsId },
            new() { Id = SubCameraId, SubCategoryCode = "ELEC-CM-001", Name = "Cameras & Photography", Description = "Digital cameras and photography equipment", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategoryGamersId, SubCategoryCode = "ELEC-GM-001", Name = "Gaming Devices", Description = "Gaming consoles, graphics cards, and gaming peripherals", CategoryId = CategoryElectronicsId },
            new() { Id = SubCategoryAccessoriesId, SubCategoryCode = "ELEC-AC-001", Name = "Electronics Accessories", Description = "Chargers, cables, cases, and other electronic accessories", CategoryId = CategoryElectronicsId },

            // === FASHION SUBCATEGORIES (8) ===
            new() { Id = SubCategoryMenClothingId, SubCategoryCode = "FASH-MC-001", Name = "Men's Clothing", Description = "T-shirts, shirts, pants, jackets for men", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryWomenClothingId, SubCategoryCode = "FASH-WC-001", Name = "Women's Clothing", Description = "Dresses, tops, jeans, sarees for women", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryKidsClothingId, SubCategoryCode = "FASH-KC-001", Name = "Kids Clothing", Description = "Comfortable and colorful clothing for children", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryShoesId, SubCategoryCode = "FASH-SH-001", Name = "Shoes", Description = "Casual, formal, and sports shoes for all", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryAccessoriesClothingId, SubCategoryCode = "FASH-AC-001", Name = "Clothing Accessories", Description = "Belts, scarves, caps, and other clothing accessories", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryBagsId, SubCategoryCode = "FASH-BG-001", Name = "Bags", Description = "Backpacks, handbags, travel bags, and purses", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryJewelryId, SubCategoryCode = "FASH-JW-001", Name = "Jewelry", Description = "Rings, necklaces, bracelets, and earrings", CategoryId = CategoryFashionId },
            new() { Id = SubCategoryWatchesId, SubCategoryCode = "FASH-WT-001", Name = "Watches", Description = "Analog and digital watches for men and women", CategoryId = CategoryFashionId },

            // === HOME & GARDEN SUBCATEGORIES (5) ===
            new() { Id = SubCategoryFurnitureId, SubCategoryCode = "HOME-FR-001", Name = "Furniture", Description = "Beds, sofas, chairs, tables, and shelves", CategoryId = CategoryHomeGardenId },
            new() { Id = SubCategoryKitchenId, SubCategoryCode = "HOME-KT-001", Name = "Kitchen & Dining", Description = "Cookware, utensils, dinnerware, and kitchen appliances", CategoryId = CategoryHomeGardenId },
            new() { Id = SubCategoryBedroomId, SubCategoryCode = "HOME-BD-001", Name = "Bedroom", Description = "Bedding, pillows, mattresses, and bedroom decor", CategoryId = CategoryHomeGardenId },
            new() { Id = SubCategoryLivingRoomId, SubCategoryCode = "HOME-LR-001", Name = "Living Room", Description = "Sofas, coffee tables, lighting, and wall decor", CategoryId = CategoryHomeGardenId },
            new() { Id = SubCategoryGardenId, SubCategoryCode = "HOME-GD-001", Name = "Garden & Outdoor", Description = "Garden tools, planters, outdoor furniture, and landscaping supplies", CategoryId = CategoryHomeGardenId },

            // === SPORTS SUBCATEGORIES (6) ===
            new() { Id = SubCategoryFitnessId, SubCategoryCode = "SPORT-FT-001", Name = "Fitness Equipment", Description = "Dumbbells, treadmills, yoga mats, and exercise equipment", CategoryId = CategorySportsId },
            new() { Id = SubCategoryOutdoorId, SubCategoryCode = "SPORT-OD-001", Name = "Outdoor Gear", Description = "Tents, backpacks, hiking boots, and camping equipment", CategoryId = CategorySportsId },
            new() { Id = SubCategoryTeamSportsId, SubCategoryCode = "SPORT-TS-001", Name = "Team Sports", Description = "Footballs, basketballs, cricket bats, and team sports equipment", CategoryId = CategorySportsId },
            new() { Id = SubCategoryWaterSportsId, SubCategoryCode = "SPORT-WS-001", Name = "Water Sports", Description = "Surfboards, wetsuits, swimming gear, and water sports equipment", CategoryId = CategorySportsId },
            new() { Id = SubCategoryYogaId, SubCategoryCode = "SPORT-YG-001", Name = "Yoga & Pilates", Description = "Yoga mats, blocks, straps, and meditation accessories", CategoryId = CategorySportsId },
            new() { Id = SubCategoryCyclingId, SubCategoryCode = "SPORT-CY-001", Name = "Cycling", Description = "Bicycles, helmets, cycling clothes, and bike accessories", CategoryId = CategorySportsId },

            // === BOOKS SUBCATEGORIES (3) ===
            new() { Id = SubCategoryFictionId, SubCategoryCode = "BOOK-FC-001", Name = "Fiction", Description = "Novels, stories, and fictional literature", CategoryId = CategoryBooksId },
            new() { Id = SubCategoryNonfictionId, SubCategoryCode = "BOOK-NF-001", Name = "Non-Fiction", Description = "Biographies, history, self-help, and educational books", CategoryId = CategoryBooksId },
            new() { Id = SubCategoryEducationalId, SubCategoryCode = "BOOK-ED-001", Name = "Educational", Description = "Textbooks, academic books, and learning materials", CategoryId = CategoryBooksId }
        };
    }

    private static IEnumerable<Product> GetPreconfiguratedProducts()
    {
        var products = new List<Product>();
        var productCode = 1;

        // === SMARTPHONES (15 products) ===
        var smartphones = new[]
        {
            ("iPhone 15 Pro", "Premium flagship iPhone with titanium design", 999.99m),
            ("iPhone 15", "Latest iPhone with A17 Pro chip", 799.99m),
            ("Samsung Galaxy S24 Ultra", "Flagship Android phone with AI features", 1299.99m),
            ("Samsung Galaxy A54", "Mid-range Samsung with great value", 449.99m),
            ("Google Pixel 8 Pro", "Google's flagship with amazing camera", 999.99m),
            ("Google Pixel 8", "Affordable Google phone", 699.99m),
            ("OnePlus 12", "Fast performance budget flagship", 799.99m),
            ("Xiaomi 14 Ultra", "Premium Chinese smartphone", 899.99m),
            ("Oppo Find X7", "Innovative smartphone design", 749.99m),
            ("Vivo X100", "Flagship killer phone", 699.99m),
            ("Realme 12 Pro", "Budget-friendly flagship", 349.99m),
            ("Motorola Edge 50", "Near-stock Android experience", 399.99m),
            ("Nothing Phone 2", "Unique design with LED light", 549.99m),
            ("Sony Xperia 1 VI", "Professional smartphone", 1099.99m),
            ("HTC U24 Pro", "Comeback smartphone", 799.99m)
        };

        foreach (var (name, desc, price) in smartphones)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategorySmartphonesId));

        // === LAPTOPS (12 products) ===
        var laptops = new[]
        {
            ("MacBook Pro 16\" M3 Max", "Professional laptop for creators", 3499.99m),
            ("MacBook Pro 14\" M3", "Portable powerful laptop", 1999.99m),
            ("MacBook Air M2", "Affordable yet powerful MacBook", 1199.99m),
            ("Dell XPS 15", "Ultra-slim performance laptop", 1799.99m),
            ("Dell XPS 13", "Compact powerhouse laptop", 999.99m),
            ("HP Spectre x360", "2-in-1 convertible laptop", 1299.99m),
            ("Lenovo ThinkPad X1 Carbon", "Business laptop excellence", 1499.99m),
            ("Asus ROG Zephyrus G16", "Gaming laptop with RTX graphics", 2499.99m),
            ("MSI Stealth 16 Studio", "Creator-focused gaming laptop", 2199.99m),
            ("Razer Blade 16", "Premium gaming ultrabook", 2799.99m),
            ("Framework Laptop", "Modular repairable laptop", 1399.99m),
            ("Acer Swift 3", "Budget gaming laptop", 649.99m)
        };

        foreach (var (name, desc, price) in laptops)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategoryLaptopsId));

        // === TABLETS (8 products) ===
        var tablets = new[]
        {
            ("iPad Pro 12.9\" M2", "Ultimate iPad for professionals", 1099.99m),
            ("iPad Air 11\"", "Powerful mid-range iPad", 599.99m),
            ("iPad 10th Gen", "Budget-friendly iPad", 349.99m),
            ("Samsung Galaxy Tab S9 Ultra", "Android flagship tablet", 1099.99m),
            ("Samsung Galaxy Tab S9 FE", "Affordable Samsung tablet", 349.99m),
            ("Lenovo Tab P11 Pro", "Budget Android tablet", 399.99m),
            ("Microsoft Surface Pro 9", "Laptop-tablet hybrid", 999.99m),
            ("OnePlus Pad", "Gaming tablet alternative", 479.99m)
        };

        foreach (var (name, desc, price) in tablets)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategoryTabletsId));

        // === HEADPHONES & EARBUDS (10 products) ===
        var headphones = new[]
        {
            ("Sony WH-1000XM5", "Best noise-canceling headphones", 399.99m),
            ("Bose QuietComfort 45", "Premium comfortable headphones", 379.99m),
            ("Apple AirPods Pro Max", "Luxury over-ear headphones", 549.99m),
            ("Apple AirPods Pro", "Popular wireless earbuds", 249.99m),
            ("Samsung Galaxy Buds2 Pro", "Android premium earbuds", 229.99m),
            ("Nothing Ear", "Stylish transparent earbuds", 99.99m),
            ("Sony LinkBuds S", "Lightweight true wireless earbuds", 198.99m),
            ("Sennheiser Momentum 4", "Long battery life headphones", 399.99m),
            ("Beats Studio Pro", "Audio quality focused", 349.99m),
            ("JBL Tune 670NC", "Budget noise-canceling", 99.99m)
        };

        foreach (var (name, desc, price) in headphones)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategoryHeadphonesId));

        // === SMARTWATCHES (8 products) ===
        var smartwatches = new[]
        {
            ("Apple Watch Series 9", "Premium fitness smartwatch", 399.99m),
            ("Apple Watch SE", "Affordable Apple Watch", 249.99m),
            ("Samsung Galaxy Watch6 Classic", "Rotating bezel smartwatch", 299.99m),
            ("Samsung Galaxy Watch6", "Modern design smartwatch", 249.99m),
            ("Garmin Epix Gen 2", "Sports-focused smartwatch", 599.99m),
            ("Fitbit Sense 2", "Health tracking focus", 299.99m),
            ("OnePlus Watch 2", "Budget smartwatch", 149.99m),
            ("Pixel Watch 2", "Google smartwatch", 349.99m)
        };

        foreach (var (name, desc, price) in smartwatches)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategorySmartwatchesId));

        // === CAMERAS (5 products) ===
        var cameras = new[]
        {
            ("Sony Alpha 7R V", "High-resolution mirrorless camera", 3298.00m),
            ("Canon EOS R5", "Professional mirrorless camera", 3899.00m),
            ("Nikon Z9", "Advanced mirrorless camera", 5496.95m),
            ("GoPro Hero 12", "Action camera", 499.99m),
            ("DJI Air 3", "Consumer drone camera", 999.99m)
        };

        foreach (var (name, desc, price) in cameras)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCameraId));

        // === GAMING DEVICES (5 products) ===
        var gaming = new[]
        {
            ("PS5 Console", "Sony latest gaming console", 499.99m),
            ("Xbox Series X", "Microsoft flagship console", 499.99m),
            ("Nintendo Switch OLED", "Portable gaming device", 349.99m),
            ("RTX 4090 Graphics Card", "Ultimate gaming GPU", 1599.99m),
            ("RGB Gaming Chair", "Premium gaming chair", 299.99m)
        };

        foreach (var (name, desc, price) in gaming)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategoryGamersId));

        // === ELECTRONICS ACCESSORIES (7 products) ===
        var accessories = new[]
        {
            ("USB-C Fast Charger 65W", "Universal fast charging", 29.99m),
            ("Power Bank 20000mAh", "Long battery backup", 34.99m),
            ("Lightning Cable 6ft", "Durable Apple cable", 19.99m),
            ("Phone Case Rugged", "Protective phone case", 14.99m),
            ("Screen Protector Pack", "Tempered glass protection", 9.99m),
            ("USB Hub 7-in-1", "Multi-port connectivity", 39.99m),
            ("Wireless Charging Pad", "Fast wireless charging", 24.99m)
        };

        foreach (var (name, desc, price) in accessories)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryElectronicsId, SubCategoryAccessoriesId));

        // === MEN'S CLOTHING (12 products) ===
        var menClothing = new[]
        {
            ("Cotton T-Shirt Blue", "Comfortable everyday t-shirt", 19.99m),
            ("Formal Shirt White", "Professional office shirt", 49.99m),
            ("Denim Jeans Dark Blue", "Classic fit jeans", 59.99m),
            ("Chinos Beige", "Versatile chinos", 44.99m),
            ("Polo Shirt Red", "Casual polo shirt", 34.99m),
            ("Winter Jacket Black", "Warm winter coat", 129.99m),
            ("Hoodie Gray", "Comfortable hoodie", 39.99m),
            ("Swim Shorts", "Beach swim trunks", 29.99m),
            ("Leather Belt Brown", "Classic leather belt", 34.99m),
            ("Cargo Shorts Khaki", "Practical cargo shorts", 39.99m),
            ("Bomber Jacket Navy", "Stylish jacket", 89.99m),
            ("Henley Shirt Green", "Casual henley shirt", 24.99m)
        };

        foreach (var (name, desc, price) in menClothing)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryMenClothingId));

        // === WOMEN'S CLOTHING (12 products) ===
        var womenClothing = new[]
        {
            ("Women T-Shirt Pink", "Comfortable women's tee", 19.99m),
            ("Summer Dress Floral", "Light and breezy dress", 54.99m),
            ("Women Jeans Skinny", "Trendy skinny jeans", 59.99m),
            ("Saree Silk", "Traditional silk saree", 89.99m),
            ("Kurti Cotton", "Ethnic traditional kurti", 34.99m),
            ("Cardigan Sweater Beige", "Cozy cardigan", 44.99m),
            ("Winter Coat Wool", "Warm wool coat", 149.99m),
            ("Tank Top White", "Basic tank top", 12.99m),
            ("Leggings Black", "Comfortable leggings", 24.99m),
            ("Maxi Skirt Navy", "Long flowing skirt", 39.99m),
            ("Crop Top Blue", "Trendy crop top", 22.99m),
            ("Blazer Gray", "Professional blazer", 79.99m)
        };

        foreach (var (name, desc, price) in womenClothing)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryWomenClothingId));

        // === KIDS CLOTHING (8 products) ===
        var kidsClothing = new[]
        {
            ("Kids T-Shirt Cartoon", "Fun printed kids tee", 14.99m),
            ("Kids Jeans Blue", "Durable kids jeans", 29.99m),
            ("Summer Dress Kids", "Light kids dress", 24.99m),
            ("Kids Hoodie Pink", "Cozy kids hoodie", 29.99m),
            ("Kids Shorts Colorful", "Playful kids shorts", 19.99m),
            ("Kids Winter Jacket", "Warm kids jacket", 59.99m),
            ("Kids Pajama Set", "Comfortable sleepwear", 24.99m),
            ("Kids Swim Costume", "Fun beach wear", 22.99m)
        };

        foreach (var (name, desc, price) in kidsClothing)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryKidsClothingId));

        // === SHOES (10 products) ===
        var shoes = new[]
        {
            ("Running Shoes Nike", "Professional running shoes", 129.99m),
            ("Casual Sneakers White", "Versatile casual shoes", 74.99m),
            ("Formal Shoes Brown", "Classic formal shoes", 89.99m),
            ("Sneakers Black Adidas", "Popular sports sneakers", 99.99m),
            ("Sandals Comfort", "Easy-wear sandals", 34.99m),
            ("Boots Leather", "Stylish leather boots", 119.99m),
            ("Flip Flops", "Simple flip flops", 14.99m),
            ("Sports Shoes Basketball", "Basketball court shoes", 109.99m),
            ("Loafers Comfortable", "Casual loafer shoes", 69.99m),
            ("Heels Pumps Black", "Elegant heels", 79.99m)
        };

        foreach (var (name, desc, price) in shoes)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryShoesId));

        // === CLOTHING ACCESSORIES (6 products) ===
        var clothingAcc = new[]
        {
            ("Leather Belt Black", "Classic leather belt", 34.99m),
            ("Scarf Wool Warm", "Cozy wool scarf", 24.99m),
            ("Cap Baseball Black", "Sports baseball cap", 19.99m),
            ("Beanie Winter", "Warm winter beanie", 22.99m),
            ("Sunglasses UV", "Protective sunglasses", 49.99m),
            ("Necktie Silk", "Professional silk necktie", 29.99m)
        };

        foreach (var (name, desc, price) in clothingAcc)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryAccessoriesClothingId));

        // === BAGS (8 products) ===
        var bags = new[]
        {
            ("Backpack Travel", "Large travel backpack", 79.99m),
            ("Handbag Leather", "Premium leather handbag", 129.99m),
            ("Shoulder Bag Canvas", "Casual shoulder bag", 44.99m),
            ("Laptop Backpack", "Padded laptop backpack", 59.99m),
            ("Crossbody Bag", "Convenient crossbody", 39.99m),
            ("Gym Bag Duffel", "Sports duffel bag", 49.99m),
            ("Wallet Leather", "Compact leather wallet", 29.99m),
            ("Tote Bag Large", "Spacious tote bag", 34.99m)
        };

        foreach (var (name, desc, price) in bags)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryBagsId));

        // === JEWELRY (7 products) ===
        var jewelry = new[]
        {
            ("Gold Ring", "18K gold ring", 249.99m),
            ("Diamond Necklace", "Premium diamond pendant", 399.99m),
            ("Silver Bracelet", "Elegant silver bracelet", 99.99m),
            ("Pearl Earrings", "Classic pearl studs", 79.99m),
            ("Gemstone Ring", "Colorful gemstone ring", 149.99m),
            ("Gold Chains", "Fashionable gold chain", 119.99m),
            ("Bangle Set", "Traditional bangle set", 89.99m)
        };

        foreach (var (name, desc, price) in jewelry)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryJewelryId));

        // === WATCHES (6 products) ===
        var watches = new[]
        {
            ("Rolex Submariner", "Luxury luxury watch", 5999.99m),
            ("Seiko Prospex", "Professional diving watch", 399.99m),
            ("Casio G-Shock", "Durable sports watch", 99.99m),
            ("Omega Seamaster", "Premium sports watch", 4999.99m),
            ("Citizen Eco-Drive", "Solar powered watch", 299.99m),
            ("Fossil Smartwatch", "Fashion smartwatch", 199.99m)
        };

        foreach (var (name, desc, price) in watches)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryFashionId, SubCategoryWatchesId));

        // === FURNITURE (8 products) ===
        var furniture = new[]
        {
            ("Wooden Bed Frame", "Sturdy bed frame", 399.99m),
            ("Office Chair", "Ergonomic office chair", 249.99m),
            ("Coffee Table Wood", "Modern coffee table", 199.99m),
            ("Sofa 3-Seater", "Comfortable sofa", 799.99m),
            ("Bookshelf", "Wooden bookshelf", 179.99m),
            ("Dining Table", "Dining table set", 449.99m),
            ("Study Desk", "Computer study desk", 299.99m),
            ("TV Stand", "Modern TV stand", 149.99m)
        };

        foreach (var (name, desc, price) in furniture)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryHomeGardenId, SubCategoryFurnitureId));

        // === KITCHEN & DINING (6 products) ===
        var kitchen = new[]
        {
            ("Stainless Steel Cookware", "12-piece cookware set", 199.99m),
            ("Non-Stick Pan Set", "Durable non-stick", 79.99m),
            ("Dinner Plates Set", "12-piece dinner set", 59.99m),
            ("Glass Bowls Set", "Microwave safe bowls", 24.99m),
            ("Knife Set", "Professional knives", 89.99m),
            ("Coffee Maker", "Automatic coffee maker", 99.99m)
        };

        foreach (var (name, desc, price) in kitchen)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryHomeGardenId, SubCategoryKitchenId));

        // === BEDROOM (5 products) ===
        var bedroom = new[]
        {
            ("King Size Bedsheet", "Soft cotton sheets", 49.99m),
            ("Pillow Set", "Memory foam pillows", 79.99m),
            ("Comforter Duvet", "Warm comforter", 99.99m),
            ("Mattress Topper", "Comfort mattress pad", 149.99m),
            ("Bedroom Lamp", "Reading lamp", 34.99m)
        };

        foreach (var (name, desc, price) in bedroom)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryHomeGardenId, SubCategoryBedroomId));

        // === LIVING ROOM (5 products) ===
        var livingRoom = new[]
        {
            ("Wall Shelf", "Floating wall shelf", 39.99m),
            ("Wall Clock", "Modern wall clock", 29.99m),
            ("Decorative Lamp", "Floor standing lamp", 69.99m),
            ("Picture Frame", "Photo frame set", 24.99m),
            ("Throw Pillow", "Decorative cushion", 19.99m)
        };

        foreach (var (name, desc, price) in livingRoom)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryHomeGardenId, SubCategoryLivingRoomId));

        // === GARDEN & OUTDOOR (5 products) ===
        var garden = new[]
        {
            ("Garden Tool Set", "Complete garden tools", 59.99m),
            ("Flower Pots", "Ceramic pots set", 34.99m),
            ("Outdoor Chair", "Weather-resistant chair", 89.99m),
            ("Garden Hose", "Durable garden hose", 29.99m),
            ("Lawn Mower", "Electric lawn mower", 299.99m)
        };

        foreach (var (name, desc, price) in garden)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryHomeGardenId, SubCategoryGardenId));

        // === FITNESS EQUIPMENT (8 products) ===
        var fitness = new[]
        {
            ("Dumbbell Set", "Adjustable dumbbells", 149.99m),
            ("Yoga Mat", "Thick yoga mat", 29.99m),
            ("Treadmill", "Home treadmill", 699.99m),
            ("Kettlebell", "Cast iron kettlebell", 39.99m),
            ("Resistance Bands", "Fitness band set", 19.99m),
            ("Yoga Block", "Cork yoga block", 14.99m),
            ("Exercise Ball", "Stability ball", 24.99m),
            ("Push-up Bars", "Push-up handles", 22.99m)
        };

        foreach (var (name, desc, price) in fitness)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryFitnessId));

        // === OUTDOOR GEAR (6 products) ===
        var outdoor = new[]
        {
            ("Tent 4-Person", "Camping tent", 199.99m),
            ("Hiking Backpack", "60L backpack", 129.99m),
            ("Sleeping Bag", "Warm sleeping bag", 89.99m),
            ("Camping Stove", "Portable stove", 44.99m),
            ("Hiking Boots", "Waterproof boots", 129.99m),
            ("Flashlight LED", "Bright flashlight", 24.99m)
        };

        foreach (var (name, desc, price) in outdoor)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryOutdoorId));

        // === TEAM SPORTS (6 products) ===
        var teamSports = new[]
        {
            ("Football", "Professional soccer ball", 34.99m),
            ("Basketball", "Official basketball", 44.99m),
            ("Cricket Bat", "Wooden cricket bat", 79.99m),
            ("Volleyball", "Volleyball ball", 24.99m),
            ("Hockey Stick", "Field hockey stick", 59.99m),
            ("Baseball Glove", "Leather baseball glove", 89.99m)
        };

        foreach (var (name, desc, price) in teamSports)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryTeamSportsId));

        // === WATER SPORTS (5 products) ===
        var waterSports = new[]
        {
            ("Surfboard", "Fiberglass surfboard", 349.99m),
            ("Snorkel Set", "Diving snorkel set", 49.99m),
            ("Wetsuit", "Thermal wetsuit", 99.99m),
            ("Kickboard", "Swimming kickboard", 19.99m),
            ("Flippers", "Swimming fins", 34.99m)
        };

        foreach (var (name, desc, price) in waterSports)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryWaterSportsId));

        // === YOGA & PILATES (4 products) ===
        var yoga = new[]
        {
            ("Yoga Strap", "Cotton yoga strap", 12.99m),
            ("Meditation Cushion", "Zafu meditation", 39.99m),
            ("Pilates Ring", "Resistance ring", 24.99m),
            ("Yoga Mat Bag", "Mat carrying bag", 19.99m)
        };

        foreach (var (name, desc, price) in yoga)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryYogaId));

        // === CYCLING (5 products) ===
        var cycling = new[]
        {
            ("Mountain Bike", "All-terrain bike", 499.99m),
            ("Bicycle Helmet", "Safety helmet", 79.99m),
            ("Cycling Shoes", "Clip-in cycling shoes", 119.99m),
            ("Bike Lock", "Steel chain lock", 34.99m),
            ("Cycling Gloves", "Padded gloves", 24.99m)
        };

        foreach (var (name, desc, price) in cycling)
            products.Add(CreateProduct(productCode++, name, desc, price, CategorySportsId, SubCategoryCyclingId));

        // === FICTION BOOKS (8 products) ===
        var fiction = new[]
        {
            ("The Great Gatsby", "Classic American novel", 14.99m),
            ("To Kill a Mockingbird", "American classic", 12.99m),
            ("1984", "Dystopian fiction", 13.99m),
            ("Pride and Prejudice", "Romance classic", 11.99m),
            ("The Catcher in the Rye", "Coming of age", 12.99m),
            ("Harry Potter Series", "Fantasy adventure", 89.99m),
            ("The Lord of the Rings", "Epic fantasy", 99.99m),
            ("Dune", "Science fiction", 18.99m)
        };

        foreach (var (name, desc, price) in fiction)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryBooksId, SubCategoryFictionId));

        // === NON-FICTION BOOKS (6 products) ===
        var nonfiction = new[]
        {
            ("Sapiens", "Human history", 18.99m),
            ("Educated", "Memoir", 17.99m),
            ("Thinking, Fast and Slow", "Psychology", 16.99m),
            ("A Brief History of Time", "Science", 15.99m),
            ("The Selfish Gene", "Evolution", 14.99m),
            ("Steve Jobs Biography", "Biography", 16.99m)
        };

        foreach (var (name, desc, price) in nonfiction)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryBooksId, SubCategoryNonfictionId));

        // === EDUCATIONAL BOOKS (6 products) ===
        var educational = new[]
        {
            ("Mathematics Textbook", "Advanced calculus", 89.99m),
            ("Physics Fundamentals", "Physics principles", 79.99m),
            ("Chemistry Guide", "Organic chemistry", 74.99m),
            ("Biology Essentials", "Life sciences", 69.99m),
            ("Programming in C++", "C++ tutorial", 49.99m),
            ("Web Development Guide", "Full stack development", 54.99m)
        };

        foreach (var (name, desc, price) in educational)
            products.Add(CreateProduct(productCode++, name, desc, price, CategoryBooksId, SubCategoryEducationalId));

        return products;
    }

    private static Product CreateProduct(int code, string name, string summary, decimal price, string categoryId, string subCategoryId)
    {
        return new()
        {
            Id = ModelHelpers.GenerateId(),
            ProductCode = $"PROD-{code:D4}",
            Name = name,
            Summary = summary,
            Description = $"High-quality {name}. Perfect for customers looking for premium products. This item combines excellent value with superior craftsmanship and durability. Carefully selected and tested to meet the highest standards.",
            ImageFile = $"product-{code}.png",
            Price = price,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            Balance = new Random().Next(10, 200)
        };
    }
}