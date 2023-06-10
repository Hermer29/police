using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Upgrading.UnitTypes
{
    public static class UnitTypesExtensions
    {
        public static IEnumerable<PartialUpgradableUnit> OrderByLogic(this IEnumerable<PartialUpgradableUnit> units)
        {
            var upgradableUnits = units as PartialUpgradableUnit[] ?? units.ToArray();
            if (upgradableUnits.Length == 1)
                return upgradableUnits;

            var queue = new List<PartialUpgradableUnit>();
            PartialUpgradableUnit startingUnit = GetUnitStartingUpgradeChain(upgradableUnits);
            queue.Add(startingUnit);
            var otherUnpurchasableUnits = NonStartingEvolvingUnits(upgradableUnits).ToList();
            
            PartialUpgradableUnit tail = startingUnit;

            while (otherUnpurchasableUnits.Count != 0)
            {
                EvolvedUpgradableUnit next = otherUnpurchasableUnits.FirstOrDefault(x => x.PreviousUnit.Guid == tail.Guid);
                otherUnpurchasableUnits.Remove(next);
                queue.Add(next);
                tail = next;
            }
            queue.AddRange(upgradableUnits.Where(x => x.Purchasable));
            return queue;
        }

        public static PartialUpgradableUnit GetCurrentFromOrdered(this IEnumerable<PartialUpgradableUnit> orderedUnits)
        {
            var current = (PartialUpgradableUnit) null;
            foreach (PartialUpgradableUnit orderedUnit in orderedUnits)
            {
                if (orderedUnit.IsFullyUpgraded())
                {
                    current = orderedUnit;
                    continue;
                }

                return orderedUnit;
            }
            return current;
        }

        private static IEnumerable<EvolvedUpgradableUnit> NonStartingEvolvingUnits(PartialUpgradableUnit[] units) =>
            units.Where(x => x is EvolvedUpgradableUnit)
                .Cast<EvolvedUpgradableUnit>();

        private static PartialUpgradableUnit GetUnitStartingUpgradeChain(PartialUpgradableUnit[] partialUpgradableUnits) 
            => partialUpgradableUnits.First(x => x.OwnedByDefault);

        public static PartialUpgradableUnit GetNext(this IEnumerable<PartialUpgradableUnit> orderedUnits, PartialUpgradableUnit previous)
        {
            var asArr = orderedUnits as PartialUpgradableUnit[] ?? orderedUnits.ToArray();
            int targetIndex = asArr.IndexOf(previous);
            bool isNextIndexIsOutOfRange = asArr.Length == targetIndex + 1;
            return isNextIndexIsOutOfRange ? null : asArr[targetIndex + 1];
        }
    }
}