using System;
using System.Collections;
using Infrastructure;
using UnityEngine;

namespace Helpers
{
    public static class CoroutinesExtension
    {
        public static Coroutine ContinueWith(this Coroutine coroutine, Action action)
        {
            var runner = AllServices.Get<ICoroutineRunner>();
            return runner.StartCoroutine(AsyncAwaiter(coroutine, action));
        }

        private static IEnumerator AsyncAwaiter(Coroutine coroutine, Action action)
        {
            yield return coroutine;
            action.Invoke();
        }
    }
}