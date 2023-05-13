using UnityEngine;

namespace Allies.Components
{
    public class BatonAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static int VictoryAnimationParameterHash { get; } = Animator.StringToHash("OnWin");

        public void MakeVictory()
        {
            _animator.SetTrigger(VictoryAnimationParameterHash);
        }
    }
}