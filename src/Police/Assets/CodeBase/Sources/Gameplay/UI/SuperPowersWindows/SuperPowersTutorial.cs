using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SuperPowersTutorial : MonoBehaviour
    {
        [SerializeField] private UnityEvent _actionOnSuperUnit;
        [SerializeField] private UnityEvent _actionOnNuke;
        [SerializeField] private UnityEvent _actionOnNukeClicked;
        [SerializeField] private UnityEvent _actionOnSuperUnitClicked;
        [SerializeField] private Button _continuationForSuperUnit;
        [SerializeField] private Button _continuationForNuke;
        
        public void ShowNukeTutorial()
        {
            _actionOnNuke.Invoke();
            _continuationForNuke.onClick.AddListener(NukeClicked);
        }

        private void NukeClicked()
        {
            _continuationForNuke.onClick.RemoveListener(NukeClicked);
            _actionOnNukeClicked.Invoke();
        }

        public void ShowSuperUnit()
        {
            _actionOnSuperUnit.Invoke();
            _continuationForSuperUnit.onClick.AddListener(ClickedSuperUnit);
        }

        private void ClickedSuperUnit()
        {
            _continuationForSuperUnit.onClick.RemoveListener(NukeClicked);
            _actionOnSuperUnitClicked.Invoke();
        }
    }
}