using System.Collections.Generic;
using System.Linq;
using PeopleDraw.Components;
using PeopleDraw.Factory;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace PeopleDraw.Selection
{
    public class Selector : MonoBehaviour
    {
        private PoolUnit[] _units;

        [Inject]
        public void Construct(IEnumerable<PoolUnit> units, UnitsFactory factory)
        {
            Assert.AreEqual(units.Count(), 3);

            _units = units.ToArray();
            Select(0);
        }

        public PoolUnit Current { get; private set; }
        public int CurrentSerialNonIndex { get; set; }

        public void Select(int whichOneIndex)
        {
            Current = _units[whichOneIndex];
            CurrentSerialNonIndex = whichOneIndex + 1;
        }
    }
}