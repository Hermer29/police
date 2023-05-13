using UnityEngine;

namespace ActiveCharacters.Shared.Components.Attacking
{
    public abstract class Attack : MonoBehaviour
    {
        public abstract void EnableAttack(Attackable target);
        public abstract void DisableAttack();
    }
}