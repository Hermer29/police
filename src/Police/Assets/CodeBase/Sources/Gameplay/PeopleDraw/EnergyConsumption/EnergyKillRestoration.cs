using PeopleDraw.EnergyConsumption;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class EnergyKillRestoration
    {
        private readonly Energy _energy;
        
        private const int RestorePerKill = 1;

        public EnergyKillRestoration(Energy energy)
        {
            _energy = energy;
        }
        
        public void Restore()
        {
            _energy.RestoreEnergy(RestorePerKill);
        }
    }
}