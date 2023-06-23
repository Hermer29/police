using System;
using UniRx;
using UnityEngine;
using Gizmos = Popcron.Gizmos;

namespace Helpers
{
    public static class DelayedGizmo
    {
        public static void Line(Vector3 a, Vector3 b, Color color, bool dashed = false, float time = 1)
        {
            CallInUpdateForTime(() =>
            {
                Gizmos.Line(a, b, color, dashed);
            }, time);
        }

        public static void Sphere(Vector3 position, float radius, Color color, bool dashed = false, int pointsCount = 16, float time = 1)
        {
            CallInUpdateForTime(() =>
            {
                Gizmos.Sphere(position, radius, color, dashed, pointsCount);
            }, time);
        }

        private static void CallInUpdateForTime(Action action, float time)
        {
            var startTime = Time.time;
            Observable.EveryUpdate().Where(x => startTime + time > Time.time)
                .Subscribe(_ => action());
        }
    }
}