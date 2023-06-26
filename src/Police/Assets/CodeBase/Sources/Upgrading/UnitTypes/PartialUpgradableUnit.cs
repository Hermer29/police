using System;
using System.Linq;
using DefaultNamespace.Gameplay;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats.StatsTables;
using Helpers;
using Infrastructure;
using ModestTree;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Upgrading.UnitTypes
{
    [CreateAssetMenu(fileName = "PartialUpgradableUnit", menuName = "Upgradable units/Create Partial upgradable unit")]
    public class PartialUpgradableUnit : UpgradableUnit
    {
        [SerializeField] public LevelPartUnits[] LevelPartsUnitAppearances;

        public int UpgradePartsReachedSerial
        {
            get
            {
                if (UpgradedLevel.Value == 1)
                    return 1;
                return Mathf.Clamp((UpgradedLevel.Value + 2) / UpgradeUnitsConstants.LevelsToUpgradePart,
                    1, MaxSuperLevel);
            }
        }

        public int MaxSuperLevel => LevelPartsUnitAppearances.Length;

        private ReactiveProperty<LevelPartUnits> _currentAppearance;
        private ReactiveProperty<Sprite> _currentIcon;
        private ReactiveProperty<AssetReference> _currentReference;
        public IReadOnlyReactiveProperty<LevelPartUnits> CurrentAppearance => _currentAppearance;
        public IReadOnlyReactiveProperty<Sprite> CurrentIcon => _currentIcon;
        public IReadOnlyReactiveProperty<AssetReference> CurrentReference => _currentReference;

        public void Initialize()
        {
            _currentAppearance = new ReactiveProperty<LevelPartUnits>();
            UpgradedLevel.Subscribe(_ => _currentAppearance.Value = GetCurrentAppearance());
            _currentIcon = new ReactiveProperty<Sprite>(_currentAppearance.Value.Illustration);
            _currentReference = new ReactiveProperty<AssetReference>(_currentAppearance.Value.Asset);
            _currentAppearance.Skip(1).Subscribe(newValue =>
            {
                _currentIcon.Value = newValue.Illustration;
                _currentReference.Value = newValue.Asset;
            });
        }

        private LevelPartUnits GetCurrentAppearance()
        {
            int currentAppearanceIndex = UpgradePartsReachedSerial - 1;
            LevelPartUnits currentAppearance = LevelPartsUnitAppearances[currentAppearanceIndex];
            
            if(currentAppearance.IsEmpty == false)
                return currentAppearance;
            
            return LevelPartsUnitAppearances.Take(UpgradePartsReachedSerial)
                .Reverse().First(x => x.Asset.RuntimeKeyIsValid());
        }

        public bool IsFullyUpgraded()
            => UpgradePartsReachedSerial >= MaxSuperLevel;

        protected override bool IsCanUpgrade() => IsFullyUpgraded() == false;

        public bool TryGetNextAppearance(out LevelPartUnits next)
        {
            int current = LevelPartsUnitAppearances.IndexOf(GetCurrentAppearance());
            int nextIndex = current + 1;
            if (LevelPartsUnitAppearances.Length <= nextIndex)
            {
                next = null;
                return false;
            }

            next = LevelPartsUnitAppearances[nextIndex];
            return true;
        }

        public LevelPartUnits GetLastAppearance() 
            => LevelPartsUnitAppearances[^1];

        public AlliedUnitsBalanceDto CalculateBalance()
        {
            switch (Type)
            {
                case UnitType.Barrier:
                    return CalculateBarrier();
                case UnitType.Ranged:
                    return CalculateRanged();
                case UnitType.Melee:
                    return CalculateMelee();
            }

            throw new ArgumentOutOfRangeException(nameof(Type));
        }

        private AlliedUnitsBalanceDto CalculateBarrier()
        {
            var barriers = AllServices.Get<BarriersStatsTable>();
            var related = barriers.First(x => x.RelatedUnit == this);
            var health = related.HealthEvaluation.EvaluateFor(related.HealthGrow, UpgradedLevel.Value,
                related.InitialHealth);
            return new AlliedUnitsBalanceDto { Health = health + health * related.ExtraHealthModifier};
        }
        
        private AlliedUnitsBalanceDto CalculateRanged()
        {
            var barriers = AllServices.Get<RangedStatsTable>();
            var related = barriers.First(x => x.RelatedUnit == this);
            var damage = related.DamageEvaluation.EvaluateFor(related.DamageGrowStrength, UpgradedLevel.Value,
                related.InitialDamage);
            return new AlliedUnitsBalanceDto { Damage = damage, Range = related.AttackRange + related.AttackRange * related.ExtraRangeModifier };
        }
        
        private AlliedUnitsBalanceDto CalculateMelee()
        {
            var barriers = AllServices.Get<MeleeStatsTable>();
            var related = barriers.First(x => x.RelatedUnit == this);
            var damage = related.DamageEvaluation.EvaluateFor(related.DamageStrengthGrow, UpgradedLevel.Value,
                related.InitialDamage);
            var health = related.HealthEvaluation.EvaluateFor(related.HealthStrengthGrow, UpgradedLevel.Value,
                related.InitialHealth);
            return new AlliedUnitsBalanceDto { Damage = damage + damage * related.ExtraDamageModifier, 
                Health = health + health * related.ExtraHealthModifier};
        }
    }
}