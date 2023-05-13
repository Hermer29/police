using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Levels.UI.Defeated
{
    public class DefeatedWindow : MonoBehaviour
    {
        [SerializeField] private Button _replay;

        [Inject]
        public void Construct()
        {
            _replay.onClick.AddListener(ReplayingLevel);
        }

        public event Action ContinuePlaying;

        private void Start()
        {
            Hide();
        }

        private void ReplayingLevel()
        {
            Hide();
            ContinuePlaying?.Invoke();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            Debug.Log("Opening loose window");
            gameObject.SetActive(true);
        }
    }
}