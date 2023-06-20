using System;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [Serializable]
    public class MeleeHostileStatsEntry
    {
        public HostileUnit RelatedUnit;
        public float InitialDamage;
        public float DamageStrengthGrow;
        public EvaluationType DamageEvaluation;
        public float InitialHealth;
        public float HealthStrengthGrow;
        public EvaluationType HealthEvaluation;
    }
}