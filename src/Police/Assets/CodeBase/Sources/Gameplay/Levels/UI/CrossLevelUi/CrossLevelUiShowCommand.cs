using Gameplay.Levels.Services.LevelsTracking;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    public class CrossLevelUiShowCommand
    {
        private readonly CrossLevelUi _crossLevelUi;
        private readonly LevelService _level;

        public CrossLevelUiShowCommand(CrossLevelUi crossLevelUi, LevelService level)
        {
            _crossLevelUi = crossLevelUi;
            _level = level;
        }

        public void Show()
        {
            if (_level.Level < 4)
            {
                
            }
            else if (_level.Level < 3)
            {
                
            }
        }
        
        
    }
}