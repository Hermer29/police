using ActiveCharacters.Shared.Components;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
using Monetization.AdvertisingPlane;
using NaughtyAttributes;
using PeopleDraw.EnergyConsumption;
using PeopleDraw.Selection;
using Selection;
using Services.SuperPowersService;
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
        [Inject] EnergyKillRestoration _killRestoration;
        [Inject] Energy _energy;
        [Inject] Nuke _nuke;
        [Inject] AdPlane _plane;

        public void ShowExclamationMark(Vector3 from, Vector3 to) => _arrow.ShowArrowTowardsTarget(from, to);
        public void UpdateEnergyValue(int amount) => _energyUi.SetEnergyValue(amount);

        public void IncrementEnergyForEnemyDeath(Attackable attackable) => _killRestoration.Restore(attackable);
        public void DefineMaxEnergyAndFill(int levelEnergyAmount) => _energy.DefineMaxEnergyAndFill(levelEnergyAmount);
        public int EnergyAmount => _energy.Amount;
        public void RestoreEnergy(int restorePerKill) => _energy.RestoreEnergy(restorePerKill);

        public void DefineMaxEnergyValue(int value) => _energyUi.DefineMaxValue(value);
        public void DestroyOldPeople() => _unitsFactory.ReturnAllToPool();
        [Button] public void LaunchNuke() => _nuke.LaunchNuke();

        public void AlliedUnitsToVictory() => _unitsFactory.AnimatorsPlayVictory();
        public void StartNextLevelTimeline() => _nextLevelDirector.Play();
        public void ShowPlayButton() => Debug.Log("Show play button");
        public void ShowLevelData() => Debug.Log("Show level data");
        public void SetTimeScale(float scale) => Time.timeScale = scale;
        [Button] public void ShowPlane() => _plane.Show();
    }
}