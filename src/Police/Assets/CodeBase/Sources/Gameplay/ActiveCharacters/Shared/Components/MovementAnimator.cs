using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class MovementAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static int RunAnimationParameterHash { get; } = Animator.StringToHash("IsRunning");

        public void SetMovementEnabled(bool state)
        {
            _animator.SetBool(RunAnimationParameterHash, state);
        }
    }
}