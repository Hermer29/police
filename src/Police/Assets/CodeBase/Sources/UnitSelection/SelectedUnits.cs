using System;
using System.Linq;
using Gameplay.PeopleDraw.EnergyConsumption;
using PeopleDraw.Components;
using Selection;
using Services.UseService;
using UniRx;
using UnitSelection;
using UnityEngine;
using Upgrading.UnitTypes;
using Zenject;

namespace PeopleDraw.Selection
{
    public class SelectedUnits : MonoBehaviour
    {
        private UnitType _currentType;
        private Energy _energy;
        private UnitsAssetCache _usingService;
        private SelectorUi _ui;

        [Inject]
        public void Construct(UnitsAssetCache usingService, Energy energy, SelectorUi ui)
        {
            _ui = ui;
            _usingService = usingService;
            _energy = energy;

            Subscribe();
        }

        public ReactiveProperty<PoolUnit> Current { get; } = new();

        public ReactiveProperty<UnitType> CurrentType { get; } = new(UnitType.Barrier);

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
            UpdateButtonsActivation();
        }

        private void OnSelect(SelectorButton uiSelectorButton)
        {
            foreach (SelectorButton button in _ui.SelectorButtons)
            {
                button.Deselect();
            }
            if (_energy.Amount >= uiSelectorButton.UnitType.Cost())
            {
                uiSelectorButton.Select();
                Select(uiSelectorButton.UnitType);
            }
            UpdateButtonsActivation();
        }

        public void SelectUnit(UnitType type)
        {
            OnSelect(_ui.SelectorButtons.First(x => x.UnitType == type));
        }

        private void Select(UnitType type)
        {
            Current.Value = _usingService.SelectedUnits[type];
            CurrentType.Value = type;
        }

        public void NotifyUnitDrawn()
        {
            _energy.SpendEnergy(CurrentType.Value.Cost());
            UpdateButtonsActivation();
        }

        private void UpdateButtonsActivation()
        {
            foreach (SelectorButton button in _ui.SelectorButtons)
            {
                button.SelectButton.interactable = HasEnoughEnergyToSpawn(button);
            }
        }

        public bool HasEnoughEnergyToSpawn(SelectorButton button) 
            => button.UnitType.Cost() <= _energy.Amount;

        public bool HasEnoughEnergyToSpawnSelected() 
            => _currentType.Cost() <= _energy.Amount;
    }
}