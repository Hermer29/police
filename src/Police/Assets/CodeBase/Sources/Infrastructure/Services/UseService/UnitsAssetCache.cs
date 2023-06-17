using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Infrastructure.Services.UseService;
using PeopleDraw.Components;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public class UnitsAssetCache : IDisposable, IManuallyInitializable
    {
        private class AssetReferenceHolder
        {
            public AssetReference Asset = new();
            public IDisposable ChangeAppearanceSubscription;
        }
        
        private readonly UnitsUsingService _unitsUsingService;

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

        public UnitsAssetCache(UnitsUsingService unitsUsingService)
        {
            _unitsUsingService = unitsUsingService;
        }

        void IManuallyInitializable.Initialize()
        {
            PreloadAndHoldLinks();
        }

        private void PreloadAndHoldLinks()
        {
            _unitsUsingService.UsedUnits.ObserveReplace().Subscribe(OnUnitReplaced);
            foreach ((UnitType key, PartialUpgradableUnit value) in _unitsUsingService.UsedUnits)
                UseUnit(value.CurrentReference.Value, key);
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(PreloadAndHoldLinks)} finished");
        }

        private void OnUnitReplaced(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            LoadNewUnit(update);
        }

        private void LoadNewUnit(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            TryReleasePreviousAsset(update.Key);
            DisposeAppearanceChangeSubscription(update);
            MakeAppearanceChangeSubscription(update);
            UseUnit(update.NewValue.CurrentReference.Value, update.Key);
        }

        private void OnPartUpgraded(AssetReference levelPartUnits, UnitType updateKey)
        {
            TryReleasePreviousAsset(updateKey);
            UseUnit(levelPartUnits, updateKey);
        }

        private async void UseUnit(AssetReference assetReference, UnitType unitType)
        {
            if (AlreadyUsingReference(assetReference, unitType))
                return;
            
            GameObject unit = await Addressables.LoadAssetAsync<GameObject>(assetReference).Task;
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(UseUnit)} loaded {unitType}, asset name: {unit.name}");
            _assetLinks[unitType].Asset = assetReference;
            _selectedUnits[unitType] = unit.GetComponent<PoolUnit>();
        }

        private void MakeAppearanceChangeSubscription(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            _assetLinks[update.Key].ChangeAppearanceSubscription = 
                update.NewValue.CurrentReference.Subscribe(
                    value => OnPartUpgraded(value, update.Key));
        }

        private void DisposeAppearanceChangeSubscription(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update) 
            => _assetLinks[update.Key].ChangeAppearanceSubscription?.Dispose();

        private bool AlreadyUsingReference(AssetReference assetReference, UnitType unitType) 
            => ReferenceAlreadyInUse(assetReference, unitType);

        private bool ReferenceAlreadyInUse(AssetReference assetReference, UnitType unitType) 
            => _assetLinks[unitType].Asset == assetReference;

        private void TryReleasePreviousAsset(UnitType unitType)
        {
            if (AssetLinkExists(unitType)) 
                Addressables.Release(_assetLinks[unitType].Asset);
        }

        private bool AssetLinkExists(UnitType unitType)
        {
            return _assetLinks[unitType].Asset != null && _assetLinks[unitType].Asset.RuntimeKeyIsValid();
        }

        public void Dispose() 
            => DisposeAppearanceChangeSubscriptions();

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