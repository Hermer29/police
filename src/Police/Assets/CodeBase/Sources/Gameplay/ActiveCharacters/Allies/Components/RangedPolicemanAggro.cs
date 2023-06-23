using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.ActiveCharacters.Allies.Components
{
    public class RangedPolicemanAggro : AlliedAggro
    {
        [SerializeField] private NearestTargetListener _reader;
        [FormerlySerializedAs("_movingToTargetAttack")] [SerializeField] private DetectionReaction _detectionReaction;
        [FormerlySerializedAs("_rangeChecker")] [SerializeField] private Attack _attack;
        
        private Attackable _target;
        private bool _stopped;

        public float TargetDistance => (transform.position - _target.transform.position).magnitude;
        public bool TargetDefined => _target != null;
        
        private void Start()
        {
            _reader.ClosestObjectApproached += TriggerEntered;
            _reader.ClosestObjectLeftTheZone += LostTarget;
            _reader.FindAgain();
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
            _detectionReaction.Follow(target.GetComponent<BoxCollider>());
            _attack.EnableAttack(target);
            _target = target;
        }

        private void DiscardTarget()
        {
            _attack.DisableAttack();
            _detectionReaction.Forget();
            _target = null;
        }

        public override void Stop()
        {
            DiscardTarget();
        }

        public override void Enable()
        {
            _stopped = false;
        }
    }
}