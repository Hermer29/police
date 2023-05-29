using Fx;
using UnityEngine;
using static PeopleDraw.AssetManagement.UnitsFxAssetConstants;

namespace PeopleDraw.AssetManagement
{
    public class UnitsFxAssetLoader : IUnitsFxAssetLoader
    {
        private ParticlesHolder _spawnFx;

        public ParticlesHolder LoadSpawnFxParticles()
        {
            if (_spawnFx == null)
            {
                _spawnFx = Resources.Load<ParticlesHolder>(Spawnfx);
            }
            return _spawnFx;
        }
    }
}