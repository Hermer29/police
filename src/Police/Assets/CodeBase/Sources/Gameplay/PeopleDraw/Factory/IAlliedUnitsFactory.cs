using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.PeopleDraw.Factory
{
    public interface IAlliedUnitsFactory
    {
        PoolUnit InstantiateDrawnUnit(PoolUnit prefab, Vector3 position, int selectedSerial);
        ObjectPool<PoolUnit> CreateUnitsPool(PoolUnit prefab);
        void AnimatorsPlayVictory();
        void ReturnAllToPool();
    }
}