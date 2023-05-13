using Gameplay.Levels;
using Gameplay.Levels.AssetManagement;
using Gameplay.Levels.Factory;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.Defeated;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
using Gameplay.PeopleDraw.Selection;
using PeopleDraw.EnergyConsumption;
using Services;
using UnityEngine;
using Zenject;

namespace Infrastructure.DependencyInjection
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private CityDefinition _spawners;
        [SerializeField] private LevelMediator _mediator;
        [SerializeField] private SelectorUi _selector;
        [SerializeField] private EnergyUi _energyUi;
        [SerializeField] private BulletFactory _bulletFactory;
        [SerializeField] private DefeatedWindow _defeatedWindow;

        private const string InputServicePath = "Prefabs/InputService";

        public override void InstallBindings()
        {
            BindInputService();

            Container.Bind<BulletFactory>()
                .FromInstance(_bulletFactory)
                .AsSingle();
            
            Container.Bind<Energy>()
                .ToSelf()
                .AsSingle();

            Container.Bind<DefeatedWindow>()
                .FromInstance(_defeatedWindow)
                .AsSingle();
            
            Container.BindInstance(_energyUi).AsSingle();
            Container.BindInstance(_selector).AsSingle();
            
            Container.Bind<CityDefinition>()
                .ToSelf()
                .FromInstance(_spawners)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LevelEngine>()
                .AsSingle()
                .NonLazy();

            Container.Bind<CharactersAssetLoader>()
                .ToSelf()
                .AsSingle();

            Container.Bind<EnergyKillRestoration>()
                .ToSelf()
                .AsSingle();

            BindEnemyFactory();
            BindMediator();
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>()
                .To<InputService>()
                .FromComponentInNewPrefabResource(InputServicePath)
                .AsSingle();
        }

        private void BindEnemyFactory()
        {
            Container.Bind<EnemiesFactory>()
                .ToSelf()
                .AsSingle();
        }

        private void BindMediator()
        {
            Container.BindInstance<ILevelMediator>(_mediator)
                .AsSingle();
        }
    }
}