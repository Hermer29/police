using Gameplay.Levels.UI;

namespace PeopleDraw.EnergyConsumption
{
    public class Energy
    {
        private readonly ILevelMediator _mediator;

        private int _currentEnergyValue = 0;
        private int _maxEnergyValue = 0;
        
        public Energy(ILevelMediator mediator)
        {
            _mediator = mediator;
        }

        public int Amount => _currentEnergyValue;

        public void DefineMaxEnergyAndFill(int value)
        {
            _currentEnergyValue = value;
            _maxEnergyValue = value;
            _mediator.DefineMaxEnergyValue(value);
        }

        public void SpendEnergy(int amount)
        {
            _currentEnergyValue -= amount;
            if (_currentEnergyValue < 0)
            {
                _currentEnergyValue = 0;
            }
            _mediator.UpdateEnergyValue(_currentEnergyValue);
        }

        public void RestoreEnergy(int amount)
        {
            _currentEnergyValue += amount;
            if (_currentEnergyValue > _maxEnergyValue)
            {
                _currentEnergyValue = _maxEnergyValue;
            }
            _mediator.UpdateEnergyValue(_currentEnergyValue);
        }
    }
}