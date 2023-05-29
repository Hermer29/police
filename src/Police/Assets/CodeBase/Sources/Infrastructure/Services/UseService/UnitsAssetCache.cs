using System;
using System.Collections.Generic;
using System.Linq;
using PeopleDraw.Components;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Upgrading.UnitTypes;

namespace Services.UseService
{
    public class UnitsAssetCache : IDisposable
    {
        private class AssetReferenceHolder
        {
            public AssetReference Asset;
            public IDisposable ChangeAppearanceSubscription;
        }
        
        private readonly UnitsUsingService _unitsUsingService;

        private Dictionary<UnitType, AssetReferenceHolder> _assetLinks = new();
        
        public UnitsAssetCache(UnitsUsingService unitsUsingService)
        {
            _unitsUsingService = unitsUsingService;
            _unitsUsingService.UsedUnits.ObserveReplace().Subscribe(OnUnitReplaced);
            SelectedUnits = new ReactiveDictionary<UnitType, PoolUnit>();
        }

        public ReactiveDictionary<UnitType, PoolUnit> SelectedUnits;

        private void OnUnitReplaced(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            DisposeAppearanceChangeSubscription(update);
            MakeAppearanceChangeSubscription(update);
            UseUnit(update.NewValue.CurrentAppearance.Value.Asset, update.Key);
        }

        private void MakeAppearanceChangeSubscription(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            _assetLinks[update.Key].ChangeAppearanceSubscription = 
                update.NewValue.CurrentAppearance.Subscribe((value) => OnPartUpgraded(value, update.Key));
        }

        private void DisposeAppearanceChangeSubscription(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> update)
        {
            _assetLinks[update.Key].ChangeAppearanceSubscription?.Dispose();
        }

        private async void UseUnit(AssetReference assetReference, UnitType unitType)
        {
            if (KeyNotPresented(unitType) == false && ReferenceAlreadyInUse(assetReference, unitType)) return;
            TryReleasePreviousAsset(unitType);

            if (_assetLinks.ContainsKey(unitType) == false)
            {
                _assetLinks.Add(unitType, new AssetReferenceHolder());
            }
            
            GameObject unit = await Addressables.LoadAssetAsync<GameObject>(assetReference).Task;

            _assetLinks[unitType].Asset = assetReference;
            SelectedUnits[unitType] = unit.GetComponent<PoolUnit>();
        }

        private bool KeyNotPresented(UnitType unitType)
        {
            return _assetLinks.ContainsKey(unitType) == false;
        }

        private bool ReferenceAlreadyInUse(AssetReference assetReference, UnitType unitType)
        {
            return _assetLinks[unitType].Asset == assetReference;
        }

        private void TryReleasePreviousAsset(UnitType unitType)
        {
            if (_assetLinks.ContainsKey(unitType) == false)
                return;
            ;
            if (_assetLinks[unitType].Asset != null) Addressables.Release(_assetLinks[unitType]);
        }

        public void PreloadAndHoldLinks()
        {
            foreach ((UnitType key, PartialUpgradableUnit value) in _unitsUsingService.UsedUnits)
                UseUnit(value.CurrentAppearance.Value.Asset, key);
            Debug.Log($"{nameof(UnitsAssetCache)}.{nameof(PreloadAndHoldLinks)} finished");
        }

        private void OnPartUpgraded(LevelPartUnits levelPartUnits, UnitType updateKey)
        {
            UseUnit(levelPartUnits.Asset, updateKey);
        }

        public void Dispose()
        {
            DisposeAppearanceChangeSubscriptions();
        }

        private void DisposeAppearanceChangeSubscriptions()
        {
            foreach (IDisposable disposable in _assetLinks.Values.Select(x => x.ChangeAppearanceSubscription))
            {
                disposable.Dispose();
            }
        }
    }
}