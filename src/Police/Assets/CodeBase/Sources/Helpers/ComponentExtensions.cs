using System;
using UnityEngine;

namespace Helpers
{
    public static class ComponentExtensions
    {
        public static TComponent With<TComponent>(this TComponent component, Action<TComponent> action)
            where TComponent : Component
        {
            action.Invoke(component);
            return component;
        }

        public static TComponent With<TSource, TComponent>(this TSource source, Action<TComponent> action) 
            where TSource: Component 
            where TComponent: Component
        {
            var required = source.GetComponent<TComponent>();
            action.Invoke(required);
            return required;
        }
    }
}