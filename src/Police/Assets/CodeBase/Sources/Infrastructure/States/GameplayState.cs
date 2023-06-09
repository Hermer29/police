﻿using System.Collections;
using System.Linq;
using DefaultNamespace.Audio;
using Gameplay.Levels;
using Gameplay.Levels.Factory;
using Gameplay.Levels.Services.LevelsTracking;
using Gameplay.Levels.UI;
using Gameplay.PeopleDraw.Factory;
using Gameplay.UI;
using Hermer29.Almasury;
using Infrastructure.Services;
using Infrastructure.Services.UseService;
using Monetization.AdvertisingPlane;
using PeopleDraw.Selection;
using Services;
using Services.AdvertisingService;
using Services.PrefsService;
using Services.PropertyAcquisition;
using Services.PurchasesService.PurchasesWrapper;
using Tutorial;
using UnityEngine;
using Upgrading.UnitTypes;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class GameplayState : State
    {
        private DiContainer _container;
        private IInputService _inputService;
        private LevelEngine _levelEngine;
        private EnemiesFactory _enemiesFactory;
        private ILevelService _levelService;
        private TutorialEngine _tutorial;
        private IGameFactory _gameFactory;
        private ICoroutineRunner _coroutineRunner;
        private GameplayUI _gameplayUi;
        private AdBlockWindow _adBlockWindow;
        private EndGameLogic _endGameLogic;
        private IAdvertisingService _advertising;
        private SelectedUnits _selection;
        private AdPlane _adPlane;
        private BulletFactory _bulletsFactory;
        private SuperPowersUi _superPowersUi;
        private UsedUnitsService _service;
        private GlobalAudio _audio;
        private PropertyService _consumables;
        private AlmostLostDetector _almostLostDetector;

        private bool _goingToMenu;
        private bool _warpToNextTown;
        private bool _justCompletedLastTownLevel;

        private void ResolveDependencies()
        {
            _container = AllServices.Get<DiContainer>();
            _coroutineRunner = AllServices.Get<ICoroutineRunner>();
            
            _inputService = _container.Resolve<IInputService>();
            _levelEngine = _container.Resolve<LevelEngine>();
            _enemiesFactory = _container.Resolve<EnemiesFactory>();
            _levelService = _container.Resolve<ILevelService>();
            _gameFactory = _container.Resolve<IGameFactory>();
            _gameplayUi = _container.Resolve<GameplayUI>();
            _adBlockWindow = _container.Resolve<AdBlockWindow>();
            _endGameLogic = _container.Resolve<EndGameLogic>();
            _advertising = _container.Resolve<IAdvertisingService>();
            _selection = _container.Resolve<SelectedUnits>();
            _adPlane = _container.Resolve<AdPlane>();
            _bulletsFactory = _container.Resolve<BulletFactory>();
            _superPowersUi = _container.Resolve<SuperPowersUi>();
            _service = _container.Resolve<UsedUnitsService>();
            _audio = _container.Resolve<GlobalAudio>();
            _consumables = _container.Resolve<PropertyService>();
            _almostLostDetector = _container.Resolve<AlmostLostDetector>();
        }

        protected override void OnEnter()
        {
            Debug.Log($"{nameof(GameplayState)}.{nameof(OnEnter)} called");
            ResolveDependencies();
            AllServices.Get<IPrefsService>().SetInt("NotFirstStart", 1);
            ExecuteAction();
        }

        private void ExecuteAction()
        {
            AllServices.Bind(_levelEngine.CurrentTrigger(_levelService.LocalLevel));
            _almostLostDetector.StartWaitingForMoment();
            _selection.SelectUnit(UnitType.Barrier);
            _advertising.ShowInterstitial();
            _adBlockWindow.Reposition();
            _gameplayUi.SetIcons(_service.UsedUnits.Select(
                x => x.Value.CurrentIcon.Value).ToArray());

            if (_levelService.Level > 7)
            {
                _superPowersUi.ShowButtons();
            }
            else
            {
                _superPowersUi.gameObject.SetActive(false);
            }

            ProcessTutorial();

            if (TutorialRequired())
            {
                _tutorial = _gameFactory.CreateTutorial();
                _tutorial.Show();
            }

            Debug.Log($"Running local level: {_levelService.LocalLevel}");
            _levelEngine.ExecuteLevel(_levelService.LocalLevel);
            PollGamesEnd();
        }

        private void ProcessTutorial()
        {
            if (SuperPowersTutorialRequired())
            {
                _consumables.Own(Property.Nuke);
                _consumables.Own(Property.SuperUnit);
                ShowSuperPowersTutorial();
                _levelEngine.ModifyDifficulty(10);
            }
            else
            {
                _adPlane.StartProcessing();
            }

            _inputService.Enable();
        }

        private void ShowSuperPowersTutorial()
        {
            _superPowersUi.ShowSuperUnitTutorial();
            
            _almostLostDetector.NearlyLost += _superPowersUi.ShowNukeTutorial;
        }

        private bool SuperPowersTutorialRequired() => _levelService.Level == 8;

        private bool TutorialRequired() => _levelService.Level == 1;

        private void PollGamesEnd()
        {
            _levelEngine.Lost += HandleLoss;
            _levelEngine.Won += HandleWon;
            _levelEngine.Canceled += HandleCancel;
        }

        private void HandleCancel()
        {
            PreGameEnd();
            Debug.Log($"{nameof(GameplayState)}.{nameof(PollGamesEnd)} Detected cancel");
            RegisterGamesEnd();
            _goingToMenu = true;
        }

        private void HandleLoss()
        {
            PreGameEnd();
            Debug.Log($"{nameof(GameplayState)}.{nameof(PollGamesEnd)} Detected loose");
            _endGameLogic.NotifyLost(_levelService.Level);
            _endGameLogic.Closed += CloseLost;
            _advertising.ShowInterstitial();
            _audio.PlayLoose();
            RegisterGamesEnd();
        }

        private void PreGameEnd()
        {
            _superPowersUi.CloseIfOpened();
            _superPowersUi.HideButtons();
        }
        
        private void HandleWon()
        {
            PreGameEnd();
            Debug.Log($"{nameof(GameplayState)}.{nameof(PollGamesEnd)} Detected win");
            var mediator = AllServices.Get<ILevelMediator>();
            mediator.AlliedUnitsToVictory();
            if (_levelService.LocalLevel == 10)
            {
                _justCompletedLastTownLevel = true;
            }
            _audio.PlayWin();
            
            _coroutineRunner.StartCoroutine(WaitForWinProcess());
        }

        private IEnumerator WaitForWinProcess()
        {
            yield return new WaitForSeconds(2);
            _endGameLogic.NotifyWon(_levelService.Level);
            _endGameLogic.Closed += CloseWon;
            _levelService.IncrementLevel();
            _advertising.ShowInterstitial();
            RegisterGamesEnd();
        }
        
        private void RegisterGamesEnd()
        {
            _superPowersUi.HideButtons();
            _bulletsFactory.FreeAll();
            if (TutorialRequired())
            {
                _tutorial.Disable();
            }
            _inputService.Disable();
            _gameplayUi.Hide();
        }

        private void CloseWon()
        {
            _endGameLogic.Closed -= CloseWon;
            _coroutineRunner.StartCoroutine(WaitAndGoToMenuStateWhenWon());
        }

        private void CloseLost()
        {
            _endGameLogic.Closed -= CloseLost;
            _coroutineRunner.StartCoroutine(WaitAndGoToMenuStateWhenLost());
        }

        private IEnumerator WaitAndGoToMenuStateWhenWon()
        {
            if (_justCompletedLastTownLevel)
            {
                _warpToNextTown = true;
                yield break;
            }

            var mediator = AllServices.Get<ILevelMediator>();
            mediator.StartNextLevelTimeline();
            _goingToMenu = true;
            mediator.DestroyOldPeople();
        }
        
        private IEnumerator WaitAndGoToMenuStateWhenLost()
        {
            Debug.Log($"{nameof(GameplayState)}.{nameof(WaitAndGoToMenuStateWhenLost)} called");

            yield return null;
            _goingToMenu = true;
        }

        protected override void OnExit()
        {
            _justCompletedLastTownLevel = false;
            _goingToMenu = false;
            _warpToNextTown = false;
            _adBlockWindow.Reposition();
            _levelEngine.Lost -= HandleLoss;
            _levelEngine.Won -= HandleWon;
            _levelEngine.Canceled -= HandleCancel;
            _enemiesFactory.FlushEnemies();
            _adPlane.StopProcessing();
            _almostLostDetector.Uninitialize();
            AllServices.Get<ILevelMediator>().DestroyOldPeople();
            if (_levelService.Level == 9)
            {
                _almostLostDetector.NearlyLost -= _superPowersUi.ShowNukeTutorial;
            }
        }

        [Transition(typeof(MenuState))]
        public bool TimeToGoToMenu() 
            => _goingToMenu;

        [Transition(typeof(LoadLevelState))]
        public bool WarpToNextTownState() 
            => _warpToNextTown;
    }
}