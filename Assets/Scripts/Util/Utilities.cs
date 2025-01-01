using Unity.Mathematics.Geometry;
using UnityEngine;
using Math = System.Math;

namespace DefaultNamespace
{
    public static class Utilities
    {
        public static int WeightedRandom(params int[] weights)
        {
            int totalWeight = 0;
            foreach (int weight in weights)
            {
                totalWeight += weight;
            }

            int randomValue = Random.Range(0, totalWeight);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomValue < weights[i])
                {
                    return i;
                }

                randomValue -= weights[i];
            }

            return weights.Length - 1;
        }
        
        public static Vector2Int Vector2IntClamp(Vector2Int value, Vector2Int min, Vector2Int max)
        {
            return new Vector2Int(
                Mathf.Clamp(value.x, min.x, max.x),
                Mathf.Clamp(value.y, min.y, max.y)
            );
        }
    }
    
}