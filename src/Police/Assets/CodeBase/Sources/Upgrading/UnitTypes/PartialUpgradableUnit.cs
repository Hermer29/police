using Helpers;
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

        public ReactiveProperty<LevelPartUnits> CurrentAppearance;

        public void Initialize() 
            => CurrentAppearance = new ReactiveProperty<LevelPartUnits>(GetCurrentAppearance());

        public LevelPartUnits GetCurrentAppearance()
        {
            for (int i = UpgradePartsReachedSerial - 1; i >= 0; i--)
            {
                LevelPartUnits appearanceByLevel = LevelPartsUnitAppearances[i];
                if (appearanceByLevel == null)
                {
                    continue;
                }
                return appearanceByLevel;
            }

            throw new InvalidStateException(
                $"Array {nameof(LevelPartsUnitAppearances)} must contain at least first element, and it must be not null");
        }

        public bool IsFullyUpgraded()
            => UpgradePartsReachedSerial == _levelPartsQuantity;

        protected override void OnLevelIncremented()
            => CurrentAppearance.Value = GetCurrentAppearance();
    }
}