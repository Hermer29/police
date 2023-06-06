using Barracks;
using Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.UI.CrossLevels;
using Zenject;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class CrossLevelUi : MonoBehaviour
    {
        private GenericWindowsGroup _windowsGroup;
        
        [SerializeField] private CrossLevelUpgradePresenter _upgrades;
        [SerializeField] private Button _openShop;
        [SerializeField] private Button _openBarracks;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private SettingsWindow _settingsWindow;
       
        [field: SerializeField] public Button PlayButton { get; private set; }

        [Inject]
        public void Construct(GenericWindowsGroup windowsGroup)
        {
            _windowsGroup = windowsGroup;
            _openShop.onClick.AddListener(ShowShop);
            _openBarracks.onClick.AddListener(ShowBarracks);
            _settingsButton.onClick.AddListener(ShowSettings);
        }

        private void ShowSettings() => _settingsWindow.Show();

        private void ShowBarracks()
        {
            _windowsGroup.ShowBarracks();
            ShowGenericWindow();
        }

        private void ShowShop()
        {
            _windowsGroup.ShowShop();
            ShowGenericWindow();
        }

        private void ShowGenericWindow()
        {
            Hide();
            _windowsGroup.Closed += OnGenericWindowsGroupClosed;
        }

        private void OnGenericWindowsGroupClosed()
        {
            Show();
            _windowsGroup.Closed -= OnGenericWindowsGroupClosed;
        }

        public void Initialize()
        {
            _upgrades.LoadListWithData();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _upgrades.Show();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _upgrades.Hide();
        }
    }
}