using Fx;
using UnityEngine;

namespace Gameplay.PeopleDraw.Factory
{
    public interface IUnitsFxFactory
    {
        ParticlesHolder CreateSpawnFx(Vector3 point);
    }
}