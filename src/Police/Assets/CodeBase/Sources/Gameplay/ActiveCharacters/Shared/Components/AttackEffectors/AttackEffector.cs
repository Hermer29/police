using System.Collections;
using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public abstract class AttackEffector : MonoBehaviour
    {
        public abstract IEnumerator Execute(Attackable target);
    }
}