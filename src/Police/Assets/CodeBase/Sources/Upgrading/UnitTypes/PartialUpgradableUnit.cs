using Helpers;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Upgrading.UnitTypes
{
    [CreateAssetMenu(fileName = "PartialUpgradableUnit", menuName = "Upgradable units/Create Partial upgradable unit")]
    public class PartialUpgradableUnit : UpgradableUnit
    {
        [SerializeField] public LevelPartUnits[] LevelPartsUnitAppearances;
        [SerializeField, Min(1)] private int _levelPartsQuantity = 1;

        public int UpgradePartsReachedSerial
        {
            get
            {
                if (UpgradedLevel.Value == 1)
                    return 1;
                return (UpgradedLevel.Value + 2) / UpgradeUnitsConstants.LevelsToUpgradePart;
            }
        }

        public int MaxSuperLevel => LevelPartsUnitAppearances.Length;

        public ReactiveProperty<LevelPartUnits> CurrentAppearance;

        public void Initialize() 
            => CurrentAppearance = new ReactiveProperty<LevelPartUnits>(GetCurrentAppearance());

        public LevelPartUnits GetCurrentAppearance()
        {
            Debug.Log($"{nameof(PartialUpgradableUnit)}.{nameof(GetCurrentAppearance)} at {name} Called");
            var origin = Mathf.Clamp(UpgradePartsReachedSerial - 1, 0, LevelPartsUnitAppearances.Length - 1);
            for (int i = origin; i >= 0; i--)
            {
                LevelPartUnits appearanceByLevel = LevelPartsUnitAppearances[i];
                if (appearanceByLevel.Asset.RuntimeKeyIsValid() == false)
                {
                    continue;
                }
                Debug.Log($"{nameof(PartialUpgradableUnit)}.{nameof(GetCurrentAppearance)} Resolved at {name} {appearanceByLevel.Asset}");
                return appearanceByLevel;
            }
            
            throw new InvalidStateException(
                $"Array {nameof(LevelPartsUnitAppearances)} must contain at least first element, and it must be not null");
        }

        public bool IsFullyUpgraded()
            => UpgradePartsReachedSerial == _levelPartsQuantity;

        protected override bool IsCanUpgrade() => IsFullyUpgraded() == false;

        protected override void OnLevelIncremented()
        {
            
            Debug.Log($"{name} upgraded! Current level: {UpgradedLevel.Value}, IsFullyUpgraded: {IsFullyUpgraded()}, SuperLevels: {UpgradePartsReachedSerial}");
            CurrentAppearance.Value = GetCurrentAppearance();
        }

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
    }
}