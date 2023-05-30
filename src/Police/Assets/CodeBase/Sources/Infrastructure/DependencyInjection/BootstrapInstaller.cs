using Gameplay.Levels;
using Gameplay.Levels.Services.LevelsTracking;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Loading;
using Services.MoneyService;
using Services.PrefsService;
using UnityEngine;
using Zenject;

namespace Infrastructure.DependencyInjection
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private LevelsMapper _levelsMapper; 
        
        public override void InstallBindings()
        {
            BindLevelsClasses();
            BindAssetLoader();
            BindFactory();
            BindLoading();
            BindCoroutineRunner();
            BindPrefsService();
            BindMoneyService();
        }

        private void BindLevelsClasses()
        {
            BindLevelsMapper();
            BindLevelService();
        }

        private void BindLoading()
        {
            CreateLoadingScreen();
            CreateLoadingManager();
        }

        private void BindLevelsMapper() =>
            Container.BindInstance(_levelsMapper)
                .AsSingle();

        private void BindMoneyService() =>
            Container.Bind<IMoneyService>()
                .To<MoneyService>()
                .AsSingle();

        private void BindCoroutineRunner() =>
            Container.Bind<ICoroutineRunner>()
                .FromInstance(this)
                .AsSingle();

        private void BindPrefsService() =>
            Container.Bind<IPrefsService>()
                .To<PlayerPrefsService>()
                .AsSingle();

        private void BindLevelService() =>
            Container.Bind<ILevelService>()
                .To<LevelService>()
                .AsSingle();

        private void CreateLoadingManager() =>
            Container.Bind<LoadingManager>()
                .ToSelf()
                .FromNewComponentOn(gameObject)
                .AsSingle();

        private void CreateLoadingScreen() =>
            Container.Bind<LoadingScreen>()
                .ToSelf()
                .FromComponentInNewPrefabResource(AssetsConstants.LoadingScreenPath)
                .AsSingle();

        private void BindAssetLoader() =>
            Container.Bind<IAssetLoader>()
                .To<AssetLoader>()
                .AsSingle();

        private void BindFactory() =>
            Container.Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
    }
}