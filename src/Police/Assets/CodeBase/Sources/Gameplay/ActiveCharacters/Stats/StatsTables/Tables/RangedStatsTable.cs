﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GUIExtensions;
using UnityEngine;
using Upgrading.UnitTypes;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [CreateAssetMenu(menuName = "Stat Tables/Create Ranged stats table", fileName = "RangedStatsTable", order = 0)]
    public class RangedStatsTable : ScriptableObject, IEnumerable<RangedStatsEntry>
    {
        [Table] public RangedStatsEntry[] _entries;
        
        public IEnumerator<RangedStatsEntry> GetEnumerator() 
            => _entries.Cast<RangedStatsEntry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
        
        public RangedStatsEntry this[UpgradableUnit unit] => _entries.First(x => x.RelatedUnit == unit);
    }
}