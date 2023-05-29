using System.Collections.Generic;
using UnityEngine;
using Upgrading.UI.CrossLevels;
using Upgrading.UnitTypes;

namespace Upgrading.AssetManagement
{
    public interface IUnitsAssetLoader
    {
        IEnumerable<PartialUpgradableUnit> LoadAllUpgradableUnits();
        CrossLevelUpgradeEntry InstantiateCrossLevelUpgradeEntry(Transform parent);
    }
}