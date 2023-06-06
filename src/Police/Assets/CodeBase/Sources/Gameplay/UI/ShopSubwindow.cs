using Interface;
using Services.MoneyService;
using Services.PrefsService;
using Services.PurchasesService.PurchasesWrapper;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    internal class ShopSubwindow : MonoBehaviour
    {
        private IProductsService _products;
        private IPrefsService _prefs;
        private IMoneyService _moneyService;
        
        [SerializeField] private ButtonWithTint _buyStarterPack;
        [SerializeField] private ButtonWithTint _buyFirePack;

        [Inject]
        public void Construct(IProductsService productsService, IPrefsService prefsService, IMoneyService moneyService)
        {
            _moneyService = moneyService;
            _prefs = prefsService;
            _products = productsService;
            
            _buyStarterPack.Button.onClick.AddListener(BuyStarterPack);
            _buyFirePack.Button.onClick.AddListener(BuyFirePack);

            Initialize();
        }

        private void Initialize()
        {
            DetermineIfShowingTintIsRequired(Product.FirePack, _buyFirePack);
            DetermineIfShowingTintIsRequired(Product.StarterPack, _buyStarterPack);
        }

        private void DetermineIfShowingTintIsRequired(Product product, ButtonWithTint buttonWithTint)
        {
            if (IsProductBought(product)) buttonWithTint.ShowTint();
        }

        private bool IsProductBought(Product product) 
            => _prefs.GetInt(ConstructBoughtKey(product), 0) == 1;

        private void BuyFirePack() => BuyProduct(Product.FirePack, _buyFirePack, new ProductReward
        {
            Coins = 50000
        });

        private void BuyStarterPack() => BuyProduct(Product.StarterPack, _buyStarterPack, new ProductReward
        {
            Coins = 2000
        });

        private void BuyProduct(Product product, ButtonWithTint buttonWithTint, ProductReward productReward)
        {
            _products.TryBuy(product, () =>
            {
                _moneyService.AddMoney(productReward.Coins);
                _prefs.SetInt(ConstructBoughtKey(product), 1);
                buttonWithTint.ShowTint();
            });
        }

        private static string ConstructBoughtKey(Product product) 
            => $"{product}_IsBought";

        public void Show() => GetComponent<GameObjectActivationBaker>().SetActive(true);

        public void Hide() => GetComponent<GameObjectActivationBaker>().SetActive(false);
    }
}