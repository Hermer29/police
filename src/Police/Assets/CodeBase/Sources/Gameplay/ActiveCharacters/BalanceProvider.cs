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

        public BalanceProvider(ILevelService levelService)
        {
            _levelService = levelService;
            _statsTable = AllServices.Get<MeleeHostileStatsTable>();
        }

        public UnitBalanceDto GetFor(HostileUnit unit)
        {
            MeleeHostileStatsEntry relatedEntry = _statsTable.First(x => x.RelatedUnit == unit);
            float health = relatedEntry.InitialHealth + relatedEntry.HealthEvaluation.CalculateFor( 
                relatedEntry.HealthStrengthGrow, _levelService.Level);
            float damage = relatedEntry.InitialDamage + relatedEntry.DamageEvaluation.CalculateFor(
                relatedEntry.DamageStrengthGrow, _levelService.Level);
            return new UnitBalanceDto
            {
                Damage = damage,
                Health = health
            };
        }
    }
}