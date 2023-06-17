using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PurchasesService.PurchasesWrapper
{
    public class ProductsService : IProductsService
    {
        private readonly IPurchasesService _purchasesService;
        
        private Dictionary<Product, string> _products = new()
        {
            { Product.FirePack, "firepack" },
            { Product.StarterPack, "starterpack" },
            { Product.AdvertisingDisable, "adblock" },
            { Product.UnitUpgrade, "unitupgrade" },
            { Product.Coins500, "coins500" },
            { Product.Coins2000, "coins2000" },
            { Product.Coins10000, "coins10000" },
            {Product.Nuke1, "nuke1"},
            {Product.Nuke10, "nuke10"},
            { Product.Nuke30, "nuke30"},
            { Product.SuperUnit1, "superunit1"},
            { Product.SuperUnit10, "superunit10"},
            {Product.SuperUnit30, "superunit30"},
        };

        public ProductsService(IPurchasesService purchasesService) => _purchasesService = purchasesService;

        public void TryBuy(Product product, Action onSuccess)
        {
            Debug.Log($"{nameof(ProductsService)}.{nameof(TryBuy)} called. Product: {product}");
            _purchasesService.TryBuy(_products[product], onSuccess);
        }
    }
}