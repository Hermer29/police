using System;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [Serializable]
    public class BarriersStatsEntry
    {
        public UpgradableUnit RelatedUnit;
        public float InitialHealth;
        public float HealthGrow;
        public EvaluationType HealthEvaluation;
        public float ExtraHealthModifier;
    }
}