using ActiveCharacters.Shared.Components;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using Upgrading.UnitTypes;

namespace Gameplay.Levels.UI
{
    public interface ILevelMediator
    {
        void ShowExclamationMark(Vector3 spawnPointPosition);
        void StartNextLevelTimeline();
        void MakeUnitSelected(UnitType whichOne);
        void AlliedUnitsToVictory();
        void DefineMaxEnergyValue(int value);
        void UpdateEnergyValue(int amount);
        void IncrementEnergyForEnemyDeath(Attackable attackable);
        void ShowDefeatedWindow();
        void DefineMaxEnergyAndFill(int levelEnergyAmount);
        int EnergyAmount { get; }
        void RestoreEnergy(int restorePerKill);
    }
}