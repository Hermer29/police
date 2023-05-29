using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace AnimationUtility
{
    public class MovingByTrajectory : Animation
    {
        [SerializeField] private Vector3[] _localPoints;
        [SerializeField] private float _transitionTime;
        
        private Sequence _sequence;
        
        public override void Stop()
        {
            gameObject.SetActive(false);
            _sequence.Kill();
        }

        public override void Play()
        {
            gameObject.SetActive(true);
            _sequence = DOTween.Sequence();
            foreach (Tween tween in GetTweens())
            {
                _sequence.Append(tween);
            }
            _sequence.SetLoops(-1, LoopType.Restart);
        }

        private IEnumerable<Tween> GetTweens()
        {
            return _localPoints.Select(point => 
                    transform.DOLocalMove(point, _transitionTime).SetEase(Ease.InOutExpo));
        }
    }
}