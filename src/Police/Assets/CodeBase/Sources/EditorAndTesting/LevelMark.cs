using UnityEngine;

namespace Testing
{
    public class LevelMark : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }
}