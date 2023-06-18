using System.Threading.Tasks;
using DefaultNamespace.Audio;
using Gameplay.PeopleDraw.Factory;
using Infrastructure.Services.UseService;
using PeopleDraw.Components;
using PeopleDraw.Selection;
using Services;
using Services.UseService;
using UniRx;
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
        private readonly UsedUnitsService _service;
        private readonly UnitsAssetCache _unitsAssetCache;
        private readonly GlobalAudio _audio;

        private PoolUnit _currentDrawingUnplacableUnit;
        private float _cooldown;

        public Drawer(IInputService inputService, SelectedUnits unitSelectedUnits, IAlliedUnitsFactory factory, IUnitsFxFactory fxFactory,
            UsedUnitsService service, UnitsAssetCache unitsAssetCache, GlobalAudio audio)
        {
            _inputService = inputService;
            _unitSelectedUnits = unitSelectedUnits;
            _factory = factory;
            _fxFactory = fxFactory;
            _service = service;
            _unitsAssetCache = unitsAssetCache;
            _audio = audio;

            _inputService.DrawnAtPoint += InputServiceOnDrawnAtPoint;
            Debug.Log($"{nameof(Drawer)} constructed");
            Observable.EveryUpdate().Subscribe(_ => _cooldown -= Time.deltaTime);
        }

        private PoolUnit CurrentUnit => _unitsAssetCache.SelectedUnits[_unitSelectedUnits.SelectedType.Value];

        private void InputServiceOnDrawnAtPoint(RaycastHit drawPoint, RaycastHit previous)
        {
            if(_cooldown > 0)
                return;
            
            if (CantDrawUnit(drawPoint))
                return;

            if (CurrentUnit.PlacingBlock.OverlapsOtherUnit(drawPoint.point))
                return;

            _cooldown = .1f;
            PoolUnit unit = CreateUnit(drawPoint);
            Rotate(drawPoint, previous, unit);
            ShowSpawnFx(drawPoint);
            _audio.PlayUnitPlacement();
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

            var relativeAngle = -90;
            if (Vector3.Dot(lookDirection, Vector3.right) > 0)
            {
                relativeAngle = 90;
            }
            
            unit.transform.rotation = Quaternion.LookRotation(lookDirection) 
                                      * Quaternion.Euler(0, relativeAngle, 0);
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

        private PoolUnit CreateUnit(RaycastHit drawPoint)
        {
            PoolUnit unit = _factory.InstantiateDrawnUnit(
                unitType: _unitSelectedUnits.SelectedType.Value,
                position: drawPoint.point);
            
            if (unit.TryGetComponent(out NavMeshAgent agent)) 
                agent.Warp(drawPoint.point);

            return unit;
        }
    }
}