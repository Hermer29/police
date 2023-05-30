using Helpers;
using ModestTree;
using UnityEngine;
using Range = Logic.Range;

namespace Interface
{
    public class RouletteArrow : MonoBehaviour
    {
        private const int DistanceFromCenter = 130;
        
        [SerializeField] private float _normalizedRotation;
        [SerializeField] private Range[] _sectors;
        [SerializeField] private float _rotationSpeed;
        
        private bool _isRotating;
        private float _currentRotation;
        private RectTransform _rectTransform;
        public float CurrentRotation => _currentRotation;

        private void OnValidate()
        {
            SetNormalizedRotation(_normalizedRotation);    
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void SetNormalizedRotation(float rotation)
        {
            var position = new Vector2(Mathf.Sin(rotation), Mathf.Cos(rotation));

            var angle = Quaternion.LookRotation(position)
                        * Quaternion.Euler(0, 90, -90);
            transform.rotation = angle;
            if (_rectTransform == null)
                GetComponent<RectTransform>().anchoredPosition = position * DistanceFromCenter;
            else
                _rectTransform.anchoredPosition = position * DistanceFromCenter;
        }

        private void Update()
        {
            if (_isRotating == false)
                return;

            _currentRotation = Mathf.Repeat(Mathf.MoveTowardsAngle(
                current: _currentRotation * Mathf.Rad2Deg,
                target: (_currentRotation + _rotationSpeed) * Mathf.Rad2Deg,
                maxDelta: _rotationSpeed * Mathf.Rad2Deg * Time.deltaTime) * Mathf.Deg2Rad, 6.27f);
            
            SetNormalizedRotation(_currentRotation);
        }

        public void RotateСontinuously()
        {
            _isRotating = true;
        }

        private void StopRotating()
        {
            _isRotating = false;
        }

        public float StopRotation()
        {
            StopRotating();
            return _currentRotation;
        }

        public int GetPrizeIndex()
        {
            var hitRange = _sectors.IndexOf(_sectors.GetHitRange(CurrentRotation));
            Debug.Log(hitRange);
            return hitRange;
        }
    }
}