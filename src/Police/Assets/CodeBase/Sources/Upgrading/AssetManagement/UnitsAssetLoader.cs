using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upgrading.UI.CrossLevels;
using Upgrading.UnitTypes;

namespace Upgrading.AssetManagement
{
    public class UnitsAssetLoader : IUnitsAssetLoader
    {
        public IEnumerable<PartialUpgradableUnit> LoadAllUpgradableUnits()
        {
            var resources = Resources.LoadAll<UpgradableUnit>("UpgradableUnits").Cast<PartialUpgradableUnit>();
            
            return resources;
        }

        public CrossLevelUpgradeEntry InstantiateCrossLevelUpgradeEntry(Transform parent)
        {
            return Object.Instantiate(
                original: Resources.Load<CrossLevelUpgradeEntry>("Prefabs/UI/CrossLevelUpgradeEntryv2"),
                parent: parent);
        }
    }
}