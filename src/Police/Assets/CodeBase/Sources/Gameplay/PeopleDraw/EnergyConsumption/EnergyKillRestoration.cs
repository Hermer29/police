using ActiveCharacters.Shared.Components;
using Gameplay.Levels.UI;

namespace Gameplay.PeopleDraw.EnergyConsumption
{
    public class EnergyKillRestoration
    {
        private readonly ILevelMediator _energy;
        private readonly EnergyKillRestorationView _view;

        private const int RestorePerKill = 1;

        public EnergyKillRestoration(ILevelMediator energy, EnergyKillRestorationView view)
        {
            _view = view;
            _energy = energy;
        }
        
        public void Restore(Attackable attackable)
        {
            _view.Show(attackable.Root.transform);
            _energy.RestoreEnergy(RestorePerKill);
        }
    }
}