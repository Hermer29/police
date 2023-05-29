using Upgrading.UnitTypes;

namespace CodeBase.Tests
{
    public static class Setup
    {
        public static PartialUpgradableUnit PartialUpgradeUnit()
        {
            var upgradable = Create.PartialUpgradableUnit();
            upgradable.LevelPartsUnitAppearances = new LevelPartUnits[4];
            upgradable.LevelPartsUnitAppearances[0] = new LevelPartUnits();
            upgradable.LevelPartsUnitAppearances[1] = new LevelPartUnits();
            upgradable.Initialize();
            return upgradable;
        }
    }
}