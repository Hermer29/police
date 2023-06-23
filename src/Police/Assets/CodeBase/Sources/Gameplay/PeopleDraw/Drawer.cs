using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Shared.Components;
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
        private Line? _line;
        private Vector3? _prevpoint;

        private const float SpawnDistanceStep = 1;

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
            _inputService.DragInProgress += InputServiceDragInProgress;
            _inputService.DragStarted += OnDragStarted;
            _inputService.DragEnded += OnDragEnded;
            Debug.Log($"{nameof(Drawer)} constructed");
            Observable.EveryUpdate().Subscribe(_ => _cooldown -= Time.deltaTime);
        }

        private void OnDragEnded()
        {
            _line = null;
        }

        private void OnDragStarted(Vector3 startPoint)
        {
            _prevpoint = startPoint;
            //_line = new Line(SpawnDistanceStep, startPoint);
        }

        private PoolUnit CurrentUnit => _unitsAssetCache.SelectedUnits[_unitSelectedUnits.SelectedType.Value];

        private void InputServiceOnDrawnAtPoint(RaycastHit drawPoint)
        {
            // if(_cooldown > 0)
            //     return;
            
            if (CantDrawUnit(drawPoint))
                return;

            // if (CurrentUnit.PlacingBlock.OverlapsOtherUnit(drawPoint.point))
            //     return;
            
            SpawnSingle(drawPoint);
        }
        
        private void InputServiceDragInProgress(RaycastHit drawPoint)
        {
            
            // if(_cooldown > 0)
            //     return;
            
            if (CantDrawUnit(drawPoint))
                return;

            // if (CurrentUnit.PlacingBlock.OverlapsOtherUnit(drawPoint.point))
            //     return;
            
            Vector3 fromToVector = drawPoint.point - _prevpoint.Value;
            if (fromToVector.magnitude < 1)
                return;
            int relativeAngle = -90;
            if (Vector3.Dot(fromToVector, Vector3.right) > 0)
            {
                relativeAngle = 90;
            }
            Quaternion rotation = Quaternion.LookRotation(fromToVector) 
                                  * Quaternion.Euler(0, relativeAngle, 0);
            SpawnSerial(drawPoint.point, rotation);
            _prevpoint = drawPoint.point;
            // foreach ((Vector3 appendAndGetNewPoint, Quaternion rotation) in _line.Value.AppendAndGetNewPoints(drawPoint.point))
            // {
            //     
            // }
        }

        private void SpawnSerial(Vector3 drawPoint, Quaternion rotation)
        {
            PoolUnit unit = CreateUnit(drawPoint);
            unit.transform.rotation = rotation;
            ShowSpawnFx(drawPoint);
            _audio.PlayUnitPlacement();
            SpendEnergy();
        }

        private void SpawnSingle(RaycastHit drawPoint)
        {
            PoolUnit unit = CreateUnit(drawPoint.point);
            unit.transform.rotation = Quaternion.Euler(0, -180, 0);
            ShowSpawnFx(drawPoint.point);
            _audio.PlayUnitPlacement();
            SpendEnergy();
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

        private void ShowSpawnFx(Vector3 drawPoint) => 
            _fxFactory.CreateSpawnFx(drawPoint);

        private PoolUnit CreateUnit(Vector3 drawPoint)
        {
            PoolUnit unit = _factory.InstantiateDrawnUnit(
                unitType: _unitSelectedUnits.SelectedType.Value,
                position: drawPoint);
            var balance = _service.UsedUnits[_unitSelectedUnits.SelectedType.Value].CalculateBalance();
            unit.GetComponent<CharactersNavigationLinks>().SetBalance(balance);

            if (unit.TryGetComponent(out NavMeshAgent agent)) 
                agent.Warp(drawPoint);

            return unit;
        }
    }
}