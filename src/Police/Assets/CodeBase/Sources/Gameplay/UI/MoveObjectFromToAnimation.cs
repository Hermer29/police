using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.UI
{
    public class MoveObjectFromToAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;

        [SerializeField] private float _time;
        
        private bool _isPositionedOnA = true;
        
        [Button]
        public void Execute()
        {
            if (_isPositionedOnA)
            {
                transform.DOMove(_pointB.position, _time).SetEase(Ease.InExpo);
                _isPositionedOnA = false;
                return;
            }
            transform.DOMove(_pointA.position, _time).SetEase(Ease.InExpo);
            _isPositionedOnA = true;
        }
    }
}