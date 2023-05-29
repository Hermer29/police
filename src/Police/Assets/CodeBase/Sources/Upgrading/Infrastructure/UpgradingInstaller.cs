using SpecialPlatforms;
using Upgrading.AssetManagement;
using Upgrading.Factory;
using Upgrading.Services.CostEstimation;
using Zenject;

namespace Upgrading.Infrastructure
{
    public class UpgradingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAssetLoader();
            BindFactory();
            BindUpgradableUnitsRepository();
            BindUnitsUpgrades();
            BindUpgradeCostService();
        }

        private void BindUpgradeCostService()
        {
            Container.Bind<IUpgradeCostService>()
                .To<UpgradeCostService>()
                .AsSingle();
        }

        private void BindAssetLoader()
        {
            Container.Bind<IUnitsAssetLoader>()
                .To<UnitsAssetLoader>()
                .AsSingle();
        }

        private void BindFactory()
        {
            Container.Bind<IUpgradableUnitsFactory>()
                .To<UpgradableUnitsUiFactory>()
                .AsSingle();
        }

        private void BindUpgradableUnitsRepository()
        {
            Container.Bind<UnitsRepository>()
                .ToSelf()
                .AsSingle();
        }

        private void BindUnitsUpgrades()
        {
            Container.Bind<UnitsUpgrades>()
                .ToSelf()
                .AsSingle();
        }
    }
}