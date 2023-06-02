using System;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class EnergyKillRestorationViewItem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Vector3 _punch;
        [SerializeField] private float _expandTime;
        [SerializeField] private float _shrinkTime;
        [SerializeField] private float _livingTime;
        
        public void Show(Action release)
        {
            transform.localScale = Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_punch, _expandTime));
            sequence.Append(transform.DOScale(Vector3.zero, _shrinkTime));
            Observable.Timer(TimeSpan.FromSeconds(_livingTime))
                .Subscribe(_ => release())
                .AddTo(this);
        }

        [Button]
        public void Execute() =>
            Show(() => Debug.Log($"{nameof(EnergyKillRestorationViewItem)} animation ended"));
    }
}