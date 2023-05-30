using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GenericWindowsGroup : MonoBehaviour
    {
        [SerializeField] private BarracksSubwindow _barracks;
        [SerializeField] private ShopSubwindow _shop;
        [SerializeField] private Button _close;

        public event Action Closed;

        public void ShowBarracks()
        {
            _barracks.Show();
            gameObject.SetActive(true);
            _close.onClick.AddListener(HideBarracks);
        }

        private void HideBarracks()
        {
            _barracks.Hide();
            gameObject.SetActive(false);
            _close.onClick.RemoveListener(HideBarracks);
            Closed?.Invoke();
        }

        public void ShowShop()
        {
            _shop.Shop();
            gameObject.SetActive(true);
            _close.onClick.AddListener(HideShop);
        }

        private void HideShop()
        {
            _shop.Hide();
            gameObject.SetActive(false);
            _close.onClick.RemoveListener(HideShop);
            Closed?.Invoke();
        }

        public void Hide() => gameObject.SetActive(false);
    }
}