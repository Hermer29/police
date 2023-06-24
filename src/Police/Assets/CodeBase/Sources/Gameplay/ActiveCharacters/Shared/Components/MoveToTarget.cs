using System.Linq;
using Helpers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MoveToTarget : DetectionReaction
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private bool _stopIfNoTarget;
        [SerializeField] private bool _log;

        [SerializeField,ReadOnly] private BoxCollider _movingTarget;
        private Vector3[] _path = new Vector3[10];
        
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
            //_agent.destination = transform.position;
            _agent.isStopped = false;
            CalculatePath();
            // var pointOnTarget = _movingTarget.ClosestPointOnBounds(_agent.transform.position);
            // var headingTo = (pointOnTarget - _agent.transform.position).normalized;
            // var speedApplied = headingTo * (Time.deltaTime * _agent.speed);
            // DelayedGizmo.Sphere(transform.position+speedApplied, .4f, Color.blue);
            // _agent.Move(speedApplied);
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
            _agent.isStopped = false;
            var path = new NavMeshPath();
            //var point = ( - _agent.transform.position).normalized;
            //var pointBehind = _agent.transform.position + point * 3;
            _agent.CalculatePath(_movingTarget.ClosestPointOnBounds(_agent.transform.position).ProjectOnDrawPlane(), path);
            path.GetCornersNonAlloc(_path);
            float pathLength = 0;
            for (var i = 0; i < _path.Length - 1; i++)
            {
                pathLength += Vector3.Distance(_path[i], _path[i + 1]);
            }

            if (pathLength < 3)
            {
                return;
            }
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