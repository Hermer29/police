using Gameplay.Levels;
using Gameplay.Levels.AssetManagement;
using Gameplay.Levels.Factory;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.CrossLevelUi;
using Gameplay.Levels.UI.Defeated;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
using Gameplay.UI;
using Helpers;
using Logic.Audio;
using PeopleDraw.AssetManagement;
using PeopleDraw.EnergyConsumption;
using Selection;
using Services;
using Services.Audio;
using Services.UseService;
using UnityEngine;
using Zenject;

namespace Gameplay.Infrastructure
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private CityDefinition _spawners;
        [SerializeField] private LevelMediator _mediator;
        [SerializeField] private BulletFactory _bulletFactory;
        [SerializeField] private ParentsForGeneratedObjects _parents;
        [SerializeField] private CrossLevelCameraAnimation cameraAnimation;
        
        private const string InputServicePath = "Prefabs/InputService";

        public override void InstallBindings()
        {
            Container.Bind<MoneyUi>()
                .FromComponentInNewPrefabResource("Prefabs/UI/Stats")
                .AsSingle();
            
            Container.BindInstance(cameraAnimation)
                .AsSingle();
            
            BindMediator();
            BindCrossLevelUi();
            BindEndGameWindow();
            CreateAndBindGameplayUI();

            Container.BindInstance(_parents)
                .AsSingle();

            BindInputService();

            Container.Bind<IUnitsFxFactory>()
                .To<UnitsFxFactory>()
                .AsSingle();

            Container.Bind<IUnitsFxAssetLoader>()
                .To<UnitsFxAssetLoader>()
                .AsSingle();
            
            Container.Bind<UnitsAssetCache>()
                .ToSelf().AsSingle();

            Container.Bind<BulletFactory>()
                .FromInstance(_bulletFactory)
                .AsSingle();
            
            Container.Bind<Energy>()
                .ToSelf()
                .AsSingle();

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

            Container.Bind<UnitsUsingService>().ToSelf().AsSingle();
            
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle();

            BindEnemyFactory();
        }

        private void CreateAndBindGameplayUI()
        {
            Container.Bind<GameplayUI>()
                .FromComponentInNewPrefabResource("Prefabs/UI/LevelUI")
                .AsSingle();

            Container.Bind<EnergyUi>()
                .FromMethod(context => context.Container.Resolve<GameplayUI>().EnergyUi)
                .AsSingle();
            
            Container.Bind<SelectorUi>()
                .FromMethod(context => context.Container.Resolve<GameplayUI>().SelectorUi)
                .AsSingle();
        }

        private void BindEndGameWindow()
        {
            Container.Bind<EndGameWindow>()
                .FromComponentInNewPrefabResource("Prefabs/UI/EndGameScreen")
                .AsSingle();
        }

        private void BindCrossLevelUi()
        {
            Container.Bind<CrossLevelUi>()
                .FromComponentInNewPrefabResource("Prefabs/UI/CrossLevelUi")
                .AsSingle();
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>()
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