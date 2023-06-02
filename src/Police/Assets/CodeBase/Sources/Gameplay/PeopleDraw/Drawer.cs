using System.Threading.Tasks;
using Gameplay.PeopleDraw.Factory;
using PeopleDraw.Components;
using PeopleDraw.Selection;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace PeopleDraw
{
    public class Drawer
    {
        private readonly IInputService _inputService;
        private readonly SelectedUnits _unitSelectedUnits;
        private readonly IAlliedUnitsFactory _factory;
        private readonly IUnitsFxFactory _fxFactory;

        private PoolUnit _currentDrawingUnplacableUnit;

        public Drawer(IInputService inputService, SelectedUnits unitSelectedUnits, IAlliedUnitsFactory factory, IUnitsFxFactory fxFactory)
        {
            _inputService = inputService;
            _unitSelectedUnits = unitSelectedUnits;
            _factory = factory;
            _fxFactory = fxFactory;

            _inputService.DrawnAtPoint += InputServiceOnDrawnAtPoint;
            Debug.Log($"{nameof(Drawer)} constructed");
        }

        private void InputServiceOnDrawnAtPoint(RaycastHit drawPoint, RaycastHit previous)
        {
            if (CantDrawUnit(drawPoint))
                return;

            PoolUnit unit = CreateUnit(drawPoint);
            if (Blocked(unit))
            {
                unit.ReturnToPool();
                return;
            }
            
            Rotate(drawPoint, previous, unit);
            ShowSpawnFx(drawPoint);
            SpendEnergy();
        }

        private static void Rotate(RaycastHit drawPoint, RaycastHit previous, PoolUnit unit)
        {
            Vector3 lookDirection = drawPoint.point - previous.point;
            const float minimalDistance = .1f;
            unit.transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, -90, 0);
        }

        private bool CantDrawUnit(RaycastHit drawPoint)
        {
            return NoIntersection(drawPoint) || UnitNotSelected() || NotEnoughEnergyToSpawnSelected();
        }

        private bool NotEnoughEnergyToSpawnSelected() => 
            _unitSelectedUnits.HasEnoughEnergyToSpawn() == false;

        private bool UnitNotSelected() => 
            _unitSelectedUnits.Current == null;

        private static bool NoIntersection(RaycastHit drawPoint) => 
            drawPoint.collider == null;

        private void SpendEnergy() => 
            _unitSelectedUnits.NotifyUnitDrawn();

        private void ShowSpawnFx(RaycastHit drawPoint) => 
            _fxFactory.CreateSpawnFx(drawPoint.point);

        private static bool Blocked(PoolUnit unit)
        {
            var placingBlock = unit.GetComponentInChildren<PlacingBlock>();
            return placingBlock != null && placingBlock.OverlapsOtherUnit();
        }

        private PoolUnit CreateUnit(RaycastHit drawPoint)
        {
            PoolUnit unit = _factory.InstantiateDrawnUnit(
                prefab: _unitSelectedUnits.Current.Value,
                position: drawPoint.point,
                selectedSerial: (int)_unitSelectedUnits.CurrentType.Value);
            
            if (unit.TryGetComponent(out NavMeshAgent agent)) 
                agent.Warp(drawPoint.point);

            return unit;
        }
    }
}