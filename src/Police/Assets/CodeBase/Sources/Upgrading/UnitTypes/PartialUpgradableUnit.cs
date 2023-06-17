using System.Linq;
using Helpers;
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
            _currentAppearance = new ReactiveProperty<LevelPartUnits>(GetCurrentAppearance());
            _currentIcon = new ReactiveProperty<Sprite>(_currentAppearance.Value.Illustration);
            _currentReference = new ReactiveProperty<AssetReference>(_currentAppearance.Value.Asset);
            _currentAppearance.Subscribe(newValue =>
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

        protected override void OnLevelIncremented()
        {
            Debug.Log($"{name} upgraded! Current level: {UpgradedLevel.Value}, IsFullyUpgraded: {IsFullyUpgraded()}, SuperLevels: {UpgradePartsReachedSerial}");
            _currentAppearance.Value = GetCurrentAppearance();
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