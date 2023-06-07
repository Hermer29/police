using System;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    [CreateAssetMenu]
    public class TownVisualMapping : ScriptableObject
    {
        [SerializeField] private MappingEntry[] _entries;

        private MappingEntry GetEntryForGlobal(int level)
        {
            if (TryGetEntryForGlobal(level, out MappingEntry result) == false)
                throw new InvalidOperationException($"No entries for level {level}");
            return result;
        }

        private bool TryGetEntryForGlobal(int level, out MappingEntry result)
        {
            foreach (var entry in _entries)
            {
                if (entry.FromLevel <= level && entry.ToLevel >= level)
                {
                    result = entry;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public Sprite GetCurrentTownSprite(int global)
            => GetEntryForGlobal(global).Sprite;

        public bool IsNextTownExists(int currentGlobal)
            => TryGetEntryForGlobal(currentGlobal + 10, out _);

        public Sprite GetNextTownSprite(int global)
            => GetEntryForGlobal(global + 10).Sprite;

        public TableEntryReference GetCurrentTownLocalizedName(int global)
            => GetEntryForGlobal(global).TableEntry;
    }
}