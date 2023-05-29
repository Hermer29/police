using System;
using Gameplay.Levels.UI;
using Infrastructure;
using ModestTree;
using UnityEngine;
using Upgrading.UnitTypes;
using Zenject;

namespace Selection
{
    public class SelectorUi : MonoBehaviour
    {
        private ILevelMediator _mediator;
        
        [SerializeField] private SelectorButton[] _select;

        private void Start() => BindSelectButtons();

        private void BindSelectButtons()
        {
            for (var i = 0; i < _select.Length; i++) 
                SubscribeOnSelected(i);
        }

        private void SubscribeOnSelected(int current)
        {
            var button =  _select[current];
            button.SelectButton.onClick.AddListener(() => OnSelected(current, button.UnitType));
            _select[current].UnitType = current switch
            {
                0 => UnitType.Barrier,
                1 => UnitType.Melee,
                2 => UnitType.Ranged,
                _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
            };
        }

        private void OnSelected(int type, UnitType buttonUnitType)
        {
            _mediator ??= AllServices.Get<ILevelMediator>();
            Debug.Log($"{nameof(SelectorUi)}.{nameof(OnSelected)} called. Type: {type}, ButtonUnitType: {buttonUnitType}");
            switch (type)
            {
                case 0:
                    if (_mediator.EnergyAmount < 2)
                    {
                        return;
                    }
                    break;
                    
                case 1:
                    if (_mediator.EnergyAmount < 8)
                    {
                        return;
                    }

                    break;
                case 2:
                    if (_mediator.EnergyAmount < 32)
                    {
                        return;
                    }
                    break;
            }
            _mediator.MakeUnitSelected(buttonUnitType);
            UpdateSelection(selected: type);
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