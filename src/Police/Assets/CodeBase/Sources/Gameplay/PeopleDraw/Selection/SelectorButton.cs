using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.PeopleDraw.Selection
{
    public class SelectorButton : MonoBehaviour
    {
        public Button SelectButton;
        public Image BackgroundHolder;
        
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