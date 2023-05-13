using UnityEngine;

namespace ActiveCharacters.Enemies.Components
{
    public class EnemiesSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}