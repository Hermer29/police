using System;
using System.Collections;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
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
        [Inject] private IAlliedUnitsFactory _unitsFactory;
        [Inject] private IAdvertisingService _advertising;
        [Inject] private Energy _energy;
        
        [SerializeField] private AdPlaneObject _prefab;
        [SerializeField, Tooltip("screen normalized")] private float _viewportSpeedPerSecond;
        [SerializeField] private float _distanceFromCamera;
        [SerializeField] private float _viewportOffset = .2f;
        [SerializeField] private float _viewportOffsetLimit = 1.2f;

        private Camera _camera;
        private AdPlaneObject _planeInstance;
        private bool _unitsHeavilySpawned;
        private Coroutine _waitingForMomentToShow;
        private bool _waitingForMoment;
        private float _currentOffset;
        private bool _disabled = true;

        [Button]
        public void Show()
        {
            Initialize();
            _disabled = false;
        }

        public void StartWaitingForMoment()
        {
            _waitingForMoment = true;
            _waitingForMomentToShow = StartCoroutine(WaitForMomentToShowThePlane());
        }

        private IEnumerator WaitForMomentToShowThePlane()
        {
            const int heavyAmount = 10;
            while (true)
            {
                if (PlayerOutOfEnergyAndEnemiesAlmostWon(heavyAmount))
                {
                    Show();
                    _waitingForMoment = false;
                    NearlyLost?.Invoke();
                    yield break;
                }
                
                if (PlayerJustSpentBigAmountOfEnergy(heavyAmount))
                {
                    _unitsHeavilySpawned = true;
                }

                yield return null;
            }
        }

        private bool PlayerOutOfEnergyAndEnemiesAlmostWon(int heavyAmount)
        {
            return _unitsHeavilySpawned && _unitsFactory.ActivePolicemenAmount < heavyAmount;
        }

        private bool PlayerJustSpentBigAmountOfEnergy(int heavyAmount)
        {
            return _unitsFactory.ActivePolicemenAmount > heavyAmount && _unitsHeavilySpawned == false;
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
            Uninitialize();
        }

        public void Uninitialize()
        {
            if (_waitingForMoment == false)
                return;
            _unitsHeavilySpawned = false;
            StopCoroutine(_waitingForMomentToShow);
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

        public event Action NearlyLost;
    }
}