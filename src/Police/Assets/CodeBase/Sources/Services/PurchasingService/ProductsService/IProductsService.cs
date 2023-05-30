using System;

namespace Services.PurchasesService.PurchasesWrapper
{
    public interface IProductsService
    {
        public void TryBuy(Product product, Action onSuccess);
    }
}