using System;
using Gameplay.Levels;
using UnityEngine;

namespace GameBalance
{
    [Serializable]
    public class LevelEntry
    {
        public int EnergyAmount = 80;
        public WaveDefinition[] Waves;
        public LooseTrigger LooseTrigger;
        public Transform LevelMiddle;
    }
}