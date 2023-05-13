using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using UnityEngine;
using UnityEngine.Serialization;

namespace ActiveCharacters.Allies.Components
{
    public class PolicemenAggro : MonoBehaviour
    {
        [SerializeField] private NearestTargetListener _reader;
        [FormerlySerializedAs("_movingToTargetAttack")] [SerializeField] private DetectionReaction _detectionReaction;
        [SerializeField] private CheckAttackRange _rangeChecker;
        
        private Attackable _target;
        private bool _stopped;

        private void Start()
        {
            _reader.ClosestObjectApproached += TriggerEntered;
            _reader.ClosestObjectLeftTheZone += LostTarget;
        }

        private void LostTarget(Collider obj)
        {
            if (_stopped)
                return;
            DiscardTarget();
            _reader.FindAgain();
        }

        private void Update()
        {
            if (_stopped)
                return;
            if (NeedToForgetDeadTarget())
            {
                DiscardTarget();
                _reader.FindAgain();
            }
        }

        private bool NeedToForgetDeadTarget()
        {
            return TargetNotDiscarded() && TargetDead();
        }

        private bool TargetNotDiscarded()
        {
            return _target != null;
        }

        private bool TargetDead()
        {
            return _target.Died;
        }

        private void TriggerEntered(Collider other)
        {
            if (_stopped)
                return;
            if (other.TryGetComponent(out Attackable attackable)) DefineTarget(attackable);
        }

        private void DefineTarget(Attackable target)
        {
            _detectionReaction.Follow(target.Root);
            _rangeChecker.CheckFor(target);
            _target = target;
        }

        private void DiscardTarget()
        {
            _rangeChecker.StopChecking();
            _detectionReaction.Forget();
            _target = null;
        }

        public void Stop()
        {
            DiscardTarget();
        }
    }
}