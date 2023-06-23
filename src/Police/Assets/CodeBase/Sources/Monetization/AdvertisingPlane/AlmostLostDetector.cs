using System;
using System.Collections;
using Gameplay.Levels;
using Gameplay.PeopleDraw.Factory;
using Infrastructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace Monetization.AdvertisingPlane
{
    public class AlmostLostDetector : MonoBehaviour
    {
        [Inject] private IAlliedUnitsFactory _unitsFactory;
        
        private Coroutine _waitingForMomentToShow;
        private bool _waitingForMoment;
        private bool _unitsHeavilySpawned;
        private IDisposable _subscription;

        public event Action NearlyLost;
        
        public void StartWaitingForMoment()
        {
            if (_waitingForMoment)
                return;
            _subscription = AllServices.Get<LooseTrigger>().OnAlmostLost.Subscribe(triggered =>
            {
                EndProcessing();
            });
            _waitingForMoment = true;
            _waitingForMomentToShow = StartCoroutine(Worker());
        }
        
        private IEnumerator Worker()
        {
            const int heavyAmount = 5;
            while (true)
            {
                if(_waitingForMoment == false)
                    yield break;
                
                if (PlayerOutOfEnergyAndEnemiesAlmostWon(heavyAmount))
                {
                    EndProcessing();
                    yield break;
                }
                
                if (PlayerJustSpentBigAmountOfEnergy(heavyAmount))
                {
                    Debug.Log("Units heavily spawned detected");
                    _unitsHeavilySpawned = true;
                }

                yield return null;
            }
        }

        private void EndProcessing()
        {
            Debug.Log("Almost lost detected");
            _waitingForMoment = false;
            _subscription.Dispose();
            NearlyLost?.Invoke();
        }

        public void Uninitialize()
        {
            if (_waitingForMoment == false)
                return;
            _unitsHeavilySpawned = false;
            StopCoroutine(_waitingForMomentToShow);
        }
        
        private bool PlayerOutOfEnergyAndEnemiesAlmostWon(int heavyAmount)
        {
            return _unitsHeavilySpawned && _unitsFactory.ActivePolicemenAmount < heavyAmount;
        }

        private bool PlayerJustSpentBigAmountOfEnergy(int heavyAmount)
        {
            return _unitsFactory.ActivePolicemenAmount > heavyAmount && _unitsHeavilySpawned == false;
        }
    }
}