using UnityEngine;

namespace Helpers
{
    public static class TransformExtensions
    {
        public static TComponent WithPosition<TComponent>(this TComponent component, Vector3 point) where TComponent: Component
        {
            component.transform.position = point;
            return component;
        }
    }
}