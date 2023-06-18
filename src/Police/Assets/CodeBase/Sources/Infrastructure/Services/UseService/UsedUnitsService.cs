using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Services.PrefsService;
using Services.UseService;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Upgrading;
using Upgrading.UnitTypes;

namespace Infrastructure.Services.UseService
{
    public class UsedUnitsService : IManuallyInitializable
    {
        private readonly IPrefsService _prefsService;
        private readonly UnitsRepository _repository;

        private readonly ReactiveDictionary<UnitType, PartialUpgradableUnit> _usedUnits = new()
        {
            { UnitType.Barrier, null},
            { UnitType.Melee, null},
            { UnitType.Ranged, null}
        };

        public IReadOnlyReactiveDictionary<UnitType, PartialUpgradableUnit> UsedUnits => _usedUnits;
        public readonly ReactiveCollection<UnitType> MaxUpgraded = new();

        public UsedUnitsService(IPrefsService prefsService, UnitsRepository repository)
        {
            _prefsService = prefsService;
            _repository = repository;
        }

        void IManuallyInitializable.Initialize() => PreloadUsedUnits();

        private void PreloadUsedUnits()
        {
            var equippedUnits = GetEquipped(_repository);
            AssertThatEquippedUnitsIsSingleInTheirTypes(equippedUnits);
            InitializeSelectedDictionary(equippedUnits);
            foreach (PartialUpgradableUnit partialUpgradableUnit in equippedUnits)
            {
                _usedUnits[partialUpgradableUnit.Type] = partialUpgradableUnit;
                Debug.Log($"{partialUpgradableUnit.Type}:{partialUpgradableUnit.name} -- upgrade level: {partialUpgradableUnit.UpgradedLevel.Value}");
            }
            OnSuperLevelUpgraded();
            Debug.Log($"{nameof(UsedUnitsService)}.{nameof(PreloadUsedUnits)} finished");
        }

        private void OnSuperLevelUpgraded()
        {
            foreach (PartialUpgradableUnit partialUpgradableUnit in _repository.GetAll())
            {
                partialUpgradableUnit.UpgradedLevel
                    .Subscribe(_ => CheckIsUnitMaxUpgradedInHierarchy(partialUpgradableUnit));
            }
        }

        private void CheckIsUnitMaxUpgradedInHierarchy(PartialUpgradableUnit partialUpgradableUnit)
        {
            if (!partialUpgradableUnit.IsFullyUpgraded()) return;
            Debug.Log($"{nameof(UsedUnitsService)}.{nameof(CheckIsUnitMaxUpgradedInHierarchy)} processing {partialUpgradableUnit.name}");
            UnitType type = partialUpgradableUnit.Type;
            PartialUpgradableUnit next = _repository.GetOrderedWithType(type)
                .GetNext(partialUpgradableUnit);
            if (next == null)
            {
                MaxUpgraded.Add(partialUpgradableUnit.Type);
                return;
            }
            _usedUnits[partialUpgradableUnit.Type] = next;
        }

        private IEnumerable<PartialUpgradableUnit> GetEquipped(UnitsRepository repository)
        {
            var upgradableUnits = repository.GetAll();
            WriteDefaults(upgradableUnits);
            var equippedUnits = upgradableUnits.Where(IsEquipped);
            return equippedUnits;
        }

        private void WriteDefaults(IEnumerable<PartialUpgradableUnit> upgradableUnits)
        {
            if (_prefsService.HasKey("NotFirstStart"))
                return;
            var grouped = upgradableUnits.GroupBy(x => x.Type);
            foreach (var grouping in grouped)
            {
                if (grouping.Any(IsEquipped)) continue;
                Use(grouping.First(x => x.OwnedByDefault));
            }
        }

        public void Use(PartialUpgradableUnit shopProduct)
        {
            _usedUnits[shopProduct.Type] = shopProduct;
            SaveEquip(shopProduct);
        }

        public bool IsEquipped<TEnum>(ITypedUnit<TEnum> equippable) where TEnum: Enum => 
            _prefsService.GetString(ConstructTypeKey(equippable)) == equippable.Guid;

        private void InitializeSelectedDictionary(IEnumerable<PartialUpgradableUnit> equippedUnits)
        {
            foreach (PartialUpgradableUnit upgradableUnit in equippedUnits)
            {
                CheckIsUnitMaxUpgradedInHierarchy(upgradableUnit);
            }
        }

        private static void AssertThatEquippedUnitsIsSingleInTheirTypes(IEnumerable<PartialUpgradableUnit> equippedUnits) =>
            Assert.IsTrue(equippedUnits.GroupBy(x => x.Type)
                .Select(x => x.Count() == 1)
                .Aggregate((result, curr) => result && curr));

        private void SaveEquip<TEnum>(ITypedUnit<TEnum> ownable) where TEnum: Enum => 
            _prefsService.SetString(ConstructTypeKey(ownable), ownable.Guid);

        private string ConstructTypeKey<TEnum>(ITypedUnit<TEnum> ownable) where TEnum : Enum
        {
            return $"{ownable.Type}_Using";
        }
    }
}