using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.UseService;
using Services.AdvertisingService;
using Services.MoneyService;
using Services.PurchasesService.PurchasesWrapper;
using UniRx;
using UnityEngine;
using Upgrading.Factory;
using Upgrading.Services.CostEstimation;
using Upgrading.UnitTypes;
using Zenject;

namespace Upgrading.UI.CrossLevels
{
    public class CrossLevelUpgradePresenter : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private CrossLevelUpgradeEntry[] _entries;
        
        private UnitsUpgrades _upgrades;
        private IUpgradableUnitsFactory _factory;
        private IUpgradeCostService _costService;
        private UnitsUsingService _usingService;
        private IMoneyService _moneyService;
        private IAdvertisingService _advertising;
        private IProductsService _purchasesService;

        private bool _initialized;
        private readonly Dictionary<UpgradableUnit, CrossLevelUpgradeEntry> _unitsAndTheirEntries = new();

        [Inject]
        public void Construct(UnitsRepository repository, UnitsUpgrades upgrades, IUpgradableUnitsFactory factory,
            IUpgradeCostService costService, UnitsUsingService usingService, IMoneyService moneyService, IAdvertisingService advertisingService, 
            IProductsService purchasesService)
        {
            _purchasesService = purchasesService;
            _advertising = advertisingService;
            _moneyService = moneyService;
            _usingService = usingService;
            _costService = costService;
            _factory = factory;
            _upgrades = upgrades;
        }

        public void Show() 
            => _parent.gameObject.SetActive(true);

        public void Hide() 
            => _parent.gameObject.SetActive(false);

        public void LoadListWithData()
        {
            if (_initialized)
                return;
            
            var entriesAndCorrespondingUnits =
                _entries.Zip(_usingService.UsedUnits.Values, (entry, unit) => (entry, unit));
            foreach ((CrossLevelUpgradeEntry entry, PartialUpgradableUnit unit) in entriesAndCorrespondingUnits)
                InitializeEntry(unit, entry);
            
            _initialized = true;
        }

        private void InitializeEntry(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry)
        {
            _unitsAndTheirEntries.Add(unit, entry);
            if (_usingService.MaxUpgraded.Contains(unit.Type))
            {
                UpdateInformation(unit, entry);
                entry.ShowMaxUpgradeActive(true);
                return;
            }
            entry.ShowMaxUpgradeActive(false);
            _usingService.MaxUpgraded.ObserveAdd().Subscribe(update 
                => OnUnitMaxUpgraded(update, unit));
            entry.UpgradeForCoins.onClick.AddListener(() => OnUpgradeForMoney(unit));
            entry.UpgradeForAds.onClick.AddListener(() => OnUpgradeForAds(unit));
            entry.UpgradeForCurrency.onClick.AddListener(() => OnUpgradeForCurrency(unit));
            UpdateInformation(unit, entry);
        }

        private void OnUnitMaxUpgraded(CollectionAddEvent<UnitType> type, PartialUpgradableUnit unitType)
        {
            if (type.Value == unitType.Type) 
                _unitsAndTheirEntries[unitType].ShowMaxUpgradeActive(true);
        }

        private void OnUpgradeForCurrency(PartialUpgradableUnit unit) =>
            _purchasesService.TryBuy(Product.UnitUpgrade, 
                () => Upgrade(unit));

        private void OnUpgradeForAds(PartialUpgradableUnit unit) 
            => _advertising.ShowRewarded(
                () => Upgrade(unit));

        private void OnUpgradeForMoney(UpgradableUnit relatedUnit)
        {
            int upgradeCost = CalculateCost(relatedUnit);
            if (_moneyService.TrySpendMoney(upgradeCost))
            {
                Upgrade(relatedUnit);
            }
        }

        private void UpdateInformation(UpgradableUnit unit, CrossLevelUpgradeEntry ui)
        {
            RecalculateUpgradeCost(unit);
            ui.ShowUpgradeLevel(unit.UpgradedLevel.Value);
        }

        private void RecalculateUpgradeCost(UpgradableUnit unit)
        {
            CrossLevelUpgradeEntry ui = _unitsAndTheirEntries[unit];
            int cost = CalculateCost(unit);
            ui.ShowUpgradeCost(cost);
        }

        private void Upgrade(UpgradableUnit relatedUnit)
        {
            _upgrades.Upgrade(relatedUnit);
            UpdateInformation(relatedUnit, _unitsAndTheirEntries[relatedUnit]);
        }

        private int CalculateCost(UpgradableUnit relatedUnit) => _costService.Calculate(relatedUnit);
    }
}