using Gameplay.Levels.UI;
using ModestTree;
using PeopleDraw.EnergyConsumption;
using UnityEngine;
using Zenject;

namespace Gameplay.PeopleDraw.Selection
{
    public class SelectorUi : MonoBehaviour
    {
        private ILevelMediator _mediator;
        private Energy _energy;
        
        [SerializeField] private SelectorButton[] _select;

        [Inject]
        public void Construct(ILevelMediator mediator, Energy energy)
        {
            _mediator = mediator;
            _energy = energy;
            
            BindSelectButtons();
        }

        private void BindSelectButtons()
        {
            for (var i = 0; i < _select.Length; i++)
            {
                SubscribeOnSelected(i);
            }
        }

        private void SubscribeOnSelected(int current)
        {
            _select[current].SelectButton.onClick.AddListener(() => OnSelected(current));
        }

        private void OnSelected(int index)
        {
            switch (index)
            {
                case 0:
                    if (_energy.Amount < 2)
                    {
                        return;
                    }
                    break;
                    
                case 1:
                    if (_energy.Amount < 8)
                    {
                        return;
                    }

                    break;
                case 2:
                    if (_energy.Amount < 32)
                    {
                        return;
                    }
                    break;
            }
            _mediator.MakeUnitSelected(index);
            UpdateSelection(selected: index);
        }

        private void UpdateSelection(int selected)
        {
            SelectorButton justSelected = _select[selected];
            foreach (SelectorButton button in _select.Except(justSelected))
            {
                button.ShowNotSelected();
            }
            justSelected.ShowSelected();
        }

        public void UpdateOnEnergyUpdated(int amount)
        {
            UpdateSelector(amount, 2, _select[0]);
            UpdateSelector(amount, 8, _select[1]);
            UpdateSelector(amount, 32, _select[2]);
        }

        private void UpdateSelector(int amount, int cost, SelectorButton selectorButton)
        {
            bool insufficientEnergy = amount < cost;
            if (insufficientEnergy)
            {
                selectorButton.ShowInsufficientEnergy();
            }
            else
            {
                selectorButton.ShowNotSelected();
            }
        }
    }
}