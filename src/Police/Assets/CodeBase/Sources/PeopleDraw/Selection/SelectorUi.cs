using System.Collections.Generic;
using PeopleDraw.Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PeopleDraw.Selection
{
    public class SelectorUi : MonoBehaviour
    {
        private Selector _selector;
        
        [SerializeField] private Button[] _select;

        [Inject]
        public void Construct(Selector selector, IEnumerable<PoolUnit> units)
        {
            _selector = selector;

            for (var i = 0; i < _select.Length; i++)
            {
                int whichOne = i;
                _select[i].onClick.AddListener(() => _selector.Select(whichOne));
            }
        }
    }
}