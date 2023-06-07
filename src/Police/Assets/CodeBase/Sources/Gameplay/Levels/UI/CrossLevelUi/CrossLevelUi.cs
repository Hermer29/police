using Gameplay.Levels.Services.LevelsTracking;
using Gameplay.UI;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Upgrading.UI.CrossLevels;
using Zenject;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class CrossLevelUi : MonoBehaviour
    {
        private GenericWindowsGroup _windowsGroup;
        private ILevelService _levelService;

        [SerializeField] private CrossLevelUpgradePresenter _upgrades;
        [SerializeField] private Button _openShop;
        [SerializeField] private Button _openBarracks;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private LocalizeStringEvent _levelLocalization;
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private TownVisualMapping _townVisualMapping;
        [SerializeField] private Image _currentTown;
        [SerializeField] private Image _nextTown;
        [SerializeField] private LocalizeStringEvent _townName;
        [SerializeField] private Image _nextTownBlock;

        [field: SerializeField] public Button PlayButton { get; private set; }

        public int CurrentLevel { get; private set; }

        [Inject]
        public void Construct(GenericWindowsGroup windowsGroup, ILevelService levelService)
        {
            _levelService = levelService;
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
            Show(_levelService.LocalLevel);
            _windowsGroup.Closed -= OnGenericWindowsGroupClosed;
        }

        public void Initialize() => _upgrades.LoadListWithData();

        public void Show(int level)
        {
            gameObject.SetActive(true);
            _upgrades.Show();
            _levelDisplay.ShowLocalLevel(level);
            
            UpdateLocalization(level);
            _currentTown.sprite = _townVisualMapping.GetCurrentTownSprite(level);
            DrawNextTown(level);
        }

        private void DrawNextTown(int level)
        {
            if (_townVisualMapping.IsNextTownExists(level))
                _nextTown.sprite = _townVisualMapping.GetNextTownSprite(level);
            else
                _nextTownBlock.gameObject.SetActive(false);
        }

        private void UpdateLocalization(int level)
        {
            CurrentLevel = level;
            _levelLocalization.RefreshString();
            _townName.SetEntry(_townVisualMapping.GetCurrentTownLocalizedName(level));
            _townName.RefreshString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _upgrades.Hide();
        }

        public void SetPlayButtonActive(bool state)
        {
            PlayButton.gameObject.SetActive(state);
        }

        public void SetUpgradeScreenActive(bool state)
        {
            if (state)
            {
                _upgrades.Show();
                return;
            }
            _upgrades.Hide();
        }

        public void SetShopAndBarracksActive(bool state)
        {
            _openShop.gameObject.SetActive(state);
            _openBarracks.gameObject.SetActive(state);
        }
    }
}