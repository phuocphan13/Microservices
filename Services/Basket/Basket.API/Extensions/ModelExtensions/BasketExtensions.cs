using ApiClient.Basket.Models;
using ApiClient.Discount.Enum;
using Basket.API.Entitites;

namespace Basket.API.Extensions.ModelExtensions;

//Toda: Should re-think about Apply Discount and Coupon. When to apply them, and how to apply them
//Should apply right at the time checkout, or apply right at the time add item to basket
public static class ShoppingBasketExtensions
{
    public static BasketDetail ToDetail(this Entitites.Basket entity)
    {
        var detail = new BasketDetail
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            TotalPrice = entity.TotalPrice,
            DiscountItems = entity.Discounts.Select(x => new DiscountItemSummary()
            {
                Amount = x.Amount,
                CatalogCode = x.CatalogCode,
                Type = x.Type
            }).ToList(),
            CouponItems = entity.Coupons.Select(x => new CouponItemSummary()
            {
                Amount = x.Amount,
                CatalogCode = x.CatalogCode,
                Type = x.Type
            }).ToList()
        };

        foreach (var item in entity.Items)
        {
            var summary = new BasketItemSummary()
            {
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                // Price = item.CalculateItemPrice(entity.Discounts, entity.Coupons)
                Price = item.Price
            };
            
            detail.Items.Add(summary);
        }

        return detail;
    }
    
    public static BasketSummary ToSummary(this Entitites.Basket entity)
    {
        var detail = new BasketSummary
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            TotalPrice = entity.TotalPrice
        };

        foreach (var item in entity.Items)
        {
            var summary = new BasketItemSummary()
            {
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                // Price = item.CalculateItemPrice(entity.Discounts, entity.Coupons
                Price = item.Price
            };

            detail.Items.Add(summary);
        }

        return detail;
    }

    public static Entitites.Basket ToEntityFromSave(this SaveBasketRequestBody requestBody)
    {
        return new()
        {
            UserId = requestBody.UserId,
            SessionDate = DateTime.UtcNow,
            Items = requestBody.Items.Select(x => new BasketItem()
            {
                Price = x.Price,
                ProductCode = x.ProductCode,
                Quantity = x.Quantity,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestBody.UserId
            }).ToList(),
            Discounts = requestBody.Discounts.Select(x => new DiscountItem()
            {
                Type = x.Type,
                CatalogCode = x.CatalogCode,
                Amount = x.Amount,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestBody.UserId
            }).ToList(),
            Coupons = requestBody.Counpons.Select(x => new CouponItem()
            {
                Type = x.Type,
                CatalogCode = x.CatalogCode,
                Amount = x.Amount,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestBody.UserId
            }).ToList()
        };
    }

    public static void ToEntityFromUpdate(this Entitites.Basket entity, SaveBasketRequestBody basket)
    {
        entity.SessionDate = DateTime.UtcNow;

        foreach (var item in basket.Items)
        {
            if (item.IsRemove)
            {
                entity.Items.RemoveAll(x => x.ProductCode == item.ProductCode);
            }
            else
            {
                var basketItem = entity.Items.FirstOrDefault(x => x.ProductCode == item.ProductCode);

                if (basketItem is not null)
                {
                    basketItem.Quantity = item.Quantity;
                    basketItem.Price = item.Price;
                    basketItem.LastModifiedDate = DateTime.Now;
                }
                else
                {
                    entity.Items.Add(new BasketItem
                    {
                        Price = item.Price,
                        ProductCode = item.ProductCode,
                        Quantity = item.Quantity
                    });
                }
            }
        }

        basket.Discounts = basket.Discounts;
        basket.Counpons = basket.Counpons;
    }
    
    public static decimal CalculateItemPrice(this BasketItem item, List<DiscountItem> discounts, List<CouponItem> coupons)
    {
        var price = item.Price;

        var discount = discounts.Where(x => (x.Type == DiscountEnum.Category && x.CatalogCode == item.CategoryCode) ||
                                            (x.Type == DiscountEnum.SubCategory && x.CatalogCode == item.SubCategoryCode) ||
                                            (x.Type == DiscountEnum.Product && x.CatalogCode == item.ProductCode));

        if (discount.Any())
        {
            price -= discount.Sum(x => x.Amount);
        }

        var couponApplieds = coupons.Where(x => x.CatalogCode == item.ProductCode);

        if (couponApplieds is not null && couponApplieds.Any())
        {
            foreach (var coupon in couponApplieds)
            {
                if (coupon.Type == CouponEnum.Fixed)
                {
                    price -= coupon.Amount;
                }
                else
                {
                    price -= price * coupon.Amount / 100;
                }
            }
        }

        return price;
    }
}
