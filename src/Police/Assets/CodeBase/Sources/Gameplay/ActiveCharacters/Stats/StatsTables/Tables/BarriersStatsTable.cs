using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GUIExtensions;
using UnityEngine;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [CreateAssetMenu(menuName = "Stat Tables/Create Barriers stats table", fileName = "BarriersStatsTable", order = 0)]
    public class BarriersStatsTable : ScriptableObject, IEnumerable<BarriersStatsEntry>
    {
        [Table] public BarriersStatsEntry[] _entries;
        
        public IEnumerator<BarriersStatsEntry> GetEnumerator() 
            => _entries.Cast<BarriersStatsEntry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();

        public BarriersStatsEntry this[UpgradableUnit unit] => _entries.First(x => x.RelatedUnit == unit);
    }
}