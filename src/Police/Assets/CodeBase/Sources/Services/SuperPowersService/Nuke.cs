using DefaultNamespace.Audio;
using DG.Tweening;
using Gameplay.Levels.Factory;
using Helpers;
using Infrastructure;
using UnityEngine;

namespace Services.SuperPowersService
{
    public class Nuke
    {
        private readonly IGameFactory _gameFactory;
        private readonly EnemiesFactory _enemiesFactory;
        private GlobalAudio _audio;

        private const float NukeSpawnHeight = 40;
        private const float FallTime = .5f;

        private GameObject _nuke;

        public Nuke(IGameFactory gameFactory, EnemiesFactory enemiesFactory, GlobalAudio audio)
            => (_gameFactory, _enemiesFactory, _audio) = (gameFactory, enemiesFactory, audio);

        public void LaunchNuke()
        {
            _nuke ??= _gameFactory.CreateNuke();
            var levelsMiddle = new MiddleScreenWorldPoint();
            Vector3 nukeSpawnPoint = levelsMiddle.Normal * NukeSpawnHeight;
            InitializeNuke(nukeSpawnPoint);
            Time.timeScale = 1;   
            _nuke.transform.DOMoveY(levelsMiddle.Point.y, FallTime)
                .Then(() => CreateNukeExplosion(levelsMiddle.Point));
        }

        private void CreateNukeExplosion(Vector3 point)
        {
            _enemiesFactory.TerminateEnemies();
            _gameFactory.CreateNukeFx(at: point);
            _audio.PlayNuke(0.2f);
        }

        private void InitializeNuke(Vector3 nukeSpawnPoint)
        {
            _nuke.transform.position = nukeSpawnPoint;
            _nuke.gameObject.SetActive(true);
        }
    }
}