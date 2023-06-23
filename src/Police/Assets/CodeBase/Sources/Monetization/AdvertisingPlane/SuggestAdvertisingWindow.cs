using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monetization.AdvertisingPlane
{
    public class SuggestAdvertisingWindow : MonoBehaviour
    {
        [SerializeField] private Button _accept;
        [SerializeField] private Button _decline;
        [SerializeField] private TMP_Text _rewardedEnergyAmount;
        [SerializeField] private GameObject _bg;
        [SerializeField] private PlaneWindow[] _windowTypes;
        
        private Action _onAccept;
        private Action _onDecline;

        private void Start()
        {
            OnEnd();
        }

        public void Show(Action onAccept, Action onDecline, AdvertisingType advertisingType)
        {
            _bg.SetActive(true);
            OpenWindow(advertisingType);
            _onAccept = onAccept;
            _onDecline = onDecline;
            _accept.onClick.AddListener(OnAccept);
            _decline.onClick.AddListener(OnDecline);
        }

        public void ShowEnergyAdvertising(Action onAccept, int givenAmount, Action onDecline = null)
        {
            _rewardedEnergyAmount.text = givenAmount.ToString();
            Show(onAccept, onDecline, AdvertisingType.Energy);
        }

        private void OpenWindow(AdvertisingType advertisingType) 
            => _windowTypes.First(x => x.Type == advertisingType).Window.SetActive(true);

        private void OnAccept()
        {
            _onAccept?.Invoke();
            OnEnd();
        }

        private void OnEnd()
        {
            _bg.SetActive(false);
            _accept.onClick.RemoveAllListeners();
            _decline.onClick.RemoveAllListeners();
            foreach (PlaneWindow planeWindow in _windowTypes)
                planeWindow.Window.SetActive(false);
        }

        private void OnDecline()
        {
            _onDecline?.Invoke();
            OnEnd();
        }
    }

    public enum AdvertisingType
    {
        Energy
    }

    [Serializable]
    public class PlaneWindow
    {
        public AdvertisingType Type;
        public GameObject Window;
    }
}