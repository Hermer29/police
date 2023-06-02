using System;
using System.Collections;
using System.Globalization;
using Gameplay.Levels.Domain;
using Monetization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Levels.UI.Defeated
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _filmSprite;
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

        [field: SerializeField] public Button NoThanks { get; private set; }
        [field: SerializeField] public Button TakeMoreReward { get; private set; }

        public void Hide()
        {
            _shown = false;
            gameObject.SetActive(false);
        }

        public void StopRoulette()
        {
            _roulette.Stop();
        }

        public float GetModifier() => _modifiers[_roulette.GetPrizeIndex()];

        public void ShowRewardedAmount(float reward) => _rewardedAmount.text = Mathf.RoundToInt(reward).ToString(CultureInfo.CurrentCulture);

        private void ShowRewardOnUp(int reward) => _rewardForLevel.text = $"+{reward}";

        public void StartRoulette() => _roulette.StartRoulette();

        public void ShowLost(int currentLevel)
        {
            ShowAsLost();
            OpenEndGameWindow(currentLevel);
        }

        private void OpenEndGameWindow(int currentLevel)
        {
            Debug.Log("Opening loose window");
            _filmSprite.SetActive(true);
            _shown = true;
            gameObject.SetActive(true);
            ShowRewardOnUp(RewardCalculator.WinRewardForLevel(currentLevel));
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

        public void ShowThatRewardForFree() => SetActiveIsRewardIsFree(true);

        public void ShowThatRewardForReward() => SetActiveIsRewardIsFree(false);

        private void SetActiveIsRewardIsFree(bool active)
        {
            NoThanks.gameObject.SetActive(!active);
            _filmSprite.SetActive(!active);
        }
    }
}