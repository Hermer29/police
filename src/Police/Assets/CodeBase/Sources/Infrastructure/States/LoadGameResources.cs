using System.Collections;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.CrossLevelUi;
using Gameplay.UI;
using Hermer29.Almasury;
using Infrastructure.Loading;
using Infrastructure.Services.UseService;
using Services.UseService;
using UnityEngine;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class LoadGameResources : State, ILoadingProcessor
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private UnitsUsingService _unitsUsingService;
        private UnitsAssetCache _unitsAssetCache;
        private DiContainer _container;
        private CrossLevelUi _crossLevelUi;
        private GameplayUI _gameplayUi;

        private bool _resourcesLoaded;

        public LoadGameResources(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public float Progress { get; set; }

        private void ResolveSceneDependencies()
        {
            _container = AllServices.Get<DiContainer>();
            _unitsAssetCache = _container.Resolve<UnitsAssetCache>();
            _unitsUsingService = _container.Resolve<UnitsUsingService>();
            _crossLevelUi = _container.Resolve<CrossLevelUi>();
            _gameplayUi = _container.Resolve<GameplayUI>();
            AllServices.Bind(_container.Resolve<ILevelMediator>());
        }

        protected override void OnEnter() => _coroutineRunner.StartCoroutine(LoadResources());

        private IEnumerator LoadResources()
        {
            ResolveSceneDependencies();

            _unitsUsingService.PreloadUsedUnits();
            _unitsAssetCache.PreloadAndHoldLinks();
            _crossLevelUi.Initialize();
            _gameplayUi.Hide();
            
            yield return null;

            Debug.Log($"{nameof(LoadGameResources)}.{nameof(LoadResources)} completed");
            Progress = 1;
            _resourcesLoaded = true;
        }

        protected override void OnExit()
        {
            _resourcesLoaded = false;
        }

        [Transition(typeof(MenuState))]
        public bool IsLevelLoaded() => _resourcesLoaded;
    }
}