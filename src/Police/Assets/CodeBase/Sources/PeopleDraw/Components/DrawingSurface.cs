using UnityEngine;

namespace PeopleDraw.Components
{
    public class DrawingSurface : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        
        private void OnDrawGizmos()
        {
            if (_collider == null)
                return;
            
            Gizmos.color = new Color(1, .2f, .2f, .5f);
            Gizmos.DrawCube(transform.position + _collider.center, new Vector3
            {
                x = transform.localScale.x,
                y = transform.localScale.y,
                z = transform.localScale.z
            });
        }
    }
}