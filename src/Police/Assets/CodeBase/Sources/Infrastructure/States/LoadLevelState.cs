using System.Collections;
using Gameplay.Levels.Services.LevelsTracking;
using Hermer29.Almasury;
using Infrastructure.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class LoadLevelState : State, ILoadingProcessor
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private bool _levelLoaded;
        private DiContainer _container;
        private ILevelService _level;
        private readonly LoadingManager _loading;
        private readonly LoadGameResources _resourcesLoading;

        private bool _justLaunched = true;

        public LoadLevelState(ICoroutineRunner coroutineRunner, DiContainer container, LoadGameResources loadResourcesState)
        {
            _container = container;
            _coroutineRunner = coroutineRunner;
            _level = container.Resolve<ILevelService>();
            _loading = container.Resolve<LoadingManager>();
            _resourcesLoading = loadResourcesState;
        }
        
        public float Progress { set; get; }

        protected override void OnEnter() => _coroutineRunner.StartCoroutine(LevelLoading());

        private IEnumerator LevelLoading()
        {
            if (_justLaunched)
            {
                _loading.RequestLoadingImmediately(this, _resourcesLoading);
            }
            else
            {
                _loading.RequestLoading(this, _resourcesLoading);
            }

            _justLaunched = false;
            AsyncOperation sceneLoading = _level.LoadCurrentLevel();
            
            var sceneChanged = false;
            sceneLoading.completed += _ => sceneChanged = true;
            sceneLoading.allowSceneActivation = true;
            
            while (sceneLoading.progress < 0.9f)
            {
                Debug.Log($"{nameof(LoadLevelState)}.{nameof(LevelLoading)} progress: {Progress}");
                Progress = sceneLoading.progress;
                yield return null;
            }
            Debug.Log($"{nameof(LoadLevelState)}.{nameof(LevelLoading)} loading completed. progress: {Progress}");
            Progress = 1;

            yield return new WaitWhile(() => sceneChanged == false);
                        
            _container = Object.FindObjectOfType<SceneContext>().Container;
            
            AllServices.Bind(_container);
            
            _level.WarpToCurrentLevel();
            _levelLoaded = true;
        }

        protected override void OnExit()
        {
            _levelLoaded = false;
        }

        [Transition(typeof(LoadGameResources))]
        public bool IsLevelLoaded() => _levelLoaded;
    }
}