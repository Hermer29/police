using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    public class StatsAffectedUnit : MonoBehaviour
    {
        private IStatsHolder[] _statsHolders;
        
        private void Start()
        {
            _statsHolders = GetComponents(typeof(IStatsHolder)).Cast<IStatsHolder>().ToArray();
        }

        public THolder GetHolder<THolder>() where THolder: class, IStatsHolder
        {
            return _statsHolders.First(x => x is THolder) as THolder;
        }
    }
}