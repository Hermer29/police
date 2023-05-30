using System;
using UnityEngine;

namespace Services.PurchasesService
{
    public class FakePurchasesService : IPurchasesService
    {
        public void TryBuy(string productId, Action onBought)
        {
            Debug.Log($"{nameof(FakePurchasesService)}.{nameof(TryBuy)} called");
            onBought?.Invoke();
        }
    }
}