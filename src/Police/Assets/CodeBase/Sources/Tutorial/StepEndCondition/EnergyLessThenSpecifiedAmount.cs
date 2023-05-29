using Gameplay.PeopleDraw.EnergyConsumption;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using Zenject;

namespace Tutorial.StepEndCondition
{
    public class EnergyLessThenSpecifiedAmount : StepsEndCondition
    {
        [SerializeField] private int _lessThan;
        
        private Energy _energy;

        public override bool IsCompleted => _energy.Amount < _lessThan;

        [Inject]
        public void Construct(Energy energy)
        {
            _energy = energy;
        }
        
    }
}