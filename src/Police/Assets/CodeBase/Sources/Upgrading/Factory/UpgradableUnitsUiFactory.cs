using UnityEngine;
using Upgrading.AssetManagement;
using Upgrading.UI.CrossLevels;
using Upgrading.UnitTypes;

namespace Upgrading.Factory
{
    public class UpgradableUnitsUiFactory : IUpgradableUnitsFactory
    {
        private readonly IUnitsAssetLoader _unitsAssetLoader;

        public UpgradableUnitsUiFactory(IUnitsAssetLoader unitsAssetLoader) 
            => _unitsAssetLoader = unitsAssetLoader;

        public CrossLevelUpgradeEntry CreateCrossLevelEntry(Transform parent, UpgradableUnit unit)
        {
            var entry = _unitsAssetLoader.InstantiateCrossLevelUpgradeEntry(parent);
            entry.Construct(unit);
            return entry;
        }
    }
}