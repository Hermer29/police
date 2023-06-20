using System;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [Serializable]
    public class RangedStatsEntry
    {
        public UpgradableUnit RelatedUnit;
        public float AttackRange;
        public float InitialDamage;
        public float DamageGrowStrength;
        public EvaluationType DamageEvaluation;
        public float ExtraHealthModifier;
        public float ExtraRangeModifier;
    }
}