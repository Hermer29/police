using PeopleDraw.EnergyConsumption;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class Energy
    {
        private readonly EnergyUi _energyUi;

        private int _currentEnergyValue = 0;
        private int _maxEnergyValue = 0;

        public Energy(EnergyUi energyUi) => _energyUi = energyUi;
        
        public int Amount => _currentEnergyValue;

        public void DefineMaxEnergyAndFill(int value)
        {
            _currentEnergyValue = value;
            _maxEnergyValue = value;
            _energyUi.DefineMaxValue(value);
        }

        public void SpendEnergy(int amount)
        {
            _currentEnergyValue -= amount;
            if (_currentEnergyValue < 0) 
                _currentEnergyValue = 0;
            _energyUi.SetEnergyValue(_currentEnergyValue);
        }

        public void RestoreEnergy(int amount)
        {
            _currentEnergyValue += amount;
            if (_currentEnergyValue > _maxEnergyValue) 
                _currentEnergyValue = _maxEnergyValue;
            _energyUi.SetEnergyValue(_currentEnergyValue);
        }
    }
}