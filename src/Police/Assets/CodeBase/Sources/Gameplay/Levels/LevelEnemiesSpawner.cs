using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Enemies.Components;
using DefaultNamespace.Gameplay;
using DefaultNamespace.Gameplay.ActiveCharacters;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using GameBalance;
using Gameplay.Levels.Services.LevelsTracking;
using Gameplay.Levels.UI;
using Infrastructure;
using LevelsMachine;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Levels
{
    public class LevelEnemiesSpawner
    {
        private readonly LevelEntry _level;
        private readonly ICharactersFactory _factory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ILevelMediator _levelMediator;
        private readonly BalanceProvider _balanceProvider;
        
        private int _waveIndex;
        private Coroutine _spawn;

        public LevelEnemiesSpawner(LevelEntry level, ICharactersFactory factory, ICoroutineRunner coroutineRunner,
            ILevelMediator levelMediator, BalanceProvider balanceProvider)
        {
            _balanceProvider = balanceProvider;
            _level = level;
            _factory = factory;
            _coroutineRunner = coroutineRunner;
            _levelMediator = levelMediator;
        }

        public void Start() => _spawn = _coroutineRunner.StartCoroutine(Spawn());

        private IEnumerator Spawn()
        {
            while (HaveMoreWavesToSpawn())
            {
                WaveDefinition currentWave = _level.Waves[_waveIndex];
                _levelMediator.ShowExclamationMark(Camera.main.transform.position.SetY(0), 
                    currentWave.SpawnPoint.transform.position);
                yield return SpawnWaveAndWaitForTime(currentWave);
                _waveIndex++;
            }
        }

        private bool HaveMoreWavesToSpawn() => _waveIndex < _level.Waves.Length;

        private IEnumerator SpawnWaveAndWaitForTime(WaveDefinition wave)
        {
            yield return new WaitForSeconds(wave.TimeToThisWave);
            SpawnEnemies(wave);
        }

        private void SpawnEnemies(WaveDefinition wave)
        {
            EnemiesSpawnPoint spawnPoint = wave.SpawnPoint;
            var enemyTypes = wave.Enemies;

            foreach (EnemyEntry enemyEntry in enemyTypes) 
                InstantiateSpecifiedEnemyQuantity(enemyEntry, spawnPoint);
        }

        private void InstantiateSpecifiedEnemyQuantity(EnemyEntry enemyType, EnemiesSpawnPoint spawnPoint)
        {
            var stats = _balanceProvider.GetFor(enemyType.Hostile);
            Debug.Log($"{nameof(LevelEnemiesSpawner)} instantiating enemies with stats Damage: {stats.Damage}, Health: {stats.Health}");
            foreach (int unused in Enumerable.Repeat(0, enemyType.Quantity))
            {
                InstantiateEnemy(enemyType.Hostile, spawnPoint);
            }
        }

        private void InstantiateEnemy(HostileUnit enemyType, EnemiesSpawnPoint spawnPoint)
        {
            _factory.FactorizeEnemy(
                prefab: enemyType.AssetReference,
                position: spawnPoint.transform.position,
                _balanceProvider.GetFor(enemyType));
        }

        private bool LastWave() => _waveIndex == _level.Waves.Length - 1;

        public void Stop() => _coroutineRunner.StopCoroutine(_spawn);
    }
}