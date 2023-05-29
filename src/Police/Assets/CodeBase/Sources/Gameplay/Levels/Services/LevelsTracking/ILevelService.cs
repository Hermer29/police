using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels.Services.LevelsTracking
{
    public interface ILevelService
    {
        int Level { get; }
        int LocalLevel { get; }
        void IncrementLevel();
        event Action LevelIncremented;
        AsyncOperation LoadCurrentLevel();
        void WarpToCurrentLevel();
    }
}