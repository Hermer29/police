using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Infrastructure.Services.UseService;
using PeopleDraw.Components;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public class UnitsAssetCache : IDisposable, IManuallyInitializable
    {
        private class AssetReferenceHolder
        {
            public AsyncOperationHandle<GameObject> Asset;
            public IDisposable ChangeAppearanceSubscription;
            public bool Loaded;
        }
        
        private readonly UsedUnitsService _usedUnitsService;

        private Dictionary<UnitType, AssetReferenceHolder> _assetLinks = new()
        {
            { UnitType.Barrier, new AssetReferenceHolder() },
            { UnitType.Melee, new AssetReferenceHolder() },
            { UnitType.Ranged, new AssetReferenceHolder() }
        };

        private ReactiveDictionary<UnitType, PoolUnit> _selectedUnits = new()
        {
            { UnitType.Barrier, null },
            { UnitType.Melee, null },
            { UnitType.Ranged, null }
        };

        public IReadOnlyReactiveDictionary<UnitType, PoolUnit> SelectedUnits => _selectedUnits;

        public UnitsAssetCache(UsedUnitsService usedUnitsService)
        {
            _usedUnitsService = usedUnitsService;
        }

        void IManuallyInitializable.Initialize() => PreloadAndHoldLinks();

        private void PreloadAndHoldLinks()
        {
            _usedUnitsService.UsedUnits.ObserveReplace().Subscribe(OnUnitReplaced);
            foreach ((UnitType key, PartialUpgradableUnit value) in _usedUnitsService.UsedUnits)
                LoadNewUnit(key, value);
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(PreloadAndHoldLinks)} finished");
        }

        private void OnUnitReplaced(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update) => LoadNewUnit(update.Key, update.NewValue);

        private void OnPartUpgraded(PartialUpgradableUnit levelPartUnits, UnitType updateKey) => LoadNewUnit(updateKey, levelPartUnits);

        private void LoadNewUnit(UnitType unitType, PartialUpgradableUnit unit)
        {
            ReleasePrevious(unitType);
            MakeAppearanceChangeSubscription(unitType, unit);
            UseUnit(unit.CurrentReference.Value, unitType);
        }

        private void ReleasePrevious(UnitType unitType)
        {
            if (_assetLinks[unitType].Loaded == false) 
                return;
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(ReleasePrevious)} {unitType} disposing. _assetLinks[unitType] = {_assetLinks[unitType].Asset}");
            TryReleasePreviousAsset(unitType);
            DisposeAppearanceChangeSubscription(unitType);
        }

        private async void UseUnit(AssetReference assetReference, UnitType unitType)
        { 
            var handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
            GameObject unit = await handle.Task;
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(UseUnit)} loaded {unitType}, asset name: {unit.name}, id: {assetReference}");
            _assetLinks[unitType].Asset = handle;
            _assetLinks[unitType].Loaded = true;
            _selectedUnits[unitType] = unit.GetComponent<PoolUnit>();
        }

        private void MakeAppearanceChangeSubscription(UnitType key, PartialUpgradableUnit unit)
        {
            Assert.IsNotNull(_assetLinks[key]);
            Assert.IsNull(_assetLinks[key].ChangeAppearanceSubscription);
            Assert.IsNotNull(unit);
            Assert.IsNotNull(unit.CurrentReference);
            _assetLinks[key].ChangeAppearanceSubscription = unit.CurrentReference.Skip(1).Subscribe(
                    _ => OnPartUpgraded(unit, key));
        }

        private void DisposeAppearanceChangeSubscription(UnitType type)
        {
            _assetLinks[type].ChangeAppearanceSubscription.Dispose();
            _assetLinks[type].ChangeAppearanceSubscription = null;
        }
        
        private void TryReleasePreviousAsset(UnitType unitType)
        {
            Assert.IsTrue(_assetLinks[unitType].Loaded);
            Addressables.Release(_assetLinks[unitType].Asset);
            _assetLinks[unitType].Loaded = false;
        }

        public void Dispose()
        {
            DisposeReferences();
            DisposeAppearanceChangeSubscriptions();
        }

        private void DisposeReferences()
        {
            foreach (var assetReferenceHolder in _assetLinks)
            {
                Addressables.Release(assetReferenceHolder);
            }
        }

        private void DisposeAppearanceChangeSubscriptions()
        {
            foreach (IDisposable disposable in _assetLinks.Values.Select(x => x.ChangeAppearanceSubscription))
                disposable.Dispose();
        }
    }

    public class UnitsUpdateHandler
    {
        
    }
}