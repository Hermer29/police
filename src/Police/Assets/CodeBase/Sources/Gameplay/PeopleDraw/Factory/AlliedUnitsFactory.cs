using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.ActiveCharacters.Shared.Components;
using Helpers;
using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay.PeopleDraw.Factory
{
    public class AlliedUnitsFactory : IAlliedUnitsFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ParentsForGeneratedObjects _parents;
        private readonly IUnitsFxFactory _unitsFxFactory;

        private List<WinAnimator> _winAnimators = new ();
        private Lazy<Dictionary<int, ObjectPool<PoolUnit>>> _poolForTypes = new(
            valueFactory: () => new Dictionary<int, ObjectPool<PoolUnit>>());
        private List<PoolUnit> _policeMans = new ();

        public AlliedUnitsFactory(IInstantiator instantiator, ParentsForGeneratedObjects parents, IUnitsFxFactory unitsFxFactory)
        {
            _instantiator = instantiator;
            _parents = parents;
            _unitsFxFactory = unitsFxFactory;
        }
        
        public PoolUnit InstantiateDrawnUnit(PoolUnit prefab, Vector3 position, int selectedSerial)
        {
            if (_poolForTypes.Value.ContainsKey(selectedSerial) == false)
            {
                _poolForTypes.Value.Add(selectedSerial, CreateUnitsPool(prefab));
            }

            PoolUnit unplacableUnit = _poolForTypes.Value[selectedSerial].Get();
            unplacableUnit.transform.position = position;
            return unplacableUnit;
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
            _unitsFxFactory.CreateDyingFx(obj.transform.position);
            obj.gameObject.SetActive(false);
        }

        public void AnimatorsPlayVictory() => 
            _winAnimators.ForEach(x => x.PlayVictory());

        public void ReturnAllToPool()
        {
            foreach (PoolUnit policeMan in _policeMans.Where(policeMan => policeMan.Used))
            {
                policeMan.ReturnToPool();
            }
        }

        private PoolUnit FactorizeUnit(PoolUnit prefab, ObjectPool<PoolUnit> pool)
        {
            PoolUnit unit = _instantiator.InstantiatePrefabForComponent<PoolUnit>(
                prefab: prefab, 
                position: Vector3.zero, 
                rotation: Quaternion.identity, 
                parentTransform: _parents.Policemen);
            unit.Construct(pool);
            _policeMans.Add(unit);
            if (unit.TryGetComponent(out WinAnimator winAnimator))
            {
                _winAnimators.Add(winAnimator);
            }
            return unit;
        }
    }
}