using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeopleDraw
{
    public struct Line
    {
        private readonly float _step;
        private List<Vector3> _hits;
        
        public Line(float step, Vector3 startPoint)
        {
            _step = step;
            _hits = new List<Vector3>();
            _hits.Add(startPoint);
        }

        public IEnumerable<(Vector3, Quaternion)> AppendAndGetNewPoints(Vector3 point)
        {
            var prev = _hits[^1];
            var current = (Vector3)default;
            var added = 0;
            
            while (current != prev)
            {
                current = Vector3.MoveTowards(prev, point, _step);
                if (current == point)
                    break;
                added++;
                _hits.Add(current);
            }

            if(added == 0)
            {
                yield break;
            }

            while (added > 1)
            {
                Vector3 pointA = _hits[^added];
                Vector3 pointB = _hits[^(added + 1)];
                Vector3 fromToVector = pointB - pointA;
                int relativeAngle = -90;
                if (Vector3.Dot(fromToVector, Vector3.right) > 0)
                {
                    relativeAngle = 90;
                }
                Quaternion rotation = Quaternion.LookRotation(fromToVector) 
                                      * Quaternion.Euler(0, relativeAngle, 0);
                yield return (pointA, rotation);
                added--;
            }
        }
    }
}