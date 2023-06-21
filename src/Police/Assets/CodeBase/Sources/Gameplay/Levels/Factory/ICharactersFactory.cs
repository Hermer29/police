using System.Threading.Tasks;
using ActiveCharacters.Shared.Components;
using DefaultNamespace.Gameplay;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LevelsMachine
{
    public interface ICharactersFactory
    {
        Task<CharactersNavigationLinks> FactorizeEnemy(AssetReference prefab, Vector3 position, UnitBalanceDto balanceDto);
        bool InstantiatedEnemiesAlive();
        void FlushEnemies();
    }
}