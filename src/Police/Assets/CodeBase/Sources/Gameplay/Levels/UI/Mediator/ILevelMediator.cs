using ActiveCharacters.Shared.Components;
using PeopleDraw.EnergyConsumption;
using UnityEngine;

namespace Gameplay.Levels.UI
{
    public interface ILevelMediator
    {
        void ShowExclamationMark(Vector3 from, Vector3 to);
        void StartNextLevelTimeline();
        void AlliedUnitsToVictory();
        void DefineMaxEnergyValue(int value);
        void UpdateEnergyValue(int amount);
        void IncrementEnergyForEnemyDeath(Attackable attackable);
        void DefineMaxEnergyAndFill(int levelEnergyAmount);
        int EnergyAmount { get; }
        void RestoreEnergy(int restorePerKill);
    }
}