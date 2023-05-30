using ActiveCharacters.Shared.Components;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
using PeopleDraw.EnergyConsumption;
using PeopleDraw.Selection;
using Selection;
using UI;
using UnityEngine;
using UnityEngine.Playables;
using Upgrading.UnitTypes;
using Zenject;

namespace Gameplay.Levels.UI
{
    public class LevelMediator : MonoBehaviour, ILevelMediator
    {
        [SerializeField] PlayableDirector _nextLevelDirector;
        
        [Inject(Id = "NextWaveArrow")] Arrow _arrow;
        [Inject] IAlliedUnitsFactory _unitsFactory;
        [Inject] SelectedUnits _selectedUnits;
        [Inject] EnergyUi _energyUi;
        [Inject] SelectorUi _selectorUi;
        [Inject] EnergyKillRestoration _killRestoration;
        [Inject] Energy _energy;

        public void ShowExclamationMark(Vector3 spawnPointPosition) => _arrow.ShowArrowTowardsTarget(spawnPointPosition);
        public void MakeUnitSelected(UnitType whichOne) => _selectedUnits.Select(whichOne);
        public void UpdateEnergyValue(int amount)
        {
            _selectorUi.UpdateOnEnergyUpdated(amount);
            _energyUi.SetEnergyValue(amount);
        }

        public void IncrementEnergyForEnemyDeath(Attackable attackable) => _killRestoration.Restore();
        public void DefineMaxEnergyAndFill(int levelEnergyAmount) => _energy.DefineMaxEnergyAndFill(levelEnergyAmount);
        public int EnergyAmount => _energy.Amount;
        public void RestoreEnergy(int restorePerKill) => _energy.RestoreEnergy(restorePerKill);

        public void DefineMaxEnergyValue(int value) => _energyUi.DefineMaxValue(value);
        public void DestroyOldPeople() => _unitsFactory.ReturnAllToPool();
        public void AlliedUnitsToVictory() => _unitsFactory.AnimatorsPlayVictory();
        public void StartNextLevelTimeline() => _nextLevelDirector.Play();
        public void ShowPlayButton() => Debug.Log("Show play button");
        public void ShowLevelData() => Debug.Log("Show level data");
        public void SetTimeScale(float scale) => Time.timeScale = scale;
    }
}