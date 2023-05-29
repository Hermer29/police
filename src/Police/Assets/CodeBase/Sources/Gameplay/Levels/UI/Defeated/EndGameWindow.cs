using System;
using System.Collections;
using DG.Tweening;
using Monetization;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Levels.UI.Defeated
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private RouletteScreen _roulette;
        [Header("Ribbons")] 
        [SerializeField] private GameObject _looseRibbon;
        [SerializeField] private GameObject _wonRibbon;
        [Header("Tints")]
        [SerializeField] private GameObject _wonTint;
        [SerializeField] private GameObject _looseTint;
        
        [field: SerializeField] public Button NoThanks { get; private set; }
        [field: SerializeField] public Button TakeMoreReward { get; private set; }
        

        public event Action Closed;

        private void ReplayingLevel() => Hide();

        private void Hide()
        {
            gameObject.SetActive(false);
            NoThanks.onClick.RemoveListener(ReplayingLevel);
            TakeMoreReward.onClick.RemoveListener(StopRouletteForReward);
            Closed?.Invoke();
        }

        private void StopRouletteForReward()
        {
            float value = _roulette.StopRoulette();
            int prizeIndex = _roulette.GetIndex(value);
            StartCoroutine(WaitAndGoMenu());
        }

        private IEnumerator WaitAndGoMenu()
        {
            yield return new WaitForSeconds(2f);
            Hide();
        }

        public void ShowLost()
        {
            ShowAsLost();
            OpenEndGameWindow();
        }

        private void OpenEndGameWindow()
        {
            Debug.Log("Opening loose window");
            gameObject.SetActive(true);
            _roulette.StartRoulette();

            TakeMoreReward.onClick.AddListener(StopRouletteForReward);
            NoThanks.onClick.AddListener(ReplayingLevel);
        }

        public void ShowWon()
        {
            ShowAsWon();
            OpenEndGameWindow();
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