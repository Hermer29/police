using UnityEngine;

namespace Tutorial.StepEndCondition
{
    public abstract class StepsEndCondition : MonoBehaviour
    {
        public abstract bool IsCompleted { get; }
    }
}