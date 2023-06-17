using System;
using Gameplay.Levels.Domain;
using Gameplay.Levels.UI.Defeated;
using Services.AdvertisingService;
using Services.MoneyService;
using Services.PrefsService;
using UniRx;
using UnityEngine;

namespace Infrastructure.States
{
    public class EndGameLogic
    {
        private readonly EndGameWindow _endGameWindow;
        private readonly IMoneyService _moneyService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IPrefsService _prefsService;

        private int _rewardForLevel;
        private IDisposable _valueChanging;

        public EndGameLogic(EndGameWindow endGameWindow, IMoneyService moneyService, IAdvertisingService advertisingService,
            IPrefsService prefsService)
        {
            _moneyService = moneyService;
            _advertisingService = advertisingService;
            _prefsService = prefsService;
            _endGameWindow = endGameWindow;
        }

        public int RewardForAdvertising => Mathf.RoundToInt(_endGameWindow.GetModifier() * _rewardForLevel);

        public int TotalEndGamesAppeared
        {
            get => _prefsService.GetInt("TotalEndGamesAppeared");
            set => _prefsService.SetInt("TotalEndGamesAppeared", value);
        }
            
        public event Action Closed;

        public void NotifyWon(int levelServiceLevel)
        {
            _rewardForLevel = RewardCalculator.WinRewardForLevel(levelServiceLevel);
            _endGameWindow.ShowWon(_rewardForLevel);
            StartEndGamePhase();
        }

        public void NotifyLost(int levelServiceLevel)
        {
            Debug.Log($"{nameof(EndGameLogic)}.{nameof(NotifyLost)} called");
            _rewardForLevel = RewardCalculator.WinRewardForLevel(levelServiceLevel);
            _endGameWindow.ShowLost(_rewardForLevel);
            StartEndGamePhase();
        }

        private void OnAcceptedReward()
        {
            _endGameWindow.StopRoulette();
            _advertisingService.ShowRewarded(onClosedAndRewarded: () =>
                {
                    OnClosed();
                    GiveMoney(RewardForAdvertising);
                },
                onError: () =>
                {
                    _endGameWindow.StartRoulette();
                });
        }

        private void GiveMoney(int reward) => _moneyService.AddMoney(reward);

        private void OnDeclinedReward()
        {
            GiveMoney(_rewardForLevel);
            OnClosed();
        }

        private void OnClosed()
        {
            TotalEndGamesAppeared++;
            _endGameWindow.NoThanks.onClick.RemoveListener(OnDeclinedReward);
            _endGameWindow.TakeMoreReward.onClick.RemoveListener(OnAcceptedReward);
            Closed?.Invoke();
            _valueChanging.Dispose();
            _endGameWindow.Hide();
        }

        private void StartEndGamePhase()
        {
            SetupButtons();
            SetupRoulette();
        }

        private void SetupButtons()
        {
            if (TotalEndGamesAppeared == 0)
            {
                _endGameWindow.ShowThatRewardForFree();
                _endGameWindow.TakeMoreReward.onClick.AddListener(OnAcceptedRewardForFree);
                return;
            }

            _endGameWindow.ShowThatRewardForReward();
            _endGameWindow.TakeMoreReward.onClick.AddListener(OnAcceptedReward);
            _endGameWindow.NoThanks.onClick.AddListener(OnDeclinedReward);
        }

        private void SetupRoulette()
        {
            _endGameWindow.StartRoulette();
            _valueChanging = Observable.EveryUpdate()
                .Subscribe(_ => _endGameWindow.ShowRewardedAmount(RewardForAdvertising));
        }

        private void OnAcceptedRewardForFree()
        {
            _endGameWindow.StopRoulette();
            OnClosed();
            GiveMoney(RewardForAdvertising);
        }
    }
}