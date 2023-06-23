using Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MoveToTarget : DetectionReaction
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private bool _stopIfNoTarget;
        [SerializeField] private bool _log;

        private BoxCollider _movingTarget;
        
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
            _agent.isStopped = false;
            _agent.Move((_movingTarget.ClosestPoint(_agent.transform.position
            ) - _agent.transform.position).normalized * (Time.deltaTime * _agent.speed));
        }
        
        public override void Follow(BoxCollider target)
        {
            if(_log)
                Debug.Log("Following target");
            _movingTarget = target;
            //CalculatePath();
        }

        private void CalculatePath()
        {
            // _agent.isStopped = false;
            // var path = new NavMeshPath();
            // var point = (_movingTarget - _agent.transform.position).normalized;
            // var pointBehind = _agent.transform.position + point * 3;
            // _agent.CalculatePath(pointBehind.ProjectOnDrawPlane(), path);
            // _agent.SetPath(path);
        }

        public override void Forget()
        {
            if(_log)
                Debug.Log("Forget called");
            _movingTarget = null;
        }
    }
}