using Services.SuperPowersService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class SuperPowersUi : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsRoot;
        [SerializeField] private Button _takeSuperUnit;
        [SerializeField] private Button _takeNuke;
        [SerializeField] private Button _close;
        [SerializeField] private GameObject _windowsRoot;
        
        [SerializeField] private SuperPowersWindow _nuke;
        [SerializeField] private SuperPowersWindow _superUnit;
        [SerializeField] private SuperPowersTutorial _tutorial;
        
        [Inject]
        public void Construct(SuperPowers powers)
        {
            _takeSuperUnit.onClick.AddListener(_superUnit.Show);
            _takeNuke.onClick.AddListener(_nuke.Show);
            _close.onClick.AddListener(Close);
            _nuke.Construct(powers, this);
            _superUnit.Construct(powers, this);
            gameObject.SetActive(false);
            Close();
            _tutorial.gameObject.SetActive(false);
        }

        public void ShowWindows()
        {
            _windowsRoot.SetActive(true);
        }

        public void ShowButtons()
        {
            _buttonsRoot.SetActive(true);
        }

        public void HideButtons()
        {
            _buttonsRoot.SetActive(false);
        }

        public void Close()
        {
            _windowsRoot.SetActive(false);
            _nuke.Hide();
            _superUnit.Hide();
        }

        public void ShowSuperUnitTutorial()
        {
            _tutorial.ShowSuperUnit();
        }

        public void ShowNukeTutorial()
        {
            Time.timeScale = 0;
            _tutorial.ShowNukeTutorial();
        }
    }
}