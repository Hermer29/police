using System.Collections;
using DefaultNamespace;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.CrossLevelUi;
using Gameplay.PeopleDraw.Factory;
using Gameplay.UI;
using Helpers;
using Hermer29.Almasury;
using Infrastructure.Loading;
using Infrastructure.Services.UseService;
using Services.UseService;
using UnityEngine;
using Upgrading;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class LoadGameResources : State, ILoadingProcessor
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private UsedUnitsService _usedUnitsService;
        private UnitsAssetCache _unitsAssetCache;
        private DiContainer _container;
        private CrossLevelUi _crossLevelUi;
        private GameplayUI _gameplayUi;
        private NewUnitWindow _newUnitWindow;
        private IAlliedUnitsFactory _alliedUnitsFactory;
        private UnitsRepository _repository;
        
        private bool _resourcesLoaded;

        public LoadGameResources(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public float Progress { get; set; }

        private void ResolveSceneDependencies()
        {
            _container = AllServices.Get<DiContainer>();
            _repository = _container.Resolve<UnitsRepository>();
            _unitsAssetCache = _container.Resolve<UnitsAssetCache>();
            _usedUnitsService = _container.Resolve<UsedUnitsService>();
            _crossLevelUi = _container.Resolve<CrossLevelUi>();
            _gameplayUi = _container.Resolve<GameplayUI>();
            _newUnitWindow = _container.Resolve<NewUnitWindow>();
            _alliedUnitsFactory = _container.Resolve<IAlliedUnitsFactory>();
            AllServices.Bind(_container.Resolve<ILevelMediator>());
        }

        protected override void OnEnter() => _coroutineRunner.StartCoroutine(LoadResources());

        private IEnumerator LoadResources()
        {
            ResolveSceneDependencies();

            InitializeTimeSensitiveServices();
            _crossLevelUi.Initialize();
            _gameplayUi.Hide();
            
            yield return null;

            Debug.Log($"{nameof(LoadGameResources)}.{nameof(LoadResources)} completed");
            Progress = 1;
            _resourcesLoaded = true;
        }

        private void InitializeTimeSensitiveServices()
        {
            var initializing = new IManuallyInitializable[]
            {
                _repository,
                _usedUnitsService,
                _unitsAssetCache,
                _alliedUnitsFactory
            };

            foreach (IManuallyInitializable manuallyInitializable in initializing)
            {
                manuallyInitializable.Initialize();
            }
        }

        protected override void OnExit()
        {
            _resourcesLoaded = false;
        }

        [Transition(typeof(MenuState))]
        public bool IsLevelLoaded() => _resourcesLoaded;
    }
}