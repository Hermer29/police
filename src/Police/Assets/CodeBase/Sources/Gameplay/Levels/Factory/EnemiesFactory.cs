using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Shared.Components;
using Gameplay.Levels.AssetManagement;
using Gameplay.Levels.UI;
using Gameplay.PeopleDraw.Factory;
using Helpers;
using Infrastructure;
using LevelsMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Levels.Factory
{
    public class EnemiesFactory : ICharactersFactory
    {
        private readonly CharactersAssetLoader _loader;
        private readonly ParentsForGeneratedObjects _parents;
        private readonly IUnitsFxFactory _unitsFxFactory;
        private readonly LevelEngine _levelEngine;

        private List<Attackable> _enemies = new();

        public EnemiesFactory(CharactersAssetLoader loader, ParentsForGeneratedObjects parents, 
            IUnitsFxFactory unitsFxFactory)
        {
            _loader = loader;
            _parents = parents;
            _unitsFxFactory = unitsFxFactory;
        }

        public int EnemiesDeadAmount => _enemies.Count(x => x.Died);

        public async Task<CharactersNavigationLinks> FactorizeEnemy(AssetReference prefab, Vector3 position)
        {
            GameObject hostile = await _loader.InstantiateHostile(prefab, position, Quaternion.identity, _parents.Zombie);
            var links = hostile.GetComponent<CharactersNavigationLinks>();
            _enemies.Add(links.Attackable);
            links.Attackable.UnitDied += AttackableOnUnitDied;
            links.RunToWin.Construct(AllServices.Get<LooseTrigger>());
            return links;
        }

        private void AttackableOnUnitDied(object sender, Attackable e)
        {
            e.UnitDied -= AttackableOnUnitDied;
            AllServices.Get<ILevelMediator>().IncrementEnergyForEnemyDeath(e);
            _unitsFxFactory.CreateDyingFx(e.Root.position);
        }

        public bool InstantiatedEnemiesAlive() 
            => _enemies.All(x => x.Died == false);

        public void FlushEnemies()
        {
            foreach (Attackable enemy in _enemies)
            {
                Object.Destroy(enemy.Root.gameObject);
            }
            _enemies.Clear();
        }

        public void TerminateEnemies()
        {
            foreach (Attackable enemy in _enemies)
            {
                enemy.Kill();
            }
        }
    }
}