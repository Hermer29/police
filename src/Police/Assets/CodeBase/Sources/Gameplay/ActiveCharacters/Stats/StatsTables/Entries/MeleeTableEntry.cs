using System;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [Serializable]
    public class MeleeTableEntry
    {
        public UpgradableUnit RelatedUnit;
        public float InitialAttackRange;
        public float InitialDamage;
        public float DamageStrengthGrow;
        public EvaluationType DamageEvaluation;
        public float InitialHealth;
        public float HealthStrengthGrow;
        public EvaluationType HealthEvaluation;
        public float ExtraHealthModifier;
        public float ExtraDamageModifier;
    }
}