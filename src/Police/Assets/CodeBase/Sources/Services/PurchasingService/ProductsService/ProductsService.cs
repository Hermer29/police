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
            { Product.UnitUpgrade, "unitupgrade" }
        };

        public ProductsService(IPurchasesService purchasesService) => _purchasesService = purchasesService;

        public void TryBuy(Product product, Action onSuccess)
        {
            Debug.Log($"{nameof(ProductsService)}.{nameof(TryBuy)} called. Product: {product}");
            _purchasesService.TryBuy(_products[product], onSuccess);
        }
    }
}