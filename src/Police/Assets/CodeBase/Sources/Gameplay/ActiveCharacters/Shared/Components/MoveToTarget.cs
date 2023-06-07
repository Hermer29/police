using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MoveToTarget : DetectionReaction
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private bool _stopIfNoTarget;
        [SerializeField] private bool _log;

        private Transform _movingTarget;
        public Transform MovingTarget => _movingTarget;
        
        private void Update()
        {
            if (_movingTarget == null)
            {
                if(_stopIfNoTarget)
                    _agent.isStopped = true;
                return;
            }
            
            if(_log)
                Debug.Log(_agent.pathStatus);
            CalculatePath();
        }
        
        public override void Follow(Transform target)
        {
            if(_log)
                Debug.Log("Following target");
            _movingTarget = target;
            CalculatePath();
        }

        private void CalculatePath()
        {
            _agent.isStopped = false;
            var path = new NavMeshPath();
            _agent.CalculatePath(_movingTarget.position, path);
            _agent.SetPath(path);
        }

        public override void Forget()
        {
            if(_log)
                Debug.Log("Forget called");
            _movingTarget = null;
        }
    }
}