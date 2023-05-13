using System;
using System.Collections;
using GameBalance;
using Gameplay.Levels.Factory;
using Gameplay.Levels.UI;
using Gameplay.Levels.UI.Defeated;
using Infrastructure;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels
{
    public sealed class LevelEngine : IInitializable
    {
        private readonly CityDefinition _cityDefinition;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly EnemiesFactory _factory;
        private readonly ILevelMediator _mediator;
        private readonly Energy _energy;
        private readonly DefeatedWindow _defeatedWindow;

        private bool _continuing;

        public LevelEngine(CityDefinition cityDefinition, ICoroutineRunner coroutineRunner, EnemiesFactory factory,
            ILevelMediator mediator, Energy energy, DefeatedWindow defeatedWindow)
        {
            _defeatedWindow = defeatedWindow;
            _cityDefinition = cityDefinition;
            _coroutineRunner = coroutineRunner;
            _factory = factory;
            _mediator = mediator;
            _energy = energy;
        }

        public void Initialize()
        {
            Start();
        }

        public void ContinuePlaying()
        {
            _defeatedWindow.ContinuePlaying -= ContinuePlaying;
            _continuing = true;
        }

        private void Start()
        {
            _coroutineRunner.StartCoroutine(CityCoroutine());
        }

        private IEnumerator CityCoroutine()
        {
            var index = 0;
            while (index < _cityDefinition.Levels.Length)
            {
                bool lost = false;
                yield return LevelCoroutine(index, () => lost = true);
                if (lost)
                {
                    _defeatedWindow.ContinuePlaying += ContinuePlaying;
                    yield return new WaitWhile(() => _continuing == false);
                    _continuing = false;
                    continue;
                }
                index++;
            }
        }

        private IEnumerator LevelCoroutine(int levelIndex, Action onLoose)
        {
            bool playerLost = false;
            LevelEntry level = _cityDefinition.Levels[levelIndex];
            level.LooseTrigger.ZombieInTrigger += () =>
            {
                playerLost = true;
            };
            _energy.DefineMaxEnergyAndFill(level.EnergyAmount);
            LevelEnemiesSpawner spawner = CreateEnemiesSpawner(level);
            spawner.Start();
            
            while (LevelCompleted(level) == false && playerLost == false)
            {
                yield return null;
            }

            if (playerLost)
            {
                _defeatedWindow.Show();
                onLoose.Invoke();
                spawner.Stop();
                _factory.FlushEnemies();
                yield break;
            }
            
            _mediator.StartNextLevelTimeline();
            _factory.FlushEnemies();
        }

        private LevelEnemiesSpawner CreateEnemiesSpawner(LevelEntry level)
        {
            return new LevelEnemiesSpawner(
                level: level,
                factory: _factory,
                coroutineRunner: _coroutineRunner,
                levelMediator: _mediator);
        }

        private bool LevelCompleted(LevelEntry rivalData)
        {
            return _factory.EnemiesDeadAmount == rivalData.EnemiesAmount();
        }
    }
}