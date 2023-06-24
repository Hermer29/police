using System.Collections.Generic;
using System.Linq;
using Helpers;
using Shop;
using SpecialPlatforms;
using Upgrading.AssetManagement;
using Upgrading.UnitTypes;

namespace Upgrading
{
    public class UnitsRepository : IManuallyInitializable
    {
        private readonly IUnitsAssetLoader _loadAllUpgradableUnits;
        private readonly SaveService _saveService;
        private PartialUpgradableUnit[] _upgradableUnits;

        public UnitsRepository(IUnitsAssetLoader loadAllUpgradableUnits, SaveService saveService)
        {
            _loadAllUpgradableUnits = loadAllUpgradableUnits;
            _saveService = saveService;
        }

        void IManuallyInitializable.Initialize()
        {
            _upgradableUnits = _loadAllUpgradableUnits.LoadAllUpgradableUnits().ToArray();
            foreach (PartialUpgradableUnit partialUpgradableUnit in _upgradableUnits)
            {
                _saveService.Bind(partialUpgradableUnit);
                partialUpgradableUnit.Initialize();
            }
        }

        public IEnumerable<PartialUpgradableUnit> GetAll() => _upgradableUnits;

        public IEnumerable<PartialUpgradableUnit> GetWithType(UnitType unit) 
            => GetAll().Where(x => x.Type == unit);

        public IEnumerable<PartialUpgradableUnit> GetOrderedWithType(UnitType unit) 
            => GetWithType(unit).OrderByLogic();

        public bool TryGetNext(PartialUpgradableUnit unit, out PartialUpgradableUnit next)
        {
            next = GetOrderedWithType(unit.Type).GetNext(unit);
            return next != null;
        }
    }
}