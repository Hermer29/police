using System;
using System.Collections.Generic;
using System.Linq;
using Services.PrefsService;
using Services.UseService;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Upgrading;
using Upgrading.UnitTypes;

namespace Infrastructure.Services.UseService
{
    public class UnitsUsingService
    {
        private readonly IPrefsService _prefsService;
        private readonly UnitsRepository _repository;
        public readonly ReactiveDictionary<UnitType, PartialUpgradableUnit> UsedUnits = new();
        public readonly ReactiveCollection<UnitType> MaxUpgraded = new();

        public UnitsUsingService(IPrefsService prefsService, UnitsRepository repository)
        {
            _prefsService = prefsService;
            _repository = repository;
        }

        public void PreloadUsedUnits()
        {
            var equippedUnits = GetEquipped(_repository);
            AssertThatEquippedUnitsIsSingleInTheirTypes(equippedUnits);
            InitializeSelectedDictionary(equippedUnits);
            WatchForNextSuperLevelCommencing();
            Debug.Log($"{nameof(UnitsUsingService)}.{nameof(PreloadUsedUnits)} finished");
        }

        private void WatchForNextSuperLevelCommencing()
        {
            foreach (PartialUpgradableUnit partialUpgradableUnit in _repository.GetAll())
            {
                partialUpgradableUnit.UpgradedLevel
                    .Subscribe(
                        _ => 
                            CheckIsUnitMaxUpgradedInHierarchy(partialUpgradableUnit));
            }
        }

        private void CheckIsUnitMaxUpgradedInHierarchy(PartialUpgradableUnit partialUpgradableUnit)
        {
            if (!partialUpgradableUnit.IsFullyUpgraded()) return;
            Debug.Log($"{nameof(UnitsUsingService)}.{nameof(CheckIsUnitMaxUpgradedInHierarchy)} processing {partialUpgradableUnit.name}");
            UnitType type = partialUpgradableUnit.Type;
            PartialUpgradableUnit next = _repository.GetOrderedWithType(type)
                .GetNext(partialUpgradableUnit);
            if (next == null)
            {
                MaxUpgraded.Add(partialUpgradableUnit.Type);
                return;
            }
            UsedUnits[partialUpgradableUnit.Type] = next;
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
            var grouped = upgradableUnits.GroupBy(x => x.Type);
            foreach (var grouping in grouped)
            {
                if (grouping.Any(IsEquipped)) continue;
                Use(grouping.First(x => x.OwnedByDefault));
            }
        }

        public void Use(PartialUpgradableUnit shopProduct)
        {
            UsedUnits[shopProduct.Type] = shopProduct;
            SaveEquip(shopProduct);
        }

        public bool IsEquipped<TEnum>(ITypedUnit<TEnum> equippable) where TEnum: Enum => 
            _prefsService.GetString(equippable.Type.ToString()) == equippable.Guid;

        private void InitializeSelectedDictionary(IEnumerable<PartialUpgradableUnit> equippedUnits)
        {
            foreach (PartialUpgradableUnit upgradableUnit in equippedUnits)
            {
                if (UsedUnits.ContainsKey(upgradableUnit.Type)) continue;
                UsedUnits.Add(upgradableUnit.Type, upgradableUnit);
                CheckIsUnitMaxUpgradedInHierarchy(upgradableUnit);
            }
        }

        private static void AssertThatEquippedUnitsIsSingleInTheirTypes(IEnumerable<PartialUpgradableUnit> equippedUnits) =>
            Assert.IsTrue(equippedUnits.GroupBy(x => x.Type)
                .Select(x => x.Count() == 1)
                .Aggregate((result, curr) => result && curr));

        private void SaveEquip<TEnum>(ITypedUnit<TEnum> ownable) where TEnum: Enum => 
            _prefsService.SetString(ownable.Type.ToString(), ownable.Guid);
    }
}