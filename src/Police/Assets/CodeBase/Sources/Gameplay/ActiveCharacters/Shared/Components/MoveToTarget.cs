using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MoveToTarget : DetectionReaction
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
        
        public override void Follow(Transform target)
        {
            _movingTarget = target;
        }

        public override void Forget()
        {
            _movingTarget = null;
        }
    }
}