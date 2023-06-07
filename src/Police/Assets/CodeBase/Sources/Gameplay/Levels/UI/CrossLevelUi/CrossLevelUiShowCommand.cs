using Gameplay.Levels.Services.LevelsTracking;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class CrossLevelUiShowCommand
    {
        private readonly CrossLevelUi _crossLevelUi;
        private readonly ILevelService _level;

        public CrossLevelUiShowCommand(CrossLevelUi crossLevelUi, ILevelService level)
        {
            _crossLevelUi = crossLevelUi;
            _level = level;
        }

        public void Show()
        {
            var barracksShopActive = _level.Level > 4;
            var upgradeScreenActive = _level.Level > 3;
        
            _crossLevelUi.Show();
            _crossLevelUi.SetUpgradeScreenActive(upgradeScreenActive);
            _crossLevelUi.SetShopAndBarracksActive(barracksShopActive);
            _crossLevelUi.SetPlayButtonActive(true);
        }
    }
}