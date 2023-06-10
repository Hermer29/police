using Upgrading.UnitTypes;

namespace UnitSelection
{
    public static class UnitTypeEconomy
    {
        public static int Cost(this UnitType type) => type switch
        {
            UnitType.Barrier => 2, 
            UnitType.Melee => 8,
            UnitType.Ranged => 32,
            UnitType.None => 0
        };
    }
}