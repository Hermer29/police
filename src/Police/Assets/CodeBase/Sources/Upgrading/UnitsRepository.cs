using System.Collections.Generic;
using System.Linq;
using Upgrading.AssetManagement;
using Upgrading.UnitTypes;

namespace Upgrading
{
    public class UnitsRepository
    {
        private PartialUpgradableUnit[] _upgradableUnits;
        
        public UnitsRepository(IUnitsAssetLoader loadAllUpgradableUnits)
        {
            _upgradableUnits = loadAllUpgradableUnits.LoadAllUpgradableUnits().ToArray();
        }

        public IEnumerable<PartialUpgradableUnit> GetAll()
        {
            return _upgradableUnits;
        }
    }
}