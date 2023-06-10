using System;
using Infrastructure.Services.UseService;
using Interface;
using Services.AdvertisingService;
using Services.MoneyService;
using Services.PurchasesService.PurchasesWrapper;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.Services.CostEstimation;
using Upgrading.UnitTypes;
using Zenject;

namespace Upgrading.UI.Barracks
{
    public class BarracksUpgradeModal : MonoBehaviour
    {
        private UnitsUsingService _usingService;
        private IUpgradeCostService _costService;
        private IProductsService _products;
        private IMoneyService _moneyService;
        private IAdvertisingService _advertisingService;
        private UnitsRepository _repository;

        [SerializeField] private LocalizedText _name;
        [SerializeField] private Button _yans;
        [SerializeField] private Button _coins;
        [SerializeField] private Button _ads;
        [SerializeField] private Image _currentUnit;
        [SerializeField] private Image _nextUnit;
        [SerializeField] private Image _currentUnitSmall;
        [SerializeField] private Button _close;
        [SerializeField] private ProgressBlock _progressBlock;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private GameObject[] _disableOnMaxUpgrade;
        [SerializeField] private TMP_Text _maxTierWriting;
        
        private IDisposable _maxUpgradeSubscription;

        [Inject]
        public void Construct(UnitsUsingService usingService, IUpgradeCostService costService, 
            IProductsService products, IMoneyService moneyService, IAdvertisingService advertisingService, 
            UnitsRepository repository)
        {
            _advertisingService = advertisingService;
            _moneyService = moneyService;
            _products = products;
            _costService = costService;
            _usingService = usingService;
            _repository = repository;
            
            _close.onClick.AddListener(Hide);
            gameObject.SetActive(false);
        }
        
        public void Show(PartialUpgradableUnit unit)
        {
            _name.SetKey(unit.LocalizedName);
            gameObject.SetActive(true);
            ShowCurrentUnitIlluistration(unit);
            
            if (_usingService.MaxUpgraded.Contains(unit.Type))
            {
                UpdateInformation(unit);
                SetActiveMaxUpgrade(true);
                return; 
            }
            SetActiveMaxUpgrade(false);
            _maxUpgradeSubscription = _usingService.MaxUpgraded.ObserveAdd().Subscribe(update 
                => OnUnitMaxUpgraded(update, unit));
            _yans.onClick.AddListener(() =>  OnUpgradeForCurrency(unit));
            _ads.onClick.AddListener(() => OnUpgradeForAds(unit));
            _coins.onClick.AddListener(() => OnUpgradeForMoney(unit));
            UpdateInformation(unit);
        }

        private void ShowAppearance(PartialUpgradableUnit unit)
        {
            if (unit.IsFullyUpgraded() == false)
            {
                foreach (GameObject objToDisable in _disableOnMaxUpgrade)
                {
                    objToDisable.SetActive(true);
                }
                if (unit.TryGetNextAppearance(out LevelPartUnits nextAppearance))
                {
                    _nextUnit.sprite = nextAppearance.Illustration;
                    _maxTierWriting.gameObject.SetActive(false);
                    return;
                }
                
                if (HasNextUnit(unit, out PartialUpgradableUnit next))
                {
                    _nextUnit.sprite = next.CurrentAppearance.Value.Illustration;
                    _maxTierWriting.gameObject.SetActive(false);
                    return;
                }
            }

            _progressBlock.WriteAtLast(unit.MaxSuperLevel);
            _nextUnit.sprite = unit.GetLastAppearance().Illustration;
            _maxTierWriting.gameObject.SetActive(true);
            foreach (GameObject objToDisable in _disableOnMaxUpgrade)
            {
                objToDisable.SetActive(false);
            }
        }

        private bool HasNextUnit(PartialUpgradableUnit unit, out PartialUpgradableUnit next)
        {
            return _repository.TryGetNext(unit, out next);
        }

        private void ShowCurrentUnitIlluistration(PartialUpgradableUnit unit)
        {
            _currentUnit.sprite = unit.GetCurrentAppearance().Illustration;
            _currentUnitSmall.sprite = unit.GetCurrentAppearance().Illustration;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _yans.onClick.RemoveAllListeners();
            _ads.onClick.RemoveAllListeners();
            _coins.onClick.RemoveAllListeners();
            _maxUpgradeSubscription?.Dispose();
        }

        private void OnUpgradeForCurrency(PartialUpgradableUnit unit) =>
            _products.TryBuy(Product.UnitUpgrade, 
                () => Upgrade(unit));

        private void Upgrade(PartialUpgradableUnit unit)
        {
            unit.Upgrade();
            UpdateInformation(unit);
        }

        private void OnUpgradeForAds(PartialUpgradableUnit unit) 
            => _advertisingService.ShowRewarded(
                () => Upgrade(unit));

        private void OnUpgradeForMoney(PartialUpgradableUnit relatedUnit)
        {
            int upgradeCost = CalculateCost(relatedUnit);
            if (_moneyService.TrySpendMoney(upgradeCost))
            {
                Upgrade(relatedUnit);
            }
        }

        private void OnUnitMaxUpgraded(CollectionAddEvent<UnitType> type, PartialUpgradableUnit unitType)
        {
            if (type.Value == unitType.Type) 
                SetActiveMaxUpgrade(true);
        }

        private void SetActiveMaxUpgrade(bool state)
        {
            _ads.gameObject.SetActive(!state);
            _coins.gameObject.SetActive(!state);
            _yans.gameObject.SetActive(!state);
        }

        private void UpdateInformation(PartialUpgradableUnit unit)
        {
            ShowUpgradeLevel(unit.UpgradedLevel.Value);
            ShowAppearance(unit);
            RecalculateUpgradeCost(unit);
        }

        private void ShowUpgradeLevel(int upgradedLevelValue)
        {
            _progressBlock.ShowLevel(upgradedLevelValue);
        }

        private void RecalculateUpgradeCost(UpgradableUnit unit)
        {
            int cost = CalculateCost(unit);
            ShowUpgradeCost(cost);
        }

        private void ShowUpgradeCost(int cost)
        {
            _cost.text = cost.ToString();
        }

        private int CalculateCost(UpgradableUnit relatedUnit) => _costService.Calculate(relatedUnit);
    }
}