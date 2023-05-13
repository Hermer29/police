using System.Threading.Tasks;
using ActiveCharacters.Shared.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LevelsMachine
{
    public interface ICharactersFactory
    {
        Task<CharactersNavigationLinks> InstantiateEnemy(AssetReference prefab, Vector3 position);
        bool InstantiatedEnemiesAlive();
        void FlushEnemies();
    }
}