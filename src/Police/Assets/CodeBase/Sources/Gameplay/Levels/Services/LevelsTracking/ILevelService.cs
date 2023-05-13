using System;

namespace Gameplay.Levels.Services.LevelsTracking
{
    public interface ILevelService
    {
        int Level { get; }
        void IncrementLevel();
        event Action LevelIncremented;
    }
}