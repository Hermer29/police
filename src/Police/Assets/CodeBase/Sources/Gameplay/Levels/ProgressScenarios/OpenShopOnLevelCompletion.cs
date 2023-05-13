using Gameplay.Levels.Services.LevelsTracking;

namespace ProgressScenarios
{
    public class OpenShopOnLevelCompletion
    {
        private readonly ILevelService _levelService;

        private const int LevelToOpenShop = 5;

        public OpenShopOnLevelCompletion(ILevelService levelService)
        {
            _levelService = levelService;
            
            levelService.LevelIncremented += LevelServiceOnLevelIncremented;
        }

        private void LevelServiceOnLevelIncremented()
        {
            if (_levelService.Level == LevelToOpenShop)
            {
                
            }
        }
    }
}