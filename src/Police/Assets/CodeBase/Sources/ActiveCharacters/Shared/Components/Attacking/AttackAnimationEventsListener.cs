using System;
using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class AttackAnimationEventsListener : MonoBehaviour
    {
        public event Action AttackStarted;
        public event Action AttackEnded;
        
        private void OnStartAttack()
        {
            AttackStarted?.Invoke();
        }

        private void OnEndAttack()
        {
            AttackEnded?.Invoke();
        }
    }
}