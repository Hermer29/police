using UnityEngine;

namespace Helpers
{
    public class ParentsForGeneratedObjects : MonoBehaviour
    {
        [field: SerializeField] public Transform SpawnFx { get; private set; }
        [field: SerializeField] public Transform Policemen { get; private set; }
        [field: SerializeField] public Transform Zombie { get; private set; }
    }
}