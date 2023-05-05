using System;
using UnityEngine;

namespace Services
{
    public interface IInputService
    {
        event Action<RaycastHit> DrawnAtPoint;
    }
}