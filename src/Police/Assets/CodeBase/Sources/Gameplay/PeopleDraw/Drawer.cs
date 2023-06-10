using System.Threading.Tasks;
using Gameplay.PeopleDraw.Factory;
using Infrastructure.Services.UseService;
using PeopleDraw.Components;
using PeopleDraw.Selection;
using Services;
using Services.UseService;
using UnitSelection;
using UnityEngine;
using UnityEngine.AI;
using Upgrading.UnitTypes;

namespace PeopleDraw
{
    public class Drawer
    {
        private readonly IInputService _inputService;
        private readonly SelectedUnits _unitSelectedUnits;
        private readonly IAlliedUnitsFactory _factory;
        private readonly IUnitsFxFactory _fxFactory;
        private readonly UnitsUsingService _usingService;
        private readonly UnitsAssetCache _unitsAssetCache;

        private PoolUnit _currentDrawingUnplacableUnit;

        public Drawer(IInputService inputService, SelectedUnits unitSelectedUnits, IAlliedUnitsFactory factory, IUnitsFxFactory fxFactory,
            UnitsUsingService usingService, UnitsAssetCache unitsAssetCache)
        {
            _inputService = inputService;
            _unitSelectedUnits = unitSelectedUnits;
            _factory = factory;
            _fxFactory = fxFactory;
            _usingService = usingService;
            _unitsAssetCache = unitsAssetCache;

            _inputService.DrawnAtPoint += InputServiceOnDrawnAtPoint;
            Debug.Log($"{nameof(Drawer)} constructed");
        }

        private PoolUnit CurrentUnit => _unitsAssetCache.SelectedUnits[_unitSelectedUnits.SelectedType.Value];

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
            if (lookDirection.sqrMagnitude > 40)
            {
                unit.transform.rotation = Quaternion.Euler(0, -180, 0);
                return;
            }
            unit.transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, -90, 0);
        }

        private bool CantDrawUnit(RaycastHit drawPoint)
        {
            return NoIntersection(drawPoint) || NoSelected() ||  NotEnoughEnergyToSpawnSelected();
        }

        private bool NoSelected() => _unitSelectedUnits.SelectedType.Value == UnitType.None;

        private bool NotEnoughEnergyToSpawnSelected() =>
            _unitSelectedUnits.HasEnoughEnergyToSpawnSelected() == false;

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
                prefab: CurrentUnit,
                position: drawPoint.point,
                selectedSerial: (int)_unitSelectedUnits.SelectedType.Value);
            
            if (unit.TryGetComponent(out NavMeshAgent agent)) 
                agent.Warp(drawPoint.point);

            return unit;
        }
    }
}