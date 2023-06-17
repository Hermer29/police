using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Upgrading.UnitTypes
{
    [Serializable]
    public class LevelPartUnits
    {
        public AssetReference Asset;
        public Sprite Illustration;

        public bool IsEmpty => Asset.RuntimeKeyIsValid() == false;
    }
}