using System;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Gameplay.Levels.UI.CrossLevelUi
{
    [Serializable]
    public class MappingEntry
    {
        public int FromLevel;
        public int ToLevel;
        public Sprite Sprite;
        public TableEntryReference TableEntry;
    }
}