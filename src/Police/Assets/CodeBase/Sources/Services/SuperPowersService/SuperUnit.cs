using System.Collections.Generic;
using ActiveCharacters.Shared.Components;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay.PeopleDraw.Factory;
using Helpers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Upgrading.UnitTypes;

namespace Services.SuperPowersService
{
    public class SuperUnit
    {
        private IAlliedUnitsFactory _factory;
        private List<GameObject> _superUnits = new List<GameObject>();

        private const float HelicopterAppearingHeight = 40;
        private const float LandingDuration = 1.5f;
        private const float HalfGroundTime = 0.5F;
        private const float TakeOffTime = 1f;
        
        public SuperUnit(IAlliedUnitsFactory factory)
        {
            _factory = factory;
            
            _factory.DestructionQueried += FactoryOnDestructionQueried;
        }

        public async void Summon()
        {
           
            var helicopter =  await _factory.CreateHelicopter();
            
            var middle = new MiddleScreenWorldPoint();
            var appearPoint = middle.Normal * HelicopterAppearingHeight + middle.Point;
            
            helicopter.transform.position = appearPoint;
            Land(helicopter, middle)
                .ThenDelay(HalfGroundTime)
                .Then(() => CreateSuperUnit(middle))
                .ThenDelay(HalfGroundTime)
                .Then(helicopter.transform.DOMoveY(appearPoint.y, TakeOffTime));
        }

        private async void CreateSuperUnit(MiddleScreenWorldPoint middleScreenWorldPoint)
        {
            var superUnit = await _factory.CreateSuperUnit();
            superUnit.GetComponent<NavMeshAgent>().Warp(middleScreenWorldPoint.Point);
            superUnit.GetComponent<CharactersNavigationLinks>().SetBalance(new AlliedUnitsBalanceDto()
            {
                Damage = 25f,
                Health = 1000
            });
            _superUnits.Add(superUnit);
        }

        private void FactoryOnDestructionQueried()
        {
            foreach (GameObject gameObject in _superUnits)
            {
                Object.Destroy(gameObject);
            }
        }

        private static TweenerCore<Vector3, Vector3, VectorOptions> Land(GameObject helicopter, MiddleScreenWorldPoint middle) 
            => helicopter.transform.DOMoveY(middle.Point.y, LandingDuration);
    }
}