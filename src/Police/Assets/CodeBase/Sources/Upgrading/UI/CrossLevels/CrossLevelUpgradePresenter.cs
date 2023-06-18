using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Audio;
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
        private UsedUnitsService _service;
        private IMoneyService _moneyService;
        private IAdvertisingService _advertising;
        private IProductsService _purchasesService;
        private GlobalAudio _audio;

        private bool _initialized;
        private readonly Dictionary<UnitType, (UpgradableUnit, CrossLevelUpgradeEntry)> _unitsAndTheirEntries = new();
        private IDisposable _subToUnitChange;
        private IDisposable _subOnUnitMaxUpgrade;
        private IDisposable _subToUnitAppearanceChange;

        [Inject]
        public void Construct(UnitsRepository repository, UnitsUpgrades upgrades, IUpgradableUnitsFactory factory,
            IUpgradeCostService costService, UsedUnitsService service, IMoneyService moneyService, IAdvertisingService advertisingService, 
            IProductsService purchasesService, GlobalAudio audio)
        {
            _audio = audio;
            _purchasesService = purchasesService;
            _advertising = advertisingService;
            _moneyService = moneyService;
            _service = service;
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

            var partialUpgradableUnits = _service.UsedUnits.Select(x => x.Value);
            var entriesAndCorrespondingUnits =
                _entries.Zip(partialUpgradableUnits, (entry, unit) => (entry, unit));
            foreach ((CrossLevelUpgradeEntry entry, PartialUpgradableUnit unit) in entriesAndCorrespondingUnits)
                InitializeEntry(unit, entry);
            
            _initialized = true;
        }

        private void InitializeEntry(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry)
        {
            _subToUnitChange = _service.UsedUnits.ObserveReplace().Where(x 
                    => x.Key == unit.Type)
                .Subscribe(updateData => Reinitialize(updateData, entry));

            _subToUnitAppearanceChange = unit.CurrentAppearance.Subscribe(x =>
            {
                Debug.Log($"Appearance changed: {unit.name}");
                entry.ShowIcon(x.Illustration);
            });
            
            entry.ShowIcon(unit.CurrentAppearance.Value.Illustration);
            if (_unitsAndTheirEntries.ContainsKey(unit.Type) == false)
            {
                _unitsAndTheirEntries.Add(unit.Type, (unit, entry));
            }
            if (_service.MaxUpgraded.Contains(unit.Type))
            {
                UpdateInformation(unit, entry);
                entry.ShowMaxUpgradeActive(true);
                return;
            }
            entry.ShowMaxUpgradeActive(false);
            _subOnUnitMaxUpgrade = _service.MaxUpgraded.ObserveAdd().Subscribe(
                update => OnUnitMaxUpgraded(update, unit));
            entry.UpgradeForCoins.onClick.AddListener(() => OnUpgradeForMoney(unit));
            entry.UpgradeForAds.onClick.AddListener(() => OnUpgradeForAds(unit));
            entry.UpgradeForCurrency.onClick.AddListener(() => OnUpgradeForCurrency(unit));
            UpdateInformation(unit, entry);
        }

        public void Reinitialize(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> replaceEvent, CrossLevelUpgradeEntry entry)
        {
            _unitsAndTheirEntries[replaceEvent.Key] = (replaceEvent.NewValue, entry);
            _subToUnitChange.Dispose();
            _subOnUnitMaxUpgrade.Dispose();
            _subToUnitAppearanceChange.Dispose();
            entry.UpgradeForCoins.onClick.RemoveAllListeners();
            entry.UpgradeForAds.onClick.RemoveAllListeners();
            entry.UpgradeForCurrency.onClick.RemoveAllListeners();
            InitializeEntry(replaceEvent.NewValue, entry);
        }

        private void OnUnitMaxUpgraded(CollectionAddEvent<UnitType> type, PartialUpgradableUnit unitType)
        {
            if (type.Value == unitType.Type) 
                _unitsAndTheirEntries[type.Value].Item2.ShowMaxUpgradeActive(true);
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
            CrossLevelUpgradeEntry ui = _unitsAndTheirEntries[unit.Type].Item2;
            int cost = CalculateCost(unit);
            ui.ShowUpgradeCost(cost);
        }

        private void Upgrade(UpgradableUnit relatedUnit)
        {
            _audio.PlayUpgrade();
            _upgrades.Upgrade(relatedUnit);
            UpdateInformation(relatedUnit, _unitsAndTheirEntries[relatedUnit.Type].Item2);
        }

        private int CalculateCost(UpgradableUnit relatedUnit) => _costService.Calculate(relatedUnit);
    }
}