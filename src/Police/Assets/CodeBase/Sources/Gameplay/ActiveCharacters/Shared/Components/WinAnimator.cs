using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components
{
    public class WinAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static int VictoryAnimationTriggerHash = Animator.StringToHash("OnWin");

        public void PlayVictory() => 
            _animator.SetTrigger(VictoryAnimationTriggerHash);
    }
}