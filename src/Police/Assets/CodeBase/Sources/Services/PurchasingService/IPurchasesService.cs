using System;

namespace Services.PurchasesService
{
    public interface IPurchasesService
    {
        void TryBuy(string productId, Action onBought);
    }
}