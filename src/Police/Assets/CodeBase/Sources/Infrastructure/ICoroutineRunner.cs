using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public interface IBootstrapRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}