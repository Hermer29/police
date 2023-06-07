using System;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.UnitTypes;

namespace Selection
{
    public class SelectorButton : MonoBehaviour
    {
        public Button SelectButton;
        public Image BackgroundHolder;
        public UnitType UnitType;
        
        [Header("Visual")]
        [SerializeField] private Sprite _selected;
        [SerializeField] private Sprite _insufficientEnergy;
        [SerializeField] private Sprite _notSelected;

        private bool _isNotEnoughEnergy = false;
        
        private void Update()
        {
            if (_isNotEnoughEnergy)
            {
                ShowInsufficientEnergy();
                if (SelectButton.interactable)
                {
                    Deselect();
                    _isNotEnoughEnergy = false;
                }

                return;
            }

            if (SelectButton.interactable == false)
            {
                _isNotEnoughEnergy = true;
                ShowInsufficientEnergy();
            }
        }

        public void Select() 
            => BackgroundHolder.sprite = _selected;

        public void ShowInsufficientEnergy() 
            => BackgroundHolder.sprite = _insufficientEnergy;

        public void Deselect() 
            => BackgroundHolder.sprite = _notSelected;
    }
}