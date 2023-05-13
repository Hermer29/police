using Gameplay.PeopleDraw.Factory;
using PeopleDraw.Components;
using PeopleDraw.EnergyConsumption;
using PeopleDraw.Selection;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace PeopleDraw
{
    public class Drawer
    {
        private readonly IInputService _inputService;
        private readonly Selector _unitSelector;
        private readonly IAlliedUnitsFactory _factory;

        private PoolUnit _currentDrawingUnplacableUnit;

        public Drawer(IInputService inputService, Selector unitSelector, IAlliedUnitsFactory factory)
        {
            _inputService = inputService;
            _unitSelector = unitSelector;
            _factory = factory;

            _inputService.DrawnAtPoint += InputServiceOnDrawnAtPoint;
        }

        private void InputServiceOnDrawnAtPoint(RaycastHit drawPoint, RaycastHit previous)
        {
            if (drawPoint.collider == null)
                return;
            if (_unitSelector.Current == null)
                return;
            if (_unitSelector.HasEnoughEnergyToSpawn() == false)
                return;

            PoolUnit unit = CreateUnit(drawPoint);
            var placingBlock = unit.GetComponentInChildren<PlacingBlock>();
            if (placingBlock != null && placingBlock.OverlapsOtherUnit())
            {
                unit.ReturnToPool();
                return;
            }
            _unitSelector.NotifyUnitDrawn();
        }

        private PoolUnit CreateUnit(RaycastHit drawPoint)
        {
            PoolUnit unit = _factory.InstantiateDrawnUnit(
                prefab: _unitSelector.Current,
                position: drawPoint.point,
                selectedSerial: _unitSelector.CurrentSerialNonIndex);
            if (unit.TryGetComponent<NavMeshAgent>(out var agent))
            {
                agent.Warp(drawPoint.point);
            }
            
            return unit;
        }
    }
}