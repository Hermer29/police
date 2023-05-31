using System;
using System.Collections;
using System.Globalization;
using Codice.Client.Common;
using Services.PrefsService;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.UI
{
    public class RandomTimer : MonoBehaviour
    {
        private IPrefsService _prefsService;
        
        public int minutes;
        public int seconds;
        public int hours;

        [SerializeField] private string _prefsIdentifier;
        [SerializeField] private LocalizeStringEvent _field;
        
        private DateTime _startTime;
        private TimeSpan _endTime;

        public TimeSpan RemainingTime => _startTime + _endTime - DateTime.Now;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_prefsIdentifier))
                _prefsIdentifier = Guid.NewGuid().ToString();
        }

        [Inject]
        public void Construct(IPrefsService prefs)
        {
            _prefsService = prefs;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            if (IsRecordNotExists())
            {
                SetupTimer();
                return;
            }

            LoadSavedTime();
        }

        private void LoadSavedTime()
        {
            _startTime = DateTime.FromBinary(long.Parse(_prefsService.GetString(StartTimeKey)));
            _endTime = TimeSpan.FromSeconds(double.Parse(_prefsService.GetString(EndTimeKey)));
        }

        private void SetupTimer()
        {
            const int threeDaysAsSeconds = 259_200;
            const int twoDaysAsSeconds = 172_800;
            _startTime = DateTime.Now;
            
            var randomSeconds = Random.Range(twoDaysAsSeconds, threeDaysAsSeconds);
            _endTime = TimeSpan.FromSeconds(randomSeconds);
            
            _prefsService.SetString(StartTimeKey, _startTime.ToBinary().ToString());
            _prefsService.SetString(EndTimeKey, randomSeconds.ToString());
        }

        private bool IsRecordNotExists() 
            => _prefsService.HasKey(StartTimeKey) == false;

        private string EndTimeKey => $"Timer_EndTime_{_prefsIdentifier}";
        private string StartTimeKey => $"Timer_StartTime_{_prefsIdentifier}";

        private void OnEnable() => StartCoroutine(UpdateTime());

        private IEnumerator UpdateTime()
        {
            while (true)
            {
                if (IsTimeIsUp())
                {
                    SetupTimer(); 
                }

                TimeSpan remainingTime = RemainingTime;
                minutes = remainingTime.Minutes;
                seconds = remainingTime.Seconds;
                hours = (int)remainingTime.TotalHours;
                _field.RefreshString();
                yield return new WaitForSeconds(1);
            }
        }

        private bool IsTimeIsUp() 
            => DateTime.Now >= _startTime + _endTime;
    }
}