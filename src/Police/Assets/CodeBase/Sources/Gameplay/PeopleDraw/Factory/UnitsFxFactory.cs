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
        
        private Lazy<ObjectPool<ParticlesHolder>> _unitsSpawnFxPool;
        private Lazy<ObjectPool<ParticlesHolder>> _unitsDyingFxPool;

        public UnitsFxFactory(IUnitsFxAssetLoader assetLoader, ParentsForGeneratedObjects parents)
        {
            _parents = parents;
            _assetLoader = assetLoader;
            
            CreateParticles();
            
            
        }

        private void CreateParticles()
        {
            _unitsSpawnFxPool = new Lazy<ObjectPool<ParticlesHolder>>(valueFactory: () =>
                new ObjectPool<ParticlesHolder>(
                    createFunc: FactorizeFx,
                    actionOnGet: fx => fx.Play(),
                    actionOnRelease: fx => fx.Stop())
            );

            _unitsDyingFxPool = new Lazy<ObjectPool<ParticlesHolder>>(valueFactory: () =>
                new ObjectPool<ParticlesHolder>(
                    createFunc: FactorizeFx,
                    actionOnGet: fx => fx.Play(),
                    actionOnRelease: fx => fx.Stop()));
        }

        private ParticlesHolder FactorizeFx() => 
            Instantiate(_assetLoader.LoadSpawnFxParticles(), _parents.SpawnFx)
                .With(fx => fx.Construct(
                    new PoolHandle(() => _unitsSpawnFxPool.Value.Release(fx))));

        public ParticlesHolder CreateSpawnFx(Vector3 point) => CreateFx(point, _unitsSpawnFxPool);

        private ParticlesHolder CreateFx(Vector3 point, Lazy<ObjectPool<ParticlesHolder>> fxPool) =>
            fxPool.Value.Get()
                .WithPosition(point);

        public ParticlesHolder CreateDyingFx(Vector3 point) => CreateFx(point, _unitsDyingFxPool);
    }
}