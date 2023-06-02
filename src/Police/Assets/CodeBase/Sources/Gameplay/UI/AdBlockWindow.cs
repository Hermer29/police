using Services.AdvertisingService.AdBlocking;
using Services.PurchasesService.PurchasesWrapper;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class AdBlockWindow : MonoBehaviour, IInitializable
    {
        private BlockingAdsAdvertisingDecorator _blocker;
        private IProductsService _products;
        
        [SerializeField] private MoveObjectFromToAnimation _animation;
        [SerializeField] private Button _buy;

        [Inject]
        public void Construct(IProductsService products, BlockingAdsAdvertisingDecorator advertisingDecorator)
        {
            _blocker = advertisingDecorator;
            _products = products;

            Debug.Log($"{nameof(AdBlockWindow)} constructed");
            
            Initialize();
        }

        public void Reposition()
        {
            if (gameObject.activeInHierarchy == false)
                return;
            _animation.Execute();
        }

        public void Initialize()
        {
            Debug.Log($"{nameof(AdBlockWindow)}.{nameof(Initialize)} called, Advertising Disabled: {_blocker.IsAdvertisingDisabled}");
            if (_blocker.IsAdvertisingDisabled)
            {
                Hide();
                return;
            }
            _buy.onClick.AddListener(OnBuy);
        }

        private void OnBuy() => _products.TryBuy(Product.AdvertisingDisable, OnBought);

        private void OnBought()
        {
            Hide();
            _blocker.DisableAdvertising();
        }

        private void Hide() => gameObject.SetActive(false);
    }
}