using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Levels.AssetManagement
{
    public class CharactersAssetLoader
    {
        public async Task<GameObject> InstantiateHostile(AssetReference assetReference, Vector3 position,
            Quaternion identity, Transform parentsZombie) =>
            await Addressables.InstantiateAsync(assetReference, position, identity, parentsZombie).Task;
    }
}