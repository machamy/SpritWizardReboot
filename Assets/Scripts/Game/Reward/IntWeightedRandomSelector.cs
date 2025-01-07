public class IntWeightedRandomSelector<T>
{
    private int[] cumulativeWeights;
    private T[] options;

    public IntWeightedRandomSelector(T[] options, int[] weights)
    {
        if (options.Length == weights.Length)
        {
            this.options = options;
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