using System;
using System.Collections.Generic;
using System.Linq;
using Services.PrefsService;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Upgrading;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public class UnitsUsingService
    {
        private readonly IPrefsService _prefsService;

        public readonly ReactiveDictionary<UnitType, PartialUpgradableUnit> UsedUnits;
        private readonly UnitsRepository _repository;

        public UnitsUsingService(IPrefsService prefsService, UnitsRepository repository)
        {
            _prefsService = prefsService;
            _repository = repository;
            UsedUnits = new ReactiveDictionary<UnitType, PartialUpgradableUnit>();
        }

        public void PreloadUsedUnits()
        {
            var equippedUnits = GetEquipped(_repository);
            AssertThatEquippedUnitsIsSingleInTheirTypes(equippedUnits);
            InitializeSelectedDictionary(equippedUnits);
            Debug.Log($"{nameof(UnitsUsingService)}.{nameof(PreloadUsedUnits)} finished");
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