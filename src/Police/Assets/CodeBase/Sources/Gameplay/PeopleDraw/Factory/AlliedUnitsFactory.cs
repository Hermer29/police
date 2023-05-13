using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.ActiveCharacters.Shared.Components;
using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.PeopleDraw.Factory
{
    public class AlliedUnitsFactory : IAlliedUnitsFactory
    {
        private Lazy<Dictionary<int, ObjectPool<PoolUnit>>> _poolForTypes = new(
            valueFactory: () => new Dictionary<int, ObjectPool<PoolUnit>>());

        private List<WinAnimator> _winAnimators = new ();
        private List<PoolUnit> _policeMans = new ();
        private readonly IInstantiator _instantiator;

        public AlliedUnitsFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
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

        public ObjectPool<PoolUnit> CreateUnitsPool(PoolUnit prefab)
        {
            ObjectPool<PoolUnit> pool = null;
            pool = new ObjectPool<PoolUnit>(
                createFunc: () => FactorizeUnit(prefab, pool),
                actionOnGet: obj =>
                {
                    obj.MarkUsed();
                    obj.gameObject.SetActive(true);
                },
                actionOnRelease: obj => obj.gameObject.SetActive(false));
            return pool;
        }

        public void AnimatorsPlayVictory()
        {
            _winAnimators.ForEach(x => x.PlayVictory());
        }

        public void ReturnAllToPool()
        {
            foreach (PoolUnit policeMan in _policeMans.Where(policeMan => policeMan.Used))
            {
                policeMan.ReturnToPool();
            }
        }
        
        private PoolUnit FactorizeUnit(PoolUnit prefab, ObjectPool<PoolUnit> pool)
        {
            PoolUnit unit = _instantiator.InstantiatePrefabForComponent<PoolUnit>(prefab);
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