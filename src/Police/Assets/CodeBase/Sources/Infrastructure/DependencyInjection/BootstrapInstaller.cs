using Infrastructure.AssetManagement;
using Infrastructure.Loading;
using Zenject;

namespace Infrastructure.DependencyInjection
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAssetLoader();
            BindFactory();
            CreateLoadingScreen();
            CreateLoadingManager();
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