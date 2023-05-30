using System;
using System.Collections;
using System.Globalization;
using DG.Tweening;
using Gameplay.Levels.Domain;
using MathUtility;
using Monetization;
using Services.MoneyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.Services.CostEstimation;
using Zenject;

namespace Gameplay.Levels.UI.Defeated
{
    public class EndGameWindow : MonoBehaviour
    {
        private IMoneyService _moneyService;
        
        [SerializeField] private RouletteScreen _roulette;
        [SerializeField] private TMP_Text _rewardedAmount;
        [SerializeField] private TMP_Text _rewardForLevel;
        [Header("Ribbons")] 
        [SerializeField] private GameObject _looseRibbon;
        [SerializeField] private GameObject _wonRibbon;
        [Header("Tints")]
        [SerializeField] private GameObject _wonTint;
        [SerializeField] private GameObject _looseTint;
        [Header("Prizes")] 
        [SerializeField] private float[] _modifiers;

        private bool _shown;
        private int _currentLevel;

        [field: SerializeField] public Button NoThanks { get; private set; }
        [field: SerializeField] public Button TakeMoreReward { get; private set; }
        private int RewardForLevel => RewardCalculator.RewardForLevel(_currentLevel);
        private int RouletteReward => Mathf.RoundToInt(RewardForLevel * _modifiers[_roulette.GetPrizeIndex()]);
        
        public event Action Closed;

        [Inject]
        public void Construct(IMoneyService moneyService)
        {
            _moneyService = moneyService;
        }
        
        private void ReplayingLevel()
        {
            _moneyService.AddMoney(RewardForLevel);
            Hide();
        }


        private void Hide()
        {
            _shown = false;
            gameObject.SetActive(false);
            NoThanks.onClick.RemoveListener(ReplayingLevel);
            TakeMoreReward.onClick.RemoveListener(StopRouletteForReward);
            Closed?.Invoke();
        }

        private void StopRouletteForReward()
        {
            _roulette.StopRoulette();
            _moneyService.AddMoney(RouletteReward);
            
            StartCoroutine(WaitAndGoMenu());
        }

        private void Update()
        {
            if (_shown == false)
                return;

            ShowRewardedAmount(RouletteReward);
        }

        private void ShowRewardedAmount(float modifier) => _rewardedAmount.text = Mathf.RoundToInt(modifier).ToString(CultureInfo.CurrentCulture);

        private void ShowRewardOnUp(int reward) => _rewardForLevel.text = $"+{reward}";

        private IEnumerator WaitAndGoMenu()
        {
            yield return new WaitForSeconds(2f);
            Hide();
        }

        public void ShowLost(int currentLevel)
        {
            ShowAsLost();
            OpenEndGameWindow(currentLevel);
        }

        private void OpenEndGameWindow(int currentLevel)
        {
            Debug.Log("Opening loose window");
            _shown = true;
            gameObject.SetActive(true);
            _roulette.StartRoulette();
            _currentLevel = currentLevel;
            TakeMoreReward.onClick.AddListener(StopRouletteForReward);
            NoThanks.onClick.AddListener(ReplayingLevel);
            ShowRewardOnUp(RewardCalculator.RewardForLevel(currentLevel));
        }

        public void ShowWon(int currentLevel)
        {
            ShowAsWon();
            OpenEndGameWindow(currentLevel);
        }

        private void ShowAsLost() => SetIsWon(false);

        private void ShowAsWon() => SetIsWon(true);

        private void SetIsWon(bool isWon)
        {
            _wonRibbon.SetActive(isWon);
            _looseRibbon.SetActive(!isWon);
            _wonTint.SetActive(isWon);
            _looseTint.SetActive(!isWon);
        }
    }
}