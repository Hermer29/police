using System.Collections.Generic;
using System.Linq;
using Gameplay.PeopleDraw.Factory;
using PeopleDraw.Selection;
using UnityEngine;
using Upgrading.UnitTypes;
using Zenject;

namespace PeopleDraw
{
    public class PeopleDrawInstaller : MonoInstaller
    {
        [SerializeField] private PartialUpgradableUnit[] _poolUnits;

        private void OnValidate()
        {
            if (_poolUnits.Length > 3) _poolUnits = _poolUnits.Take(3).ToArray();
        }

        public override void InstallBindings()
        {
            Container.Bind<IEnumerable<PartialUpgradableUnit>>().FromInstance(_poolUnits).AsSingle();
            Container.Bind<IAlliedUnitsFactory>().To<AlliedUnitsFactory>().AsSingle();
            Container.Bind<SelectedUnits>().ToSelf().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<Drawer>().ToSelf().AsSingle().NonLazy();
        }
    }
}