using System.Collections.Generic;
using System.Linq;
using Services.MoneyService;
using Services.UseService;
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

        private bool _initialized;
        private readonly Dictionary<UpgradableUnit, CrossLevelUpgradeEntry> _unitsAndTheirEntries = new();

        [Inject]
        public void Construct(UnitsRepository repository, UnitsUpgrades upgrades, IUpgradableUnitsFactory factory,
            IUpgradeCostService costService, UnitsUsingService usingService, IMoneyService moneyService)
        {
            _moneyService = moneyService;
            _usingService = usingService;
            _costService = costService;
            _factory = factory;
            _upgrades = upgrades;
        }

        public void Show() => _parent.gameObject.SetActive(true);

        public void Hide() 
            => _parent.gameObject.SetActive(false);

        public void LoadListWithData()
        {
            if (_initialized)
                return;
            
            var entriesAndCorrespondingUnits =
                _entries.Zip(_usingService.UsedUnits.Values, (entry, unit) => (entry, unit));
            foreach ((CrossLevelUpgradeEntry entry, PartialUpgradableUnit unit) in entriesAndCorrespondingUnits)
            {
                entry.UpgradeForCoins.onClick.AddListener(() => OnUpgrade(unit));
                _unitsAndTheirEntries.Add(unit, entry);
                var cost = CalculateCost(unit);
                UpdateInformation(unit, entry, cost);
            }
            _initialized = true;
        }

        private void UpdateInformation(UpgradableUnit unit, CrossLevelUpgradeEntry ui, int upgradeCost)
        {
            ui.ShowUpgradeCost(upgradeCost);
            ui.ShowUpgradeLevel(unit.UpgradedLevel.Value);
        }

        private void OnUpgrade(UpgradableUnit relatedUnit)
        {
            int upgradeCost = CalculateCost(relatedUnit);
            if (_moneyService.TrySpendMoney(upgradeCost))
            {
                _upgrades.Upgrade(relatedUnit);
                UpdateInformation(relatedUnit, _unitsAndTheirEntries[relatedUnit], upgradeCost);
            }
        }

        private int CalculateCost(UpgradableUnit relatedUnit) => _costService.Calculate(relatedUnit);
    }
}