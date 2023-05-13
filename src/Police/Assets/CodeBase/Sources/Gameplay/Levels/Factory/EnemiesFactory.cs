﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveCharacters.Shared.Components;
using Gameplay.Levels.AssetManagement;
using Gameplay.Levels.UI;
using LevelsMachine;
using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Levels.Factory
{
    public class EnemiesFactory : ICharactersFactory
    {
        private readonly CharactersAssetLoader _loader;
        private readonly ILevelMediator _mediator;

        private List<Attackable> _enemies = new();

        public EnemiesFactory(CharactersAssetLoader loader, ILevelMediator mediator)
        {
            _loader = loader;
            _mediator = mediator;
        }

        public int EnemiesDeadAmount => _enemies.Count(x => x.Died);

        public async Task<CharactersNavigationLinks> InstantiateEnemy(AssetReference prefab, Vector3 position)
        {
            GameObject hostile = await _loader.InstantiateHostile(prefab, position, Quaternion.identity);
            var links = hostile.GetComponent<CharactersNavigationLinks>();
            _enemies.Add(links.Attackable);
            links.Attackable.UnitDied += AttackableOnUnitDied;
            return links;
        }

        private void AttackableOnUnitDied(object sender, Attackable e)
        {
            e.UnitDied -= AttackableOnUnitDied;
            _mediator.IncrementEnergyForEnemyDeath(e);
        }

        public bool InstantiatedEnemiesAlive()
        {
            return _enemies.All(x => x.Died == false);
        }

        public void FlushEnemies()
        {
            foreach (Attackable enemy in _enemies)
            {
                Object.Destroy(enemy.Root.gameObject);
            }
            _enemies.Clear();
        }
    }
}