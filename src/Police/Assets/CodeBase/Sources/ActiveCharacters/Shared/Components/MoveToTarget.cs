using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MoveToTarget : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        private Transform _movingTarget;

        public Transform MovingTarget => _movingTarget;
        
        private void Update()
        {
            if (_movingTarget == null)
                return;
            
            _agent.destination = _movingTarget.position;
        }
        
        public void Follow(Transform target)
        {
            _movingTarget = target;
        }

        public void Forget()
        {
            _movingTarget = null;
        }
    }
}