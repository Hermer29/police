using System;
using System.Linq;
using UnityEngine;

namespace Gameplay.Levels
{
    [CreateAssetMenu]
    public class LevelsMapper : ScriptableObject
    {
        public SceneEntry[] Entries;
        
        public string GetSceneName(int globalLevel) => GetEntry(globalLevel).SceneName;
        
        public int GetLocalLevel(int global) => global % 10;
        
        private SceneEntry GetEntry(int global) => Entries.First(x => x.Levels.Any(x => x == global));
    }

    [Serializable]
    public class SceneEntry
    {
        public string SceneName;
        public int[] Levels;
    }
}