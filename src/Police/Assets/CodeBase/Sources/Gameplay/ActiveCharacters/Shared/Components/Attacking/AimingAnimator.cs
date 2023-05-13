using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class AimingAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsAimingParameterHash = Animator.StringToHash("IsAiming");

        public void StartAiming()
        {
            _animator.SetBool(IsAimingParameterHash, true);
        }

        public void StopAiming()
        {
            _animator.SetBool(IsAimingParameterHash, false);
        }
    }
}