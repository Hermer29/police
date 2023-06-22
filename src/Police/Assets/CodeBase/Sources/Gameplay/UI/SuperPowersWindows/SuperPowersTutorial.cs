using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SuperPowersTutorial : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tintForSuperUnit;
        [SerializeField] private MonoBehaviour[] _componentForSuperUnit;
        [SerializeField] private GameObject[] _tintForNuke;
        [SerializeField] private MonoBehaviour[] _componentsForNuke;
        [SerializeField] private Button _continuationForSuperUnit;
        [SerializeField] private Button _continuationForNuke;
        
        public void ShowNukeTutorial()
        {
            gameObject.SetActive(true);
            foreach (GameObject o in _tintForNuke)
            {
                o.SetActive(true);
            }
            foreach (MonoBehaviour o in _componentsForNuke)
            {
                o.enabled = true;
            }
            _continuationForNuke.gameObject.SetActive(true);
            _continuationForNuke.onClick.AddListener(NukeClicked);
        }

        private void NukeClicked()
        {
            gameObject.SetActive(false);
            _continuationForNuke.onClick.RemoveListener(NukeClicked);
            foreach (GameObject o in _tintForNuke)
            {
                o.SetActive(false);
            }

            foreach (MonoBehaviour o in _componentsForNuke)
            {
                o.enabled = false;
            }
        }

        public void ShowSuperUnit()
        {
            gameObject.SetActive(true);
            foreach (GameObject o in _tintForSuperUnit)
            {
                o.SetActive(true);
            }
            foreach (MonoBehaviour o in _componentForSuperUnit)
            {
                o.enabled = true;
            }
            _continuationForNuke.gameObject.SetActive(false);
            _continuationForSuperUnit.onClick.AddListener(ClickedSuperUnit);
        }

        private void ClickedSuperUnit()
        {
            gameObject.SetActive(false);
            foreach (GameObject o in _tintForSuperUnit)
            {
                o.SetActive(false);
            }

            foreach (MonoBehaviour o in _componentForSuperUnit)
            {
                o.enabled = false;
            }
            _continuationForSuperUnit.onClick.RemoveListener(ClickedSuperUnit);
        }
    }
}