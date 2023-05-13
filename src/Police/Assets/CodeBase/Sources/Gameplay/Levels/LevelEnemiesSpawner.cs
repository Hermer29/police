using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Enemies.Components;
using GameBalance;
using Gameplay.Levels.UI;
using Infrastructure;
using LevelsMachine;
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
        
        private int _waveIndex;
        private bool _allEnemiesSpawned;
        private Coroutine _spawn;

        public LevelEnemiesSpawner(LevelEntry level, ICharactersFactory factory, ICoroutineRunner coroutineRunner,
            ILevelMediator levelMediator)
        {
            _level = level;
            _factory = factory;
            _coroutineRunner = coroutineRunner;
            _levelMediator = levelMediator;
        }

        public void Start()
        {
            _spawn = _coroutineRunner.StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (HaveMoreWavesToSpawn())
            {
                WaveDefinition currentWave = _level.Waves[_waveIndex];
                _levelMediator.ShowExclamationMark(currentWave.SpawnPoint.transform.position);
                yield return SpawnWaveAndWaitForTime(currentWave);
                _waveIndex++;
            }
        }

        private bool HaveMoreWavesToSpawn()
        {
            return _waveIndex < _level.Waves.Length;
        }

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
            {
                InstantiateSpecifiedEnemyQuantity(enemyEntry, spawnPoint);
            }
            _allEnemiesSpawned = LastWave();
        }

        private void InstantiateSpecifiedEnemyQuantity(EnemyEntry enemyType, EnemiesSpawnPoint spawnPoint)
        {
            foreach (int unused in Enumerable.Repeat(0, enemyType.Quantity))
            {
                InstantiateEnemy(enemyType.Prefab, spawnPoint);
            }
        }

        private void InstantiateEnemy(AssetReference enemyType, EnemiesSpawnPoint spawnPoint)
        {
            _factory.InstantiateEnemy(
                prefab: enemyType,
                position: spawnPoint.transform.position);
        }

        public bool LevelAllEnemiesSpawned()
        {
            return _allEnemiesSpawned;
        }

        private bool LastWave()
        {
            return _waveIndex == _level.Waves.Length - 1;
        }

        public void Stop()
        {
            _coroutineRunner.StopCoroutine(_spawn);
        }
    }
}