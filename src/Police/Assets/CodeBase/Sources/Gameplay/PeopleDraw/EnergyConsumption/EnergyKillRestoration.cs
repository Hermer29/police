using Gameplay.Levels.UI;
using PeopleDraw.EnergyConsumption;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class EnergyKillRestoration
    {
        private readonly ILevelMediator _energy;
        
        private const int RestorePerKill = 1;

        public EnergyKillRestoration(ILevelMediator energy)
        {
            _energy = energy;
        }
        
        public void Restore()
        {
            _energy.RestoreEnergy(RestorePerKill);
        }
    }
}