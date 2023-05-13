// using System;
// using DefaultNamespace.Skins;
// using Services;
//
// namespace Skins
// {
//     public class ShopProductPresenter
//     {
//         private readonly ShopEntry _shopEntry;
//         private readonly IShopProduct _product;
//         private readonly PropertyService _propertyService;
//
//         public event Action PlayerTriedToBuyAndInsufficientFunds;
//         public event Action PlayerTriedToBuyAndIapRaisedError;
//         
//         public ShopProductPresenter(ShopEntry shopEntry, IShopProduct product, PropertyService propertyService)
//         {
//             _shopEntry = shopEntry;
//             _product = product;
//             _propertyService = propertyService;
//
//             _shopEntry.Use.onClick.AddListener(OnUseClicked);
//             _shopEntry.BuyForCoins.onClick.AddListener(OnBuyWithCoins);
//             _shopEntry.BuyForYans.onClick.AddListener(OnBuyWithYans);
//
//             if (propertyService.IsOwned(_product))
//             {
//                 if (_usingService.IsSavedUsed(_product))
//                 {
//                     _shopEntry.Use.interactable = false;
//                 }
//                 MarkAsBought();
//             }
//             else
//             {
//                 MarkAsUnbought();
//             }
//
//             _propertyService.PropertyOwned += UpdateRequested;
//             _shopEntry.ApplyPositionDeltaSize(_product.OverrideEntryDeltaSize);
//             _usingService.Used += UpdateRequested;
//         }
//
//         private void OnUse()
//         {
//             if (_propertyService.IsOwned(_product) == false)
//                 throw new InvalidOperationException("Trying to use item, when it is not bought");
//             
//             _usingService.Use(_product);
//             _shopEntry.Use.interactable = false;
//         }
//
//         private void OnUseClicked()
//         {
//             OnUse();
//         }
//
//         private void UpdateRequested(IAcquirable obj)
//         {
//             if (obj.Guid == _product.Guid)
//             {
//                 MarkAsBought();
//             }
//         }
//
//         private void OnBuyWithYans()
//         {
//             _iapService.TryBuy(
//                 _product as IYandexIapProduct, 
//                 onSuccess: OwnProduct, 
//                 onError: () => PlayerTriedToBuyAndIapRaisedError?.Invoke());
//         }
//
//         private void OwnProduct()
//         {
//             _propertyService.Own(_product);
//             MarkAsBought();
//             OnUse();
//         }
//
//         private void MarkAsBought()
//         {
//             _shopEntry.SetActiveBuyingBlock(false);
//             _shopEntry.SetActiveUsingBlock(true);
//         }
//
//         private void MarkAsUnbought()
//         {
//             _shopEntry.SetActiveBuyingBlock(true);
//             _shopEntry.SetActiveUsingBlock(false);
//         }
//
//         private void OnBuyWithCoins()
//         {
//             // if (_moneyService.HasEnoughCoins(_product.CoinsCost) == false)
//             // {
//             //     PlayerTriedToBuyAndInssufficientFunds?.Invoke();
//             //     return;
//             // }
//             //
//             // _moneyService.Spend(_product.CoinsCost);
//             OwnProduct();
//         }
//     }
// }