using UnityEngine;

namespace Upgrading.UnitTypes
{
    [CreateAssetMenu(fileName = "EvolvedUpgradableUnit", menuName = "Upgradable units/Create Evolved upgradable unit")]
    public class EvolvedUpgradableUnit : PartialUpgradableUnit
    {
        [field: SerializeField] public bool IsHiddenWhenNotUnlocked { get; private set; }

        public PartialUpgradableUnit PreviousUnit;

        public bool IsUnlocked() => 
            PreviousUnit.IsFullyUpgraded();
    }
}