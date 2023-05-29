using System.Threading.Tasks;
using PeopleDraw.Components;
using UnityEngine;
using Upgrading.UnitTypes;

namespace Gameplay.PeopleDraw.Factory
{
    public interface IAlliedUnitsFactory
    {
        Task<PoolUnit> InstantiateDrawnUnit(PoolUnit prefab, Vector3 position, int selectedSerial);
        void AnimatorsPlayVictory();
        void ReturnAllToPool();
    }
}