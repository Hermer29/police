using System.Threading.Tasks;
using Helpers;
using PeopleDraw.Components;
using UnityEngine;
using Upgrading.UnitTypes;

namespace Gameplay.PeopleDraw.Factory
{
    public interface IAlliedUnitsFactory : IManuallyInitializable
    {
        PoolUnit InstantiateDrawnUnit(UnitType unitType, Vector3 position);
        void AnimatorsPlayVictory();
        void ReturnAllToPool();
        int ActivePolicemenAmount { get; }
        Task<GameObject> CreateSuperUnit();
        Task<GameObject> CreateHelicopter();
    }
}