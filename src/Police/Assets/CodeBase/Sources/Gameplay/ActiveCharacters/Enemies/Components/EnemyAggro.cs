using ActiveCharacters.Shared.Components.Attacking;
using Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace ActiveCharacters.Shared.Components
{
    public class EnemyAggro : MonoBehaviour
    {
        [SerializeField] private NearestTargetListener _nearest;
        [SerializeField] private MoveToTarget _movingToTargetAttack;
        [SerializeField] private RunToWin _runToWin;
        [FormerlySerializedAs("_rangeChecker")] [SerializeField] private CheckAttackRangeByAgentDistance _rangeByAgentDistanceChecker;
        
        private Attackable _target;
        private bool _stopped;

        private void Start()
        {
            _runToWin.enabled = true;
            
            _nearest.ClosestObjectApproached += TriggerEntered;
            _nearest.ClosestObjectLeftTheZone += LostTarget;
        }

        private void Update()
        {
            if (_stopped) return;
            
            if (NeedToForgetDeadTarget())
            {
                TryChangeTarget();
            }
        }

        private void TryChangeTarget()
        {
            DiscardTarget();
            _runToWin.enabled = true;
            _nearest.FindAgain();
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

        private static bool OtherNotAttackable(Collider other, out Attackable marker)
        {
            return !other.TryGetComponent(out marker);
        }

        private void DefineTarget(Attackable marker)
        {
            _target = marker;
            _runToWin.enabled = false;
            _movingToTargetAttack.Follow(marker.Root);
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