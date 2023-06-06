using System;
using Gameplay.PeopleDraw.EnergyConsumption;
using PeopleDraw.Components;
using PeopleDraw.EnergyConsumption;
using Services.UseService;
using UniRx;
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

        [Inject]
        public void Construct(UnitsAssetCache usingService, Energy energy)
        {
            _usingService = usingService;
            _energy = energy;
        }

        public ReactiveProperty<PoolUnit> Current { get; } = new();
        public ReactiveProperty<UnitType> CurrentType { get; } = new(UnitType.Barrier);

        public void Select(UnitType type)
        {
            Current.Value = _usingService.SelectedUnits[type];
            CurrentType.Value = type;
        }

        public void NotifyUnitDrawn()
        {
            switch (CurrentType.Value)
            {
                case UnitType.Barrier:
                    _energy.SpendEnergy(2);
                    break;
                case UnitType.Melee:
                    _energy.SpendEnergy(8);
                    break;
                case UnitType.Ranged:
                    _energy.SpendEnergy(32);
                    break;
            }
        }

        public bool HasEnoughEnergyToSpawn()
        {
            int currentCost = CurrentType.Value switch
            {
                UnitType.Barrier => 2,
                UnitType.Melee => 8,
                UnitType.Ranged => 32,
                _ => throw new ArgumentOutOfRangeException()
            };
            return _energy.Amount >= currentCost;
        }
    }
}