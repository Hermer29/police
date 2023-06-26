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
        private readonly Dictionary<UnitType, Entry> _unitsAndTheirEntries = new();
        
        class Entry
        {
            public UpgradableUnit _unit;
            public CrossLevelUpgradeEntry _entry;
            public IDisposable _subToUnitChange;
            public IDisposable _subOnUnitMaxUpgrade;
            public IDisposable _subToUnitAppearanceChange;
            public IDisposable _subToUnitLevelUp;
        }
        
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
            {
                _unitsAndTheirEntries.Add(unit.Type, new Entry
                {
                    _entry = entry,
                    _unit = unit
                });
                InitializeEntry(unit, entry);
            }
            
            _initialized = true;
        }

        private void InitializeEntry(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry)
        {
            var currentEntryData = _unitsAndTheirEntries[unit.Type];
            SubscribeOnUnitChange(unit, entry, currentEntryData);
            SubscribeOnAppearanceChange(unit, entry, currentEntryData);
            SubscribeOnLevelUps(unit, entry, currentEntryData);
            entry.ShowIcon(unit.CurrentAppearance.Value.Illustration);
            UpdateInformation(unit, entry);
            if (_service.MaxUpgraded.Contains(unit.Type))
            {
                entry.ShowMaxUpgradeActive(true);
                entry.ShowUpgradeLevel(unit.UpgradedLevel.Value);
                return;
            }
            entry.ShowMaxUpgradeActive(false);
            currentEntryData._subOnUnitMaxUpgrade = _service.MaxUpgraded.ObserveAdd().Subscribe(
                update => OnUnitMaxUpgraded(update, unit));
            entry.UpgradeForCoins.onClick.AddListener(() => OnUpgradeForMoney(unit));
            entry.UpgradeForAds.onClick.AddListener(() => OnUpgradeForAds(unit));
            entry.UpgradeForCurrency.onClick.AddListener(() => OnUpgradeForCurrency(unit));
        }

        private void SubscribeOnUnitChange(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry, Entry data)
        {
            data._subToUnitChange = _service.UsedUnits.ObserveReplace().Where(x
                    => x.Key == unit.Type)
                .Subscribe(updateData => Reinitialize(updateData, entry, data));
        }

        private void SubscribeOnAppearanceChange(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry, Entry data)
        {
            data._subToUnitAppearanceChange = unit.CurrentAppearance.Subscribe(x =>
            {
                if (unit != data._unit)
                    return;
                Debug.Log($"Appearance changed: {unit.name}");
                entry.ShowIcon(x.Illustration);
            });
        }

        private void SubscribeOnLevelUps(PartialUpgradableUnit unit, CrossLevelUpgradeEntry entry, Entry data)
        {
            data._subToUnitLevelUp = unit.UpgradedLevel.Subscribe(x =>
            {
                if (unit != data._unit)
                    return;
                UpdateInformation(unit, entry);
            });
        }

        private void Reinitialize(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> replaceEvent,
            CrossLevelUpgradeEntry entry, Entry data)
        {
            data._subToUnitLevelUp.Dispose();
            data._unit = replaceEvent.NewValue;
            data._subToUnitChange.Dispose();
            data._subOnUnitMaxUpgrade.Dispose();
            data._subToUnitAppearanceChange.Dispose();
            entry.UpgradeForCoins.onClick.RemoveAllListeners();
            entry.UpgradeForAds.onClick.RemoveAllListeners();
            entry.UpgradeForCurrency.onClick.RemoveAllListeners();
            InitializeEntry(replaceEvent.NewValue, entry);
        }

        private void OnUnitMaxUpgraded(CollectionAddEvent<UnitType> type, PartialUpgradableUnit unitType)
        {
            var sameType = type.Value == unitType.Type;
            if (sameType)
            {
                var record = _unitsAndTheirEntries[type.Value];
                record._entry.ShowMaxUpgradeActive(true);
                record._entry.ShowUpgradeLevel((record._unit as PartialUpgradableUnit).UpgradedLevel.Value);
            }
        }

        private void OnUpgradeForCurrency(PartialUpgradableUnit unit) =>
            _purchasesService.TryBuy(Product.UnitUpgrade, 
                () => Upgrade(unit));

        private void OnUpgradeForAds(PartialUpgradableUnit unit) 
            => _advertising.ShowRewarded(
                () => Upgrade(unit));

        private void OnUpgradeForMoney(PartialUpgradableUnit relatedUnit)
        {
            int upgradeCost = CalculateCost(relatedUnit);
            if (_moneyService.TrySpendMoney(upgradeCost))
            {
                Upgrade(relatedUnit);
            }
        }

        private void UpdateInformation(PartialUpgradableUnit unit, CrossLevelUpgradeEntry ui)
        {
            RecalculateUpgradeCost(unit);
            if(unit.IsFullyUpgraded() == false)
                ui.ShowUpgradeLevel(unit.UpgradedLevel.Value);
        }

        private void RecalculateUpgradeCost(UpgradableUnit unit)
        {
            CrossLevelUpgradeEntry ui = _unitsAndTheirEntries[unit.Type]._entry;
            int cost = CalculateCost(unit);
            ui.ShowUpgradeCost(cost);
        }

        private void Upgrade(PartialUpgradableUnit relatedUnit)
        {
            _audio.PlayUpgrade();
            _upgrades.Upgrade(relatedUnit);
        }

        private int CalculateCost(UpgradableUnit relatedUnit) => _costService.Calculate(relatedUnit);
    }
}