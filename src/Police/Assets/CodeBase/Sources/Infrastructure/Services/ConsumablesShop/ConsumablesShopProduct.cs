using System;
using Services.PropertyAcquisition;
using Services.PurchasesService.PurchasesWrapper;

namespace Infrastructure.Services
{
    public enum ConsumablesShopProduct
    {
        SuperUnit1,
        SuperUnit10,
        SuperUnit30,
        Nuke1,
        Nuke10,
        Nuke30
    }

    public static class ConsumablesShopProductExtensions
    {
        public static Product AsProduct(this ConsumablesShopProduct shopProduct)
        {
            return shopProduct switch
            {
                ConsumablesShopProduct.SuperUnit1 => Product.SuperUnit1,
                ConsumablesShopProduct.SuperUnit10 => Product.SuperUnit10,
                ConsumablesShopProduct.SuperUnit30 => Product.SuperUnit30,
                ConsumablesShopProduct.Nuke1 => Product.Nuke1,
                ConsumablesShopProduct.Nuke10 => Product.Nuke10,
                ConsumablesShopProduct.Nuke30 => Product.Nuke30,
                _ => throw new ArgumentOutOfRangeException(nameof(shopProduct), shopProduct, null)
            };
        }

        public static Property AsProperty(this ConsumablesShopProduct shopProduct)
        {
            return shopProduct switch
            {
                ConsumablesShopProduct.SuperUnit1 => Property.SuperUnit,
                ConsumablesShopProduct.SuperUnit10 => Property.SuperUnit,
                ConsumablesShopProduct.SuperUnit30 => Property.SuperUnit,
                ConsumablesShopProduct.Nuke1 => Property.Nuke,
                ConsumablesShopProduct.Nuke10 => Property.Nuke,
                ConsumablesShopProduct.Nuke30 => Property.Nuke,
                _ => throw new ArgumentOutOfRangeException(nameof(shopProduct), shopProduct, null)
            };
        }
    }
}