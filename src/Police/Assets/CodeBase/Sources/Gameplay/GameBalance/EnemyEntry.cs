using System;
using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameBalance
{
    [Serializable]
    public class EnemyEntry
    {
        public HostileUnit Hostile;
        public int Quantity;
    }
}