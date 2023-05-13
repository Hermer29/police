using System;
using Gameplay.Levels;

namespace GameBalance
{
    [Serializable]
    public class LevelEntry
    {
        public int EnergyAmount = 80;
        public WaveDefinition[] Waves;
        public LooseTrigger LooseTrigger;
    }
}