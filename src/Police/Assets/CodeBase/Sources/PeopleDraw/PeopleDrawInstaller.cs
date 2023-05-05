using System;
using System.Collections.Generic;
using System.Linq;
using PeopleDraw.Components;
using PeopleDraw.Factory;
using PeopleDraw.Selection;
using UnityEngine;
using Zenject;

namespace PeopleDraw
{
    public class PeopleDrawInstaller : MonoInstaller
    {
        [SerializeField] private PoolUnit[] _poolUnits;

        private void OnValidate()
        {
            if (_poolUnits.Length > 3)
            {
                _poolUnits = _poolUnits.Take(3).ToArray();
            }
        }

        public override void InstallBindings()
        {
            Container.Bind<IEnumerable<PoolUnit>>().FromInstance(_poolUnits).AsSingle();
            Container.Bind<UnitsFactory>().ToSelf().AsSingle();
            Container.Bind<Selector>().ToSelf().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<Drawer>().ToSelf().AsSingle().NonLazy();
        }
    }
}