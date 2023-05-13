using System;
using Services.PrefsService;

namespace Gameplay.Levels.Services.LevelsTracking
{
    public class LevelService : ILevelService
    {
        private readonly IPrefsService _prefsService;
        
        private const string CurrentLevelKey = "CurrentLevel";
        
        public LevelService(IPrefsService prefsService)
        {
            _prefsService = prefsService;
        }

        public int Level => _prefsService.GetInt(CurrentLevelKey);

        public event Action LevelIncremented;
        
        public void IncrementLevel()
        {
            _prefsService.SetInt(CurrentLevelKey, Level + 1);
            LevelIncremented?.Invoke();
        }
    }
}