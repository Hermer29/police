using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components
{
    public class Attacker : MonoBehaviour
    {
        private float _damage;

        public void SetStats(float dealingDamage)
        {
            _damage = dealingDamage;
        }
        
        public void ApplyDamage(Attackable target)
        {
            target.ApplyDamage(_damage);
        }
    }
}