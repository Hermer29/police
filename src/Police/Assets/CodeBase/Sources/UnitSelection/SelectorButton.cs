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
        [NonSerialized] public UnitType UnitType;
        
        [Header("Visual")]
        [SerializeField] private Sprite _selected;
        [SerializeField] private Sprite _insufficientEnergy;
        [SerializeField] private Sprite _notSelected;

        public void ShowSelected()
        {
            BackgroundHolder.sprite = _selected;
        }

        public void ShowInsufficientEnergy()
        {
            BackgroundHolder.sprite = _insufficientEnergy;
        }

        public void ShowNotSelected()
        {
            BackgroundHolder.sprite = _notSelected;
        }
    }
}