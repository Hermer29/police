using UnityEngine;

namespace ActiveCharacters.Shared.Components.Attacking
{
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        
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

        private bool TargetIsNear()
        {
            return (_target.Root.position - transform.position).sqrMagnitude <= _range * _range;
        }

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