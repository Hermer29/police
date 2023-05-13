using System;
using ActiveCharacters.Enemies.Components;

namespace GameBalance
{
    [Serializable]
    public class WaveDefinition
    {
        public EnemiesSpawnPoint SpawnPoint;
        public EnemyEntry[] Enemies;
        public float TimeToThisWave;
    }
}