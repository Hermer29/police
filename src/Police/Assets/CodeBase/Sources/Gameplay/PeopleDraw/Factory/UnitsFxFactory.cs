using System;
using Fx;
using Helpers;
using Logic;
using PeopleDraw.AssetManagement;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.Object;

namespace Gameplay.PeopleDraw.Factory
{
    public class UnitsFxFactory : IUnitsFxFactory
    {
        private readonly IUnitsFxAssetLoader _assetLoader;
        private readonly ParentsForGeneratedObjects _parents;
        
        private readonly Lazy<ObjectPool<ParticlesHolder>> _unitsSpawnFxPool;

        public UnitsFxFactory(IUnitsFxAssetLoader assetLoader, ParentsForGeneratedObjects parents)
        {
            _parents = parents;
            _assetLoader = assetLoader;
            
            _unitsSpawnFxPool = new Lazy<ObjectPool<ParticlesHolder>>(valueFactory: () =>
                new ObjectPool<ParticlesHolder>(
                    createFunc: FactorizeFx,
                    actionOnGet: fx => fx.Play(),
                    actionOnRelease: fx => fx.Stop())
            );
        }

        private ParticlesHolder FactorizeFx() => 
            Instantiate(_assetLoader.LoadSpawnFxParticles(), _parents.SpawnFx)
                .With(fx => fx.Construct(
                    new PoolHandle(() => _unitsSpawnFxPool.Value.Release(fx))));

        public ParticlesHolder CreateSpawnFx(Vector3 point) => 
            _unitsSpawnFxPool.Value.Get()
                .WithPosition(point);
    }
}