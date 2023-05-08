using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class AttackingAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static int AttackAnimationParameterHash { get; } = Animator.StringToHash("OnAttack");
        private static int IdleAnimationHash { get; } = Animator.StringToHash("Idle");

        public void Attack()
        {
            _animator.SetTrigger(AttackAnimationParameterHash);
        }

        public void StopAttack()
        {
            _animator.Play(IdleAnimationHash);
        }
    }
}