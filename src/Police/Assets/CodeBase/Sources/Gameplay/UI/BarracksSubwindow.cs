using Barracks;
using Interface;
using UnityEngine;

namespace Gameplay.UI
{
    internal class BarracksSubwindow : MonoBehaviour
    {
        [SerializeField] private BarracksWindow _window;
        
        public void Show()
        {
            _window.Show();
            GetComponent<GameObjectActivationBaker>().SetActive(true);
        }

        public void Hide()
        {
            _window.Hide();
            GetComponent<GameObjectActivationBaker>().SetActive(false);
        }
    }
}