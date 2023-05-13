using Infrastructure.AssetManagement;
using Infrastructure.Loading;
using Services.PrefsService;
using Zenject;

namespace Infrastructure.DependencyInjection
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindAssetLoader();
            BindFactory();
            CreateLoadingScreen();
            CreateLoadingManager();
            
            Container.Bind<ICoroutineRunner>()
                .FromInstance(this)
                .AsSingle();

            Container.Bind<IPrefsService>()
                .To<PlayerPrefsService>()
                .AsSingle();
        }

        private void CreateLoadingManager()
        {
            Container.Bind<LoadingManager>()
                .ToSelf()
                .FromNewComponentOnRoot()
                .AsSingle();
        }

        private void CreateLoadingScreen()
        {
            Container.Bind<LoadingScreen>()
                .ToSelf()
                .FromComponentInNewPrefabResource(AssetsConstants.LoadingScreenPath)
                .AsSingle();
        }

        private void BindAssetLoader()
        {
            Container.Bind<IAssetLoader>()
                .To<AssetLoader>()
                .AsSingle();
        }

        private void BindFactory()
        {
            Container.Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
        }
    }
}