using System.Collections.Generic;
using System.Linq;

namespace Upgrading.UnitTypes
{
    public static class UnitTypesExtensions
    {
        public static IEnumerable<PartialUpgradableUnit> OrderByLogic(this IEnumerable<PartialUpgradableUnit> units)
        {
            var upgradableUnits = units as PartialUpgradableUnit[] ?? units.ToArray();
            var partialUpgradableUnits = units as PartialUpgradableUnit[] ?? upgradableUnits.ToArray();
            if (partialUpgradableUnits.Count() == 1)
                return new Queue<PartialUpgradableUnit>(partialUpgradableUnits);

            var queue = new List<PartialUpgradableUnit>();
            PartialUpgradableUnit startingUnit = GetUnitStartingUpgradeChain(partialUpgradableUnits);
            queue.Add(startingUnit);
            var otherUnpurchasableUnits = NonStartingEvolvingUnits(partialUpgradableUnits).ToList();
            
            PartialUpgradableUnit tail = startingUnit;

            while (otherUnpurchasableUnits.Count != 0)
            {
                EvolvedUpgradableUnit next = otherUnpurchasableUnits.First(x => x.PreviousUnit == tail);
                otherUnpurchasableUnits.Remove(next);
                queue.Add(next);
                tail = next;
            }
            queue.AddRange(upgradableUnits.Where(x => x.Purchasable));
            return queue;
        }

        private static IEnumerable<EvolvedUpgradableUnit> NonStartingEvolvingUnits(PartialUpgradableUnit[] units)
        {
            return units.Where(x => x.Purchasable == false && x != GetUnitStartingUpgradeChain(units))
                .Where(x => x is EvolvedUpgradableUnit)
                .Cast<EvolvedUpgradableUnit>();
        }

        private static PartialUpgradableUnit GetUnitStartingUpgradeChain(PartialUpgradableUnit[] partialUpgradableUnits)
        {
            return partialUpgradableUnits
                .First(x => x is not EvolvedUpgradableUnit && x.Purchasable == false);
        }
    }
}