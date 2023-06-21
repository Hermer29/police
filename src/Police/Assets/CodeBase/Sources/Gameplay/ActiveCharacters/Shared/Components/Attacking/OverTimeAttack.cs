using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using DefaultNamespace.Audio.Components;
using Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class OverTimeAttack : Attack
    {
        private Attackable _target;
        
        [SerializeField] private AimingAnimator _animator;
        [SerializeField] private OverTimeEffector[] _effector;
        
        public override void EnableAttack(Attackable target)
        {
            _target = target;
            _animator.StartAiming();
            foreach (OverTimeEffector overTimeEffector in _effector)
            {
                overTimeEffector.StartExecuting(target);
            }
        }

        public override void DisableAttack()
        {
            _animator.StopAiming();
            foreach (OverTimeEffector overTimeEffector in _effector)
            {
                overTimeEffector.EndExecuting();
            }
        }
    }
}