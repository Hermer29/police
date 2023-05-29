using System;
using Infrastructure;
using Services.PrefsService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Gameplay.Levels.Services.LevelsTracking
{
    public class LevelService : ILevelService
    {
        private readonly IPrefsService _prefsService;
        private readonly LevelsMapper _mapper;
        private DiContainer _diContainer;

        private const string CurrentLevelKey = "CurrentLevel";
        
        public LevelService(IPrefsService prefsService, LevelsMapper mapper)
        {
            _mapper = mapper;
            _prefsService = prefsService;
        }

        public int Level => _prefsService.GetInt(CurrentLevelKey, 1);
        public int LocalLevel => _mapper.GetLocalLevel(Level);

        public event Action LevelIncremented;

        public AsyncOperation LoadCurrentLevel()
        {
            var sceneName = _mapper.GetSceneName(Level);
            Debug.Log(
                $"{nameof(LevelService)} Loading level: {Level}, scene name: {sceneName}, local level: {_mapper.GetLocalLevel(Level)}");
            return SceneManager.LoadSceneAsync(sceneName);
        }

        public void WarpToCurrentLevel()
        {
            var crossLevelCameraAnimation = AllServices.Get<DiContainer>().Resolve<CrossLevelCameraAnimation>();
            Debug.Log($"{nameof(LevelService)}.{nameof(WarpToCurrentLevel)} warping current level");
            crossLevelCameraAnimation.ShowCameraForLevel(_mapper.GetLocalLevel(Level));
        }

        public void IncrementLevel()
        {
            _prefsService.SetInt(CurrentLevelKey, Level + 1);
            LevelIncremented?.Invoke();
        }
    }
}