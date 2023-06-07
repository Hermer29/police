using Gameplay.Levels.UI;
using UnityEngine;

namespace Selection
{
    public class SelectorUi : MonoBehaviour
    {
        private ILevelMediator _mediator;
        
        [SerializeField] private SelectorButton[] _select;

        public SelectorButton[] SelectorButtons => _select;
    }
}