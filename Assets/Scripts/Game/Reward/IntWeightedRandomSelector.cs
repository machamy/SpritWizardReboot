using System;

public class IntWeightedRandomSelector<T> where T : Enum
{
    private int[] cumulativeWeights;
    private T[] options;

    protected void SetValue(int[] weights)
    {
        options = (T[])Enum.GetValues(typeof(T));
        if (options.Length == weights.Length)
        {
            cumulativeWeights = new int[weights.Length];

            cumulativeWeights[0] = weights[0];
            for (int i = 1; i < weights.Length; i++)
            {
                cumulativeWeights[i] = cumulativeWeights[i - 1] + weights[i];
            }
        }
    }

    public T GetRandomChoice()
    {
        int totalWeight = cumulativeWeights[cumulativeWeights.Length - 1];
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        for (int i = 0; i < cumulativeWeights.Length; i++)
        {
            if (randomValue < cumulativeWeights[i])
            {
                return options[i];
            }
        }
        return default(T);
    }
}