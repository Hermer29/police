using UnityEngine;

namespace DefaultNamespace.Gameplay.ActiveCharacters.Shared
{
    public class RangeApplier : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        
        public void SetRange(float range)
        {
            _collider.radius = range;
        }
    }
}