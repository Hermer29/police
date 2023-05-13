using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.Levels.UI
{
    public interface ILevelMediator
    {
        void ShowExclamationMark(Vector3 spawnPointPosition);
        void StartNextLevelTimeline();
        void MakeUnitSelected(int whichOne);
        void AlliedUnitsToVictory();
        void DefineMaxEnergyValue(int value);
        void UpdateEnergyValue(int amount);
        void IncrementEnergyForEnemyDeath(Attackable attackable);
        void ShowDefeatedWindow();
    }
}