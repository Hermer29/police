using PeopleDraw.Selection;
using UnityEngine;
using Upgrading.UnitTypes;
using Zenject;

namespace Tutorial.StepEndCondition
{
    public class SelectedUnit : StepsEndCondition
    {
        [SerializeField] private UnitType _unit;
        
        private SelectedUnits _selection;

        [Inject]
        public void Construct(SelectedUnits selection) => _selection = selection;

        public override bool IsCompleted => _selection.Current.Value != null 
                                            && _selection.CurrentType.Value == _unit;
    }
}