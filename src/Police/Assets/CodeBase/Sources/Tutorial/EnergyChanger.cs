using Gameplay.Levels.UI;
using Gameplay.PeopleDraw.EnergyConsumption;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class EnergyChanger : MonoBehaviour
    {
        private Energy _energy;
        private ILevelMediator _mediator;

        [Inject]
        public void Construct(ILevelMediator mediator, Energy energy)
        {
            _mediator = mediator;
            _energy = energy;
        }

        public void SetEnergyValue(int amount)
        {
            _mediator.UpdateEnergyValue(amount);
            _energy.RestoreEnergy(amount);
        }
    }
}