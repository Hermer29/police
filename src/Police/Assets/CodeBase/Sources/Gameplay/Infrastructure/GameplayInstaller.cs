using DefaultNamespace;
using DefaultNamespace.Audio;
using DefaultNamespace.Gameplay.ActiveCharacters;
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
using Infrastructure.Services;
using Infrastructure.Services.UseService;
using Infrastructure.Services.YansMoneyExchange;
using Infrastructure.States;
using InputServices;
using Logic.Audio;
using Monetization.AdvertisingPlane;
using PeopleDraw.AssetManagement;
using PeopleDraw.EnergyConsumption;
using Selection;
using Services;
using Services.Audio;
using Services.PropertyAcquisition;
using Services.SuperPowersService;
using Services.UseService;
using Shop;
using UI;
using UI.AssetLoader;
using UI.Factory;
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
            Container.Bind<BalanceProvider>()
                .ToSelf()
                .AsSingle();

            Container.Bind<GlobalAudio>()
                .ToSelf()
                .FromComponentInNewPrefabResource("Prefabs/GlobalAudio")
                .AsSingle();
            
            Container.Bind<SaveService>()
                .ToSelf()
                .AsSingle();

            BindSuperPowers();
            
            Container.Bind<GameAssetLoader>()
                .ToSelf()
                .AsSingle();
            
            Container.Bind<GenericWindowsGroup>()
                .FromComponentInNewPrefabResource("Prefabs/UI/UITypeB")
                .AsSingle();

            Container.Bind<Arrow>()
                .WithId("NextWaveArrow")
                .FromComponentInNewPrefabResource("Prefabs/UI/NextWaveArrow")
                .AsSingle();

            Container.Bind<MoneyUi>()
                .FromComponentInNewPrefabResource("Prefabs/UI/Stats")
                .AsSingle();

            Container.BindInstance(cameraAnimation)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<AdBlockWindow>()
                .FromComponentInNewPrefabResource("Prefabs/UI/AdBlock")
                .AsSingle()
                .NonLazy();

            Container.Bind<EndGameLogic>()
                .ToSelf()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<EnergyKillRestorationView>()
                .FromComponentInNewPrefabResource("Prefabs/UI/EnergyKillRestorationView")
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<NewUnitWindow>()
                .FromComponentInNewPrefabResource("Prefabs/UI/NewUnitWindow")
                .AsSingle().NonLazy();

            Container.Bind<RaycastInputService>()
                .FromNewComponentOn(gameObject)
                .AsSingle();

            AddAdPlane();

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

            Container.Bind<UsedUnitsService>().ToSelf().AsSingle();
            
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle();

            Container.Bind<UiElementsFactory>()
                .ToSelf()
                .AsSingle();

            Container.Bind<UiAssetLoader>()
                .ToSelf()
                .AsSingle();

            Container.Bind<PropertyService>()
                .ToSelf()
                .AsSingle();

            BindEconomy();
            BindEnemyFactory();
        }

        private void BindEconomy()
        {
            Container.Bind<YansMoneyService>()
                .ToSelf()
                .AsSingle();

            Container.Bind<ConsumablesShopService>()
                .ToSelf()
                .AsSingle();
        }

        private void BindSuperPowers()
        {
            Container.Bind<SuperPowersUi>()
                .FromComponentInNewPrefabResource("Prefabs/UI/SuperPowersWindow")
                .AsSingle();

            Container.Bind<Nuke>()
                .ToSelf().AsSingle();

            Container.Bind<SuperUnit>()
                .ToSelf().AsSingle();

            Container.Bind<SuperPowers>()
                .ToSelf()
                .AsSingle();
            
        }

        private void AddAdPlane()
        {
            Container.Bind<AdPlane>()
                .FromComponentInNewPrefabResource("Prefabs/AdPlane")
                .AsSingle();

            Container.Bind<SuggestAdvertisingWindow>()
                .FromComponentInNewPrefabResource("Prefabs/UI/SuggestAdvertisingWindow")
                .AsSingle();
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