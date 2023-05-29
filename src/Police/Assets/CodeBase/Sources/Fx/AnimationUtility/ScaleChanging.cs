using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace AnimationUtility
{
    [RequireComponent(typeof(RectTransform))]
    public class ScaleChanging : Animation
    {
        [SerializeField] private Vector2[] _scaleChanges;
        [SerializeField] private float _transitionTime;

        private RectTransform _transform;
        private Sequence _sequence;

        public override void Stop()
        {
            gameObject.SetActive(false);
            _sequence.Kill();
        }

        public override void Play()
        {
            _transform = GetComponent<RectTransform>();
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
            return _scaleChanges.Select(point => 
                _transform.DOSizeDelta(point, _transitionTime).SetEase(Ease.InOutExpo));
        }
    }
}