using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Levels.AssetManagement
{
    public class CharactersAssetLoader
    {
        public async Task<GameObject> InstantiateHostile(AssetReference assetReference, Vector3 position,
            Quaternion identity, Transform parentsZombie)
        {
            var loading = await Addressables.LoadAssetAsync<GameObject>(assetReference).Task;
            return Object.Instantiate(loading, position, identity, parentsZombie);
        }

        public GameObject LoadBatonPoliceman(Vector3 position, Quaternion lookRotation)
        {
            throw new System.NotImplementedException();
        }
    }
}