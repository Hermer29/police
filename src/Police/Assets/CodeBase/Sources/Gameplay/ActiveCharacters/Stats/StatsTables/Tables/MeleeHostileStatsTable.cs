using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GUIExtensions;
using UnityEngine;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Stats
{
    [CreateAssetMenu(menuName = "Stat Tables/Create Melee hostile stats table", fileName = "MeleeHostileStatsTable", order = 0)]
    public class MeleeHostileStatsTable : ScriptableObject, IEnumerable<MeleeHostileStatsEntry>
    {
        [Table] public MeleeHostileStatsEntry[] _entries;
        
        public IEnumerator<MeleeHostileStatsEntry> GetEnumerator() 
            => _entries.Cast<MeleeHostileStatsEntry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
}