using UnityEngine;
using Upgrading.UnitTypes;

namespace CodeBase.Tests
{
    public static class Create
    {
        public static PartialUpgradableUnit PartialUpgradableUnit() 
            => ScriptableObject.CreateInstance<PartialUpgradableUnit>();
    }
}