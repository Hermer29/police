using ActiveCharacters.Shared.Components;
using UnityEditor;
using UnityEngine;

namespace VisualDebug
{
    public static class DebugDrawer
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        public static void DrawSphere(MoveToTarget moveToTarget, GizmoType gizmoType)
        {
            if (moveToTarget.MovingTarget != null)
            {
                Gizmos.DrawLine(moveToTarget.transform.position, moveToTarget.MovingTarget.position);
            }
        }
    }
}