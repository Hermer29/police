using System;
using System.Linq;
using PeopleDraw.Components;
using UnityEngine;

namespace PeopleDraw
{
    public static class UnitsExtensions
    {
        private static LayerMask BlockingUnitsLayer = LayerMask.NameToLayer("BlockingPlacement");
            
        public static bool OverlapsOtherUnit(this PlacingBlock unplacableUnit)
        {
            unplacableUnit.SpawnBlock.enabled = false;
            Vector3 scale = Multiply(unplacableUnit.SpawnBlock.size, unplacableUnit.transform.localScale);
            Vector3 spawnBlockCenter = unplacableUnit.transform.position + unplacableUnit.SpawnBlock.center;
            var results = Physics.OverlapBox(spawnBlockCenter, scale, Quaternion.identity);
            unplacableUnit.SpawnBlock.enabled = true;
            return results.Where(x => x.TryGetComponent<PlacingBlock>(out _))
                .Select(x => x.enabled).Count() != 0;
        }

        private static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3
            {
                x = a.x * b.x,
                y = a.y * b.y,
                z = a.z * b.z
            };
        }
    }
}