using System;
using System.Collections;
using GameBalance;
using Gameplay.Levels.Factory;
using Gameplay.Levels.UI;
using Gameplay.UI;
using Helpers;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Levels
{
    public sealed class LevelEngine
    {
        private readonly CityDefinition _cityDefinition;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ILevelMediator _mediator;
        private readonly EnemiesFactory _factory;

        private bool _lost;
        private bool _won;
        private bool _canceled;

        public LevelEngine(CityDefinition cityDefinition, EnemiesFactory factory,
            ILevelMediator mediator, GameplayUI gameplayUi)
        {
            _cityDefinition = cityDefinition;
            _coroutineRunner = AllServices.Get<ICoroutineRunner>();
            _factory = factory;
            _mediator = mediator;

            gameplayUi.Replay.onClick.AddListener(ReplayRequested);
        }

        public event Action Lost;
        public event Action Won;
        public event Action Canceled;

        private void ReplayRequested()
        {
            _canceled = true;
        }

        public void ExecuteLevel(int localLevel) => CityCoroutine(localLevel);

        private void CityCoroutine(int localLevel)
        {
            int asIndex = localLevel - 1;
            LevelEntry levelObject = _cityDefinition.Levels[asIndex];
            _coroutineRunner.StartCoroutine(LevelCoroutine(levelObject))
                .ContinueWith(OnHandlingEnded);
        }

        private void OnHandlingEnded()
        {
            Debug.Log($"{nameof(LevelEngine)}.{nameof(OnHandlingEnded)} Level handling ended");
            _canceled = false;
        }

        private IEnumerator LevelCoroutine(LevelEntry level)
        {
            bool playerLost = false;
            level.LooseTrigger.Reactivate();
            InitializeLevel(
                level, 
                out LevelEnemiesSpawner spawner, 
                onLost: () => playerLost = true);

            while (LevelCompleted(level) == false && playerLost == false)
            {
                if (_canceled)
                {
                    spawner.Stop();
                    Canceled?.Invoke();
                    yield break;
                }

                yield return null;
            }

            if (playerLost)
            {
                spawner.Stop();
                Lost?.Invoke();
                yield break;
            }
            Won?.Invoke();
        }

        private void InitializeLevel(LevelEntry level, out LevelEnemiesSpawner spawner, Action onLost)
        {
            _mediator.DefineMaxEnergyAndFill(level.EnergyAmount);
            spawner = CreateEnemiesSpawner(level);
            spawner.Start();
            _coroutineRunner.StartCoroutine(WaitForLoose(level.LooseTrigger, onLost));
        }

        private IEnumerator WaitForLoose(LooseTrigger looseTrigger, Action onLose)
        {
            yield return looseTrigger.WaitForTrigger();
            onLose.Invoke();
        }

        private LevelEnemiesSpawner CreateEnemiesSpawner(LevelEntry level)
        {
            return new LevelEnemiesSpawner(
                level: level,
                factory: _factory,
                coroutineRunner: _coroutineRunner,
                levelMediator: _mediator);
        }

        private bool LevelCompleted(LevelEntry rivalData) => _factory.EnemiesDeadAmount == rivalData.EnemiesAmount();
    }
}