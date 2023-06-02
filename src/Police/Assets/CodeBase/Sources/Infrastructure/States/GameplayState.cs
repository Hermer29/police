﻿using System.Collections;
using Gameplay.Levels;
using Gameplay.Levels.Factory;
using Gameplay.Levels.Services.LevelsTracking;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.Defeated;
using Gameplay.UI;
using Hermer29.Almasury;
using Services;
using Tutorial;
using UnityEngine;
using DiContainer = Zenject.DiContainer;

namespace Infrastructure.States
{
    public class GameplayState : State
    {
        private DiContainer _container;
        private IInputService _inputService;
        private LevelEngine _levelEngine;
        private EnemiesFactory _enemiesFactory;
        private EndGameWindow _endGameWindow;
        private ILevelService _levelService;
        private TutorialEngine _tutorial;
        private IGameFactory _gameFactory;
        private ICoroutineRunner _coroutineRunner;
        private GameplayUI _gameplayUi;
        private AdBlockWindow _adBlockWindow;

        private bool _goingToMenu;

        private void ResolveDependencies()
        {
            _container = AllServices.Get<DiContainer>();
            _coroutineRunner = AllServices.Get<ICoroutineRunner>();
            
            _inputService = _container.Resolve<IInputService>();
            _levelEngine = _container.Resolve<LevelEngine>();
            _enemiesFactory = _container.Resolve<EnemiesFactory>();
            _endGameWindow = _container.Resolve<EndGameWindow>();
            _levelService = _container.Resolve<ILevelService>();
            _gameFactory = _container.Resolve<IGameFactory>();
            _gameplayUi = _container.Resolve<GameplayUI>();
            _adBlockWindow = _container.Resolve<AdBlockWindow>();
        }

        protected override void OnEnter()
        {
            Debug.Log($"{nameof(GameplayState)}.{nameof(OnEnter)} called");
            ResolveDependencies();
            _adBlockWindow.Reposition();
            _gameplayUi.Show();
            _inputService.Enable();

            if (_levelService.Level == 1)
            {
                _tutorial = _gameFactory.CreateTutorial();
                _tutorial.Show();
            }
            
            _levelEngine.ExecuteLevel(_levelService.LocalLevel);
            _coroutineRunner.StartCoroutine(PollGamesEnd());
        }

        private IEnumerator PollGamesEnd()
        {
            while (true)
            {
                if (_levelEngine.IsLost())
                {
                    HandleLoss();
                    break;
                }

                if (_levelEngine.IsWon())
                {
                    HandleWon();
                    break;
                }

                yield return null;
            }
        }

        private void HandleLoss()
        {
            _endGameWindow.ShowLost(_levelService.Level);
            _endGameWindow.Closed += CloseLost;
            RegisterGamesEnd();
        }

        private void HandleWon()
        {
            _levelService.IncrementLevel();
            _endGameWindow.ShowWon(_levelService.Level);
            _endGameWindow.Closed += CloseWon;
            RegisterGamesEnd();
        }

        private void RegisterGamesEnd()
        {
            _inputService.Disable();
            _gameplayUi.Hide();
        }

        private void CloseWon() => _coroutineRunner.StartCoroutine(WaitAndGoToMenuStateWhenWon());

        private void CloseLost() => _coroutineRunner.StartCoroutine(WaitAndGoToMenuStateWhenLost());

        private IEnumerator WaitAndGoToMenuStateWhenWon()
        {
            AllServices.Get<ILevelMediator>().StartNextLevelTimeline();
            yield return new WaitForSeconds(2);
            _goingToMenu = true;
        }
        
        private IEnumerator WaitAndGoToMenuStateWhenLost()
        {
            yield return null;
            _goingToMenu = true;
        }

        protected override void OnExit()
        {
            _goingToMenu = false;
            _adBlockWindow.Reposition();
            _enemiesFactory.FlushEnemies();
        }

        [Transition(typeof(MenuState))]
        public bool TimeToGoToMenu() => _goingToMenu;
    }
}