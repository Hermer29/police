using System;
using System.Collections;
using ActiveCharacters.Shared.Components.Attacking;
using Enemies.Components;
using Helpers;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Gizmos = Popcron.Gizmos;

namespace ActiveCharacters.Shared.Components
{
    public class EnemyAggro : MonoBehaviour
    {
        [SerializeField] private NearestTargetListener _nearest;
        [SerializeField] private MoveToTarget _movingToTargetAttack;
        [SerializeField] private RunToWin _runToWin;
        [FormerlySerializedAs("_rangeChecker")] [SerializeField] private CheckAttackRangeByAgentDistance _rangeByAgentDistanceChecker;
        [SerializeField] private NavMeshAgent _agent;

        private RaycastHit[] _buffer = new RaycastHit[20];
        private Attackable _target;
        private bool _stopped;

        private void Start()
        {
            _runToWin.enabled = true;
            
            //_nearest.ClosestObjectApproached += TriggerEntered;
            //_nearest.ClosestObjectLeftTheZone += LostTarget;
        }

        private void OnEnable()
        {
            StartCoroutine(LookingForTargets());
        }

        private IEnumerator LookingForTargets()
        {
            var player = 1 << LayerMask.NameToLayer("Policemen");
            var checkBoxScale = new Vector3(.5f, .5f, .5f);
            var height = _agent.transform.position.ProjectOnDrawPlane().y + .5f;
            while (true)
            {
                var vectorToDestination = (_runToWin.Destination.SetY(height) - _agent.transform.position.SetY(height))
                    .normalized;
                var source = _agent.transform.position.SetY(height) - vectorToDestination;
                //DelayedGizmo.Line(source, source + vectorToDestination, Color.white);
                var count = Physics.BoxCastNonAlloc(source, 
                    new Vector3(.2f, .2f, .2f),
                    direction: vectorToDestination, 
                    results: _buffer, 
                    transform.rotation,
                    maxDistance: 20, 
                    layerMask: player);
                if (count != 0)
                {
                    var nearest = _buffer[0];
                    TriggerEntered(nearest.collider);
                    //DelayedGizmo.Sphere(nearest.point, .5f, Color.white);
                }
                yield return new WaitForSeconds(2f);
            }
        }

        private void Update()
        {
            if (_stopped) return;


            if (_target)
            {
                Gizmos.Line(transform.position, _target.Root.position, Color.red);
            }
            
            if (NeedToForgetDeadTarget())
            {
                TryChangeTarget();
            }
        }

        private void TryChangeTarget()
        {
            DiscardTarget();
            _runToWin.enabled = true;
            //_nearest.FindAgain();
        }

        public void Stop()
        {
            _stopped = true;
            _runToWin.enabled = false;
            DiscardTarget();
        }

        private void LostTarget(Collider obj)
        {
            if (_stopped) return;
            
            TryChangeTarget();
        }

        private bool NeedToForgetDeadTarget()
        {
            return TargetDefined() && TargetDead();
        }

        private bool TargetDefined()
        {
            return _target != null;
        }

        private bool TargetDead()
        {
            return _target.Died;
        }

        private void TriggerEntered(Collider other)
        {
            if (_stopped) return;
            if (OtherNotAttackable(other, out Attackable marker)) 
                return;
            DefineTarget(marker);
        }

        private static bool OtherNotAttackable(Collider other, out Attackable marker) => !other.TryGetComponent(out marker);

        private void DefineTarget(Attackable marker)
        {
            _target = marker;
            _runToWin.enabled = false;
            _movingToTargetAttack.Follow(marker.GetComponent<BoxCollider>());
            _rangeByAgentDistanceChecker.CheckFor(marker);
        }

        private void DiscardTarget()
        {
            _target = null;
            _rangeByAgentDistanceChecker.StopChecking();
            _movingToTargetAttack.Forget();
        }
    }
}