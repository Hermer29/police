using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using Gameplay.ActiveCharacters.Allies.Components;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ActiveCharacters.Allies.Components
{
    public class PolicemenAggro : AlliedAggro
    {
        [SerializeField] private NearestTargetListener _reader;
        [FormerlySerializedAs("_movingToTargetAttack")] [SerializeField] private DetectionReaction _detectionReaction;
        [FormerlySerializedAs("_rangeChecker")] [SerializeField] private CheckAttackRangeByAgentDistance _rangeByAgentDistanceChecker;
        
        [SerializeField, ReadOnly] private Attackable _target;
        [SerializeField, ReadOnly] private bool _stopped;
        [SerializeField, ReadOnly] private float _targetTimeout;

        private void Start()
        {
            _reader.ClosestObjectApproached += TriggerEntered;
            _reader.FindAgain();
        }

        private void Update()
        {
            if (_stopped)
                return;
            _targetTimeout += Time.deltaTime;
            if (NeedToForgetDeadTarget())
            {
                _targetTimeout = 0;
                DiscardTarget();
                _reader.FindAgain();
            }
        }

        private bool NeedToForgetDeadTarget()
        {
            return (TargetNotDiscarded() && TargetDead()) || TargetTimeoutReached();
        }

        private bool TargetTimeoutReached()
        {
            return _targetTimeout > 2f;
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
            _detectionReaction.Follow(target.GetComponent<BoxCollider>());
            _rangeByAgentDistanceChecker.CheckFor(target);
            _target = target;
        }

        private void DiscardTarget()
        {
            _rangeByAgentDistanceChecker.StopChecking();
            _detectionReaction.Forget();
            _target = null;
        }

        public override void Stop()
        {
            DiscardTarget();
        }

        public override void Enable() => _stopped = false;
    }
}