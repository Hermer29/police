using System.Linq;
using Gameplay.PeopleDraw.EnergyConsumption;
using Selection;
using UniRx;
using UnitSelection;
using UnityEngine;
using Upgrading.UnitTypes;
using Zenject;

namespace PeopleDraw.Selection
{
    public class SelectedUnits : MonoBehaviour
    {
        private Energy _energy;
        private SelectorUi _ui;

        [Inject]
        public void Construct(Energy energy, SelectorUi ui)
        {
            _ui = ui;
            _energy = energy;

            Subscribe();
        }

        public ReactiveProperty<UnitType> SelectedType { get; } = new(UnitType.Barrier);

        private void Subscribe()
        {
            _energy.Updated += OnEnergyUpdated;
            foreach (SelectorButton uiSelectorButton in _ui.SelectorButtons)
            {
                uiSelectorButton.SelectButton.onClick.AddListener(() => OnSelect(uiSelectorButton));
            }
        }

        private void OnEnergyUpdated()
        {
            if (HasEnoughEnergyToSpawnSelected() == false)
                SelectedType.Value = UnitType.None;
            UpdateButtonsActivation();
        }

        public void SelectUnit(UnitType type)
        {
            OnSelect(_ui.SelectorButtons.First(x => x.UnitType == type));
        }

        private void OnSelect(SelectorButton uiSelectorButton)
        {
            Debug.Log($"Button selected {uiSelectorButton.UnitType}");
            foreach (SelectorButton button in _ui.SelectorButtons)
            {
                button.Deselect();
            }
            if (_energy.Amount >= uiSelectorButton.UnitType.Cost())
            {
                uiSelectorButton.Select();
                SelectedType.Value = uiSelectorButton.UnitType;
            }
            UpdateButtonsActivation();
        }

        public void NotifyUnitDrawn()
        {
            _energy.SpendEnergy(SelectedType.Value.Cost());
            UpdateButtonsActivation();
        }

        private void UpdateButtonsActivation()
        {
            foreach (SelectorButton button in _ui.SelectorButtons)
            {
                button.SelectButton.interactable = HasEnoughEnergyToSpawn(button);
            }
            if(HasEnoughEnergyToSpawnSelected() == false)
            {
                SelectedType.Value = UnitType.None;
            }
        }

        public bool HasEnoughEnergyToSpawn(SelectorButton button) 
            => button.UnitType.Cost() <= _energy.Amount;

        public bool HasEnoughEnergyToSpawnSelected() 
            => SelectedType.Value.Cost() <= _energy.Amount;
    }
}