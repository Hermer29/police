using PeopleDraw.Components;
using PeopleDraw.Factory;
using PeopleDraw.Selection;
using Services;
using UnityEngine;

namespace PeopleDraw
{
    public class Drawer
    {
        private readonly IInputService _inputService;
        private readonly Selector _unitSelector;
        private readonly UnitsFactory _factory;

        private PoolUnit _currentDrawingUnplacableUnit;

        public Drawer(IInputService inputService, Selector unitSelector, UnitsFactory factory)
        {
            _inputService = inputService;
            _unitSelector = unitSelector;
            _factory = factory;

            _inputService.DrawnAtPoint += InputServiceOnDrawnAtPoint;
        }

        private void InputServiceOnDrawnAtPoint(RaycastHit drawPoint)
        {
            if (_unitSelector.Current == null)
                return;
            
            PoolUnit unit = CreateUnit(drawPoint);
            var placingBlock = unit.GetComponentInChildren<PlacingBlock>();
            if (placingBlock.OverlapsOtherUnit())
            {
                unit.ReturnToPool();
            }
        }

        private PoolUnit CreateUnit(RaycastHit drawPoint)
        {
            PoolUnit unit = _factory.InstantiateDrawnUnit(
                prefab: _unitSelector.Current,
                position: drawPoint.point,
                selectedSerial: _unitSelector.CurrentSerialNonIndex);
            return unit;
        }
    }
}