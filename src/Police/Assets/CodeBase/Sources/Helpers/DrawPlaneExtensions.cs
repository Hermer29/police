using UnityEngine;

namespace Helpers
{
    public static class DrawPlaneExtensions
    {
        public static Vector3 ProjectOnDrawPlane(this Vector3 point)
        {
            var mask = 1 << LayerMask.NameToLayer("RaycastPlane");
            Physics.Raycast(point + Vector3.up * 50, Vector3.down, out var raycastHit, 9999, mask);
            return raycastHit.point;
        }
    }
}