using Gameplay.PeopleDraw.EnergyConsumption;
using InputServices;
using NaughtyAttributes;
using Services.AdvertisingService;
using UnityEngine;
using Zenject;

namespace Monetization.AdvertisingPlane
{
    public class AdPlane : MonoBehaviour
    {
        [Inject] private SuggestAdvertisingWindow _window;
        [Inject] private RaycastInputService _raycastInput;
        [Inject] private IAdvertisingService _advertising;
        [Inject] private Energy _energy;
        [Inject] private AlmostLostDetector _almostLostDetector;
        
        [SerializeField] private AdPlaneObject _prefab;
        [SerializeField, Tooltip("screen normalized")] private float _viewportSpeedPerSecond;
        [SerializeField] private float _distanceFromCamera;
        [SerializeField] private float _viewportOffset = .2f;
        [SerializeField] private float _viewportOffsetLimit = 1.2f;

        private Camera _camera;
        private AdPlaneObject _planeInstance;
        
        private float _currentOffset;
        private bool _disabled = true;
        private bool _processingStarted;

        public void StartProcessing()
        {
            if (_processingStarted)
                return;
            _processingStarted = true;
            _almostLostDetector.NearlyLost += Show;
        }

        public void StopProcessing()
        {
            _processingStarted = false;
            _almostLostDetector.NearlyLost -= Show;
        }
        
        [Button]
        public void Show()
        {
            Initialize();
            _disabled = false;
            _almostLostDetector.NearlyLost -= Show;
        }

        private void Initialize()
        {
            _raycastInput.Detected += OnPlaneClicked;
            _camera ??= Camera.main;
            _planeInstance ??= Instantiate(_prefab);
        }

        private void OnPlaneClicked(IRaycastDetectableObject obj)
        {
            if (obj.GameObject.TryGetComponent<AdPlaneObject>(out var plane) == false)
                return;
            
            _window.ShowEnergyAdvertising(onAccept: () =>
            {
                _advertising.ShowRewarded(OnPlaneRewarded);
            }, _energy.MaxAmount);
        }

        private void OnPlaneRewarded()
        {
            _energy.RestoreEnergyToMax();
        }

        private void Update()
        {
            if (_disabled)
                return;

            _currentOffset += _viewportSpeedPerSecond * Time.deltaTime;
            Vector3 point = GetPoint(_currentOffset);
            _planeInstance.transform.position = point;
            if (_currentOffset >= _viewportOffsetLimit)
            {
                _currentOffset = 0;
                _disabled = true;
            }
        }

        private Vector3 GetPoint(float viewportOffset)
        {
            const float middleHeight = .5f;
            var leftMost = new Vector3((0 - _viewportOffset) + viewportOffset, middleHeight);
            Ray ray = _camera.ViewportPointToRay(leftMost);
            return ray.GetPoint(_distanceFromCamera);
        }
    }
}