using DG.Tweening;
using UnityEngine;

namespace AnimationUtility
{
    public class ShakingAnimation : Animation
    {
        [SerializeField] private float _strength;
        [SerializeField] private float _duration;
        
        public override void Stop()
        {
            transform.DOKill();
        }

        public override void Play()
        {
            Stop();
            transform.DOShakePosition(_duration, _strength);
        }
    }
}