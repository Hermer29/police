using System;
using System.Linq;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Gameplay.Levels.Services.LevelsTracking;
using Infrastructure;
using MathUtility;

namespace DefaultNamespace.Gameplay.ActiveCharacters
{
    public class BalanceProvider
    {
        private readonly ILevelService _levelService;
        private readonly MeleeHostileStatsTable _statsTable;
        private int _difficultyModification = 1;

        public BalanceProvider(ILevelService levelService)
        {
            _levelService = levelService;
            _statsTable = AllServices.Get<MeleeHostileStatsTable>();
        }

        public UnitBalanceDto GetFor(HostileUnit unit)
        {
            var level = _levelService.Level / 5;
            MeleeHostileStatsEntry relatedEntry = _statsTable.First(x => x.RelatedUnit == unit);
            float health = relatedEntry.InitialHealth + relatedEntry.HealthEvaluation.CalculateFor( 
                relatedEntry.HealthStrengthGrow, level * _difficultyModification);
            float damage = relatedEntry.InitialDamage + relatedEntry.DamageEvaluation.CalculateFor(
                relatedEntry.DamageStrengthGrow, level * _difficultyModification);
            return new UnitBalanceDto
            {
                Damage = damage,
                Health = health
            };
        }

        public void ResetDifficultyModification()
        {
            _difficultyModification = 1;
        }

        public void ModifyDifficulty(int i)
        {
            _difficultyModification = i;
        }
    }
}