using System.Collections.Generic;
using System.Linq;
using Shop;
using SpecialPlatforms;
using Upgrading.AssetManagement;
using Upgrading.UnitTypes;

namespace Upgrading
{
    public class UnitsRepository
    {
        private PartialUpgradableUnit[] _upgradableUnits;

        public UnitsRepository(IUnitsAssetLoader loadAllUpgradableUnits, SaveService saveService)
        {
            _upgradableUnits = loadAllUpgradableUnits.LoadAllUpgradableUnits().ToArray();
            foreach (ISavableData partialUpgradableUnit in _upgradableUnits.Cast<ISavableData>())
                saveService.Bind(partialUpgradableUnit);
        }

        public IEnumerable<PartialUpgradableUnit> GetAll()
        {
            return _upgradableUnits;
        }
    }
}