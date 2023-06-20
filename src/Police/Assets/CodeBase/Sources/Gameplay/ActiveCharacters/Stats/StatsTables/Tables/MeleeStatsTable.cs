using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GUIExtensions;
using UnityEngine;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [CreateAssetMenu(menuName = "Stat Tables/Create Melee stats table", fileName = "MeleeStatsTable", order = 0)]
    public class MeleeStatsTable : ScriptableObject, IEnumerable<MeleeTableEntry>
    {
        [Table] public MeleeTableEntry[] _entries;
        
        public IEnumerator<MeleeTableEntry> GetEnumerator() 
            => _entries.Cast<MeleeTableEntry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}