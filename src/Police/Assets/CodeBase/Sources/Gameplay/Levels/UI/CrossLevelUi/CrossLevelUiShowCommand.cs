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
            _crossLevelUi.Show();
            _crossLevelUi.SetUpgradeScreenActive(false);
            _crossLevelUi.SetPlayButtonActive(true);
            _crossLevelUi.SetShopAndBarracksActive(false);
            
            if (_level.Level > 3)
            {
                _crossLevelUi.SetUpgradeScreenActive(true);
            }
            else if (_level.Level > 4)
            {
                _crossLevelUi.SetShopAndBarracksActive(true);
            }
        }
        
        
    }
}