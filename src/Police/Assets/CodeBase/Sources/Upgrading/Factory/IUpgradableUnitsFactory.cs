using UnityEngine;
using Upgrading.UI.CrossLevels;
using Upgrading.UnitTypes;

namespace Upgrading.Factory
{
    public interface IUpgradableUnitsFactory
    {
        CrossLevelUpgradeEntry CreateCrossLevelEntry(Transform parent, UpgradableUnit unit);
    }
}