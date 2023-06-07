using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components.Attacking
{
    public class CheckAttackRangeByAgentDistance : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _range;
        
        private Attackable _target;

        private void Start()
        {
            _attack.DisableAttack();
        }

        private void Update()
        {
            if (_target == null)
            {
                _attack.DisableAttack();
                return;
            }

            if (TargetIsNear())
            {
                _attack.EnableAttack(_target);
            }
            else
            {
                _attack.DisableAttack();
            }
        }

        private bool TargetIsNear() => _agent.remainingDistance <= _range * _range;

        public void CheckFor(Attackable target)
        {
            _target = target;
        }
        
        public void StopChecking()
        {
            _target = null;
        }
    }
}