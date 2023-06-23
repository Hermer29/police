using System;
using UnityEngine;

namespace Services
{
    public interface IInputService
    {
        event Action<RaycastHit> DrawnAtPoint;
        event Action<RaycastHit> DragInProgress;
        event Action DragEnded;
        event Action<Vector3> DragStarted;
        void Disable();
        void Enable();
    }
}