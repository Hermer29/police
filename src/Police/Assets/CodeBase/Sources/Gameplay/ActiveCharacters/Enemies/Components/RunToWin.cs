using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Components
{
    public class RunToWin : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        private void Update()
        {
            _agent.destination = WinPosition();
        }

        private Vector3 WinPosition()
        {
            Vector3 position = _agent.transform.position;
            return new Vector3
            {
                x = position.x,
                y = position.y,
                z = 1000
            };
        }
    }
}