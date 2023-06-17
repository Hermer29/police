using UnityEngine;

namespace Gameplay.ActiveCharacters.Allies.Components
{
    public abstract class AlliedAggro : MonoBehaviour
    {
        public abstract void Stop();
        public abstract void Enable();
    }
}