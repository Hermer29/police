using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PeopleDraw.AssetManagement
{
    public class GameAssetLoader
    {
        public Task<GameObject> LoadSuperUnit()
        {
            return Addressables.LoadAssetAsync<GameObject>("SuperUnit").Task;
        }

        public Task<GameObject> LoadHelicopter()
        {
            return Addressables.LoadAssetAsync<GameObject>("Helicopter").Task;
        }   
    }
}