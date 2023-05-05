using System;
using System.Collections.Generic;
using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PeopleDraw.Factory
{
    public class UnitsFactory
    {
        private Lazy<Dictionary<int, ObjectPool<PoolUnit>>> _poolForTypes = new(
            valueFactory: () => new Dictionary<int, ObjectPool<PoolUnit>>());
        
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
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj => obj.gameObject.SetActive(false));
            return pool;
        }

        private static PoolUnit FactorizeUnit(PoolUnit prefab, ObjectPool<PoolUnit> pool)
        {
            PoolUnit unit = Object.Instantiate(prefab);
            unit.Construct(pool);
            return unit;
        }
    }
}