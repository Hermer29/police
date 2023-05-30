using System.Collections.Generic;
using System.Linq;
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

        private bool _initialized;
        private readonly Dictionary<UpgradableUnit, CrossLevelUpgradeEntry> _unitsAndTheirEntries = new();

        [Inject]
        public void Construct(UnitsRepository repository, UnitsUpgrades upgrades, IUpgradableUnitsFactory factory,
            IUpgradeCostService costService, UnitsUsingService usingService)
        {
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
                UpdateInformation(unit, entry);
            }
            _initialized = true;
        }

        private void UpdateInformation(UpgradableUnit unit, CrossLevelUpgradeEntry ui)
        {
            int upgradeCost = _costService.Calculate(unit);
            ui.ShowUpgradeCost(upgradeCost);
            ui.ShowUpgradeLevel(unit.UpgradedLevel.Value);
        }

        private void OnUpgrade(UpgradableUnit relatedUnit)
        {
            UpdateInformation(relatedUnit, _unitsAndTheirEntries[relatedUnit]);
            _upgrades.Upgrade(relatedUnit);
        }
    }
}