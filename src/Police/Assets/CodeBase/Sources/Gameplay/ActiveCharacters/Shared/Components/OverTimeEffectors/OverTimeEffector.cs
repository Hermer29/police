using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors
{
    public abstract class OverTimeEffector : MonoBehaviour
    {
        public abstract void StartExecuting(Attackable target);
        public abstract void EndExecuting();
    }
}