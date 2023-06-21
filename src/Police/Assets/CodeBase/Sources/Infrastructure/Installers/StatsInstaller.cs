using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class StatsInstaller : MonoInstaller
    {
        [SerializeField] private MeleeStatsTable _meleeStatsTable;
        [SerializeField] private RangedStatsTable _rangedStatsTable;
        [SerializeField] private BarriersStatsTable _barriersStatsTable;
        [SerializeField] private MeleeHostileStatsTable _zombiesStatsTable;
        
        public override void InstallBindings()
        {
            Container.BindInstances(_meleeStatsTable, _rangedStatsTable, _barriersStatsTable, _zombiesStatsTable);
            AllServices.Bind(_meleeStatsTable);
            AllServices.Bind(_rangedStatsTable);
            AllServices.Bind(_barriersStatsTable);
            AllServices.Bind(_zombiesStatsTable);
        }
    }
}