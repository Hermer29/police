using Tutorial.StepEndCondition;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public class TutorialStep : MonoBehaviour
    {
        [field: SerializeField] public StepsEndCondition StepsEndCondition { get; private set; }

        [SerializeField] public UnityEvent OnEnter;
        [SerializeField] public UnityEvent OnExit;
    }
}