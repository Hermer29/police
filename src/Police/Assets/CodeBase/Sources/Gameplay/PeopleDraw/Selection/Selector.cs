using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Levels.UI;
using Gameplay.PeopleDraw.Factory;
using PeopleDraw.Components;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace PeopleDraw.Selection
{
    public class Selector : MonoBehaviour
    {
        private PoolUnit[] _units;
        private Energy _energy;

        [Inject]
        public void Construct(IEnumerable<PoolUnit> units, Energy energy)
        {
            Assert.AreEqual(units.Count(), 3);

            _energy = energy;
            _units = units.ToArray();
            Select(0);
        }

        public PoolUnit Current { get; private set; }
        public int CurrentSerialNonIndex { get; private set; }

        public void Select(int whichOneIndex)
        {
            Current = _units[whichOneIndex];
            CurrentSerialNonIndex = whichOneIndex + 1;
        }

        public void NotifyUnitDrawn()
        {
            switch (CurrentSerialNonIndex)
            {
                case 1:
                    _energy.SpendEnergy(2);
                    break;
                case 2:
                    _energy.SpendEnergy(8);
                    break;
                case 3:
                    _energy.SpendEnergy(32);
                    break;
            }
        }

        public bool HasEnoughEnergyToSpawn()
        {
            int currentCost = CurrentSerialNonIndex switch
            {
                1 => 2,
                2 => 8,
                3 => 32,
                _ => throw new ArgumentOutOfRangeException()
            };
            return _energy.Amount >= currentCost;
        }
    }
}