using MathUtility;
using UnityEngine;

namespace Upgrading.UnitTypes.Stats
{
    public class UpgradableUnitStats
    {
        private readonly UpgradableUnit _unit;

        public UpgradableUnitStats(UpgradableUnit unit)
        {
            _unit = unit;
        }

        public int Current(Stat stat)
        {
            return (int)Mathf.Round(Functions.Polynomial(
                x: _unit.UpgradedLevel.Value + _unit.Tier * UpgradeUnitsConstants.LevelsToUpgradePart,
                n: StatsEvaluationConstants.GrowMultiplier,
                add: StatsEvaluationConstants.BaseStats));
        }

        public int Next(Stat stat)
        {
            return (int)Mathf.Round(Functions.Polynomial(
                x: _unit.UpgradedLevel.Value + _unit.Tier * UpgradeUnitsConstants.LevelsToUpgradePart,
                n: StatsEvaluationConstants.GrowMultiplier,
                add: StatsEvaluationConstants.BaseStats));
        }
    }
}