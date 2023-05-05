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
            Vector3 scale = Multiply(unplacableUnit.SpawnBlock.size, unplacableUnit.transform.localScale);
            Vector3 spawnBlockCenter = unplacableUnit.transform.position + unplacableUnit.SpawnBlock.center;
            var results = Physics.OverlapBox(spawnBlockCenter, scale / 2, Quaternion.identity, BlockingUnitsLayer);
            
            return results.Length != 0;
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