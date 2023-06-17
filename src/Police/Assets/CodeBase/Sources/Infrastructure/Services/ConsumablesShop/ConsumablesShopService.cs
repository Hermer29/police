using System;
using Services.PropertyAcquisition;
using Services.PurchasesService.PurchasesWrapper;

namespace Infrastructure.Services
{
    public class ConsumablesShopService
    {
        private readonly IProductsService _productsService;
        private readonly PropertyService _propertyService;

        public ConsumablesShopService(IProductsService productsService, PropertyService propertyService)
        {
            _productsService = productsService;
            _propertyService = propertyService;
        }
        
        public void Buy(ConsumablesShopProduct product)
        {
            _productsService.TryBuy(product.AsProduct(), () =>
            {
                _propertyService.OwnMultiple(product.AsProperty(), GetQuantity(product));
            });
        }

        private int GetQuantity(ConsumablesShopProduct product)
        {
            return product switch
            {
                ConsumablesShopProduct.Nuke1 => 1,
                ConsumablesShopProduct.Nuke10 => 10,
                ConsumablesShopProduct.Nuke30 => 30,
                ConsumablesShopProduct.SuperUnit1 => 1,
                ConsumablesShopProduct.SuperUnit10 => 10,
                ConsumablesShopProduct.SuperUnit30 => 30,
                _ => throw new ArgumentOutOfRangeException(nameof(product), product, null)
            };
        }
    }
}