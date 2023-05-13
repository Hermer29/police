using UnityEngine;

namespace PeopleDraw.Components
{
    public class DrawingSurface : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private bool _drawGizmo;
        
        private void OnDrawGizmos()
        {
            if (_drawGizmo == false)
                return;
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