using System;
using System.Collections;
using GameBalance;
using Gameplay.Levels.Factory;
using Gameplay.Levels.UI;
using Infrastructure;

namespace Gameplay.Levels
{
    public sealed class LevelEngine
    {
        private readonly CityDefinition _cityDefinition;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ILevelMediator _mediator;
        private EnemiesFactory _factory;

        private bool _continuing;
        private bool _lost;
        private bool _won;

        public LevelEngine(CityDefinition cityDefinition, EnemiesFactory factory,
            ILevelMediator mediator)
        {
            _cityDefinition = cityDefinition;
            _coroutineRunner = AllServices.Get<ICoroutineRunner>();
            _factory = factory;
            _mediator = mediator;
        }

        public void ExecuteLevel(int localLevel) 
            => _coroutineRunner.StartCoroutine(CityCoroutine(localLevel));

        public bool IsLost() => _lost;
        public bool IsWon() => _won;

        private IEnumerator CityCoroutine(int localLevel)
        {
            _lost = false;
            _won = false;
            
            bool lost = false;
            var asIndex = localLevel - 1;
            
            yield return LevelCoroutine(_cityDefinition.Levels[asIndex], () => lost = true);
            if (lost)
            {
                _lost = true;
                yield break;
            }
            _won = true;
        }

        private IEnumerator LevelCoroutine(LevelEntry level, Action onLoose)
        {
            bool playerLost = false;
            InitializeLevel(
                level, 
                out LevelEnemiesSpawner spawner, 
                onLost: () => playerLost = true);

            while (LevelCompleted(level) == false && playerLost == false)
                yield return null;

            if (playerLost)
            {
                _lost = true;
                onLoose.Invoke();
                spawner.Stop();
            }
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