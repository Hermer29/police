using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Allies.Components;
using ActiveCharacters.Shared.Components;
using DG.Tweening;
using Gameplay.ActiveCharacters.Shared.Components;
using Helpers;
using Infrastructure.Services.UseService;
using PeopleDraw.AssetManagement;
using PeopleDraw.Components;
using Services.UseService;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using Upgrading.UnitTypes;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.PeopleDraw.Factory
{
    public class AlliedUnitsFactory : IAlliedUnitsFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ParentsForGeneratedObjects _parents;
        private readonly IUnitsFxFactory _unitsFxFactory;
        private readonly GameAssetLoader _assetLoader;
        private readonly UnitsAssetCache _cache;

        private List<WinAnimator> _winAnimators = new ();

        private Lazy<Dictionary<UnitType, ObjectPool<PoolUnit>>> _poolForTypes = new(
            valueFactory: () => new Dictionary<UnitType, ObjectPool<PoolUnit>>
            {
                { UnitType.Barrier, null },
                { UnitType.Melee, null },
                { UnitType.Ranged, null }
            });

        private readonly List<PoolUnit> _policemen = new ();
        public int ActivePolicemenAmount { get; private set; }
        
        public AlliedUnitsFactory(IInstantiator instantiator, ParentsForGeneratedObjects parents, IUnitsFxFactory unitsFxFactory,
            GameAssetLoader assetLoader, UnitsAssetCache cache)
        {
            _cache = cache;
            _assetLoader = assetLoader;
            _instantiator = instantiator;
            _parents = parents;
            _unitsFxFactory = unitsFxFactory;
            _cache = cache;
        }

        public event Action DestructionQueried;

        public void Initialize()
        {
            _cache.SelectedUnits.ObserveReplace().Subscribe(OnUnitTypeReplaced);
        }

        private void OnUnitTypeReplaced(DictionaryReplaceEvent<UnitType, PoolUnit> unit)
        {
            var relatedUnitPool = _poolForTypes.Value[unit.Key];
            if (relatedUnitPool == null)
                return;
            relatedUnitPool.Clear();
            _poolForTypes.Value[unit.Key] = CreateUnitsPool(unit.NewValue);
        }


        public async Task<GameObject> CreateSuperUnit() 
            => Object.Instantiate(await _assetLoader.LoadSuperUnit());

        public async Task<GameObject> CreateHelicopter() 
            => Object.Instantiate(await _assetLoader.LoadHelicopter());

        public PoolUnit InstantiateDrawnUnit(UnitType unitType, Vector3 position)
        {
            var prefab = _cache.SelectedUnits[unitType];
            if(PoolNotInitialized(unitType))
            {
                _poolForTypes.Value[unitType] = CreateUnitsPool(prefab);
            }

            PoolUnit unplacableUnit = _poolForTypes.Value[unitType].Get();
            unplacableUnit.transform.position = position;
            if (unplacableUnit.TryGetComponent(out PolicemenDyingListener policemenDyingListener))
            {
                ActivePolicemenAmount++;
                policemenDyingListener.Restore();
            }

            if (unplacableUnit.TryGetComponent(out CharactersNavigationLinks navLinks))
            {
                navLinks.Attackable.Restore();
            }
            return unplacableUnit;
        }

        private bool PoolNotInitialized(UnitType unitType)
        {
            return _poolForTypes.Value[unitType] == null;
        }

        private ObjectPool<PoolUnit> CreateUnitsPool(PoolUnit prefab)
        {
            ObjectPool<PoolUnit> pool = null;
            pool = new ObjectPool<PoolUnit>(
                createFunc: () => FactorizeUnit(prefab, pool),
                actionOnGet: obj =>
                {
                    obj.MarkUsed();
                    obj.gameObject.SetActive(true);
                },
                actionOnRelease: OnAlliedRelease);
            return pool;
        }

        private void OnAlliedRelease(PoolUnit obj)
        {
            if (obj.TryGetComponent<PolicemenDyingListener>(out var listener))
            {
                ActivePolicemenAmount--;
            }
            _unitsFxFactory.CreateDyingFx(obj.transform.position);
            obj.gameObject.SetActive(false);
        }

        public void AnimatorsPlayVictory() => 
            _winAnimators.ForEach(x => x.PlayVictory());

        public void ReturnAllToPool()
        {
            foreach (PoolUnit policeMan in _policemen.Where(policeMan => policeMan.Used))
            {
                policeMan.ReturnToPool();
            }
            DestructionQueried?.Invoke();
        }

        private PoolUnit FactorizeUnit(PoolUnit prefab, ObjectPool<PoolUnit> pool)
        {
            PoolUnit unit = _instantiator.InstantiatePrefabForComponent<PoolUnit>(
                prefab: prefab, 
                position: Vector3.zero, 
                rotation: Quaternion.identity, 
                parentTransform: _parents.Policemen);
            unit.Construct(pool);
            _policemen.Add(unit);
            if (unit.TryGetComponent(out WinAnimator winAnimator))
            {
                _winAnimators.Add(winAnimator);
            }
            return unit;
        }
    }
}