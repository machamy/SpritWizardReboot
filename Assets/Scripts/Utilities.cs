using UnityEngine;

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

    }
}