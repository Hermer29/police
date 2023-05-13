using ActiveCharacters.Shared.Components;
using Gameplay.Levels.UI.Defeated;
using Gameplay.PeopleDraw.EnergyConsumption;
using Gameplay.PeopleDraw.Factory;
using Gameplay.PeopleDraw.Selection;
using PeopleDraw.EnergyConsumption;
using PeopleDraw.Selection;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace Gameplay.Levels.UI
{
    public class LevelMediator : MonoBehaviour, ILevelMediator
    {
        [SerializeField] PlayableDirector _nextLevelDirector;
        [SerializeField] Arrow _arrow;
        
        [Inject] IAlliedUnitsFactory _unitsFactory;
        [Inject] Selector _selector;
        [Inject] EnergyUi _energyUi;
        [Inject] SelectorUi _selectorUi;
        [Inject] EnergyKillRestoration _killRestoration;
        [Inject] DefeatedWindow _defeatedWindow;

        public void ShowExclamationMark(Vector3 spawnPointPosition) => _arrow.ShowArrowTowardsTarget(spawnPointPosition);
        public void MakeUnitSelected(int whichOne) => _selector.Select(whichOne);
        public void UpdateEnergyValue(int amount)
        {
            _selectorUi.UpdateOnEnergyUpdated(amount);
            _energyUi.SetEnergyValue(amount);
        }
        
        public void IncrementEnergyForEnemyDeath(Attackable attackable) => _killRestoration.Restore();
        public void ShowDefeatedWindow() => _defeatedWindow.Show();

        public void DefineMaxEnergyValue(int value) => _energyUi.DefineMaxValue(value);
        public void DestroyOldPeople() => _unitsFactory.ReturnAllToPool();
        public void AlliedUnitsToVictory() => _unitsFactory.AnimatorsPlayVictory();
        public void StartNextLevelTimeline() => _nextLevelDirector.Play();
        public void ShowPlayButton() => Debug.Log("Show play button");
        public void ShowLevelData() => Debug.Log("Show level data");
    }
}