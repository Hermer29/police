using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameBalance
{
    [Serializable]
    public class EnemyEntry
    {
        public AssetReference Prefab;
        public int Quantity;
    }
}