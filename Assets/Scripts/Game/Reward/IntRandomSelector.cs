using System.Collections.Generic;

public class IntRandomSelector<T>
{
    private int len;
    private int[] chances;
    private T[] options;

    public IntRandomSelector(T[] options, int[] chances)
    {
        if (options.Length == chances.Length)
        {
            len = options.Length;
            this.chances = chances;
            this.options = options;
        }
    }

    public List<T> GetRandomChoice()
    {
        List<T> randomSelected = new List<T>();
        int ran = UnityEngine.Random.Range(0, 101);
        for (int i = 0; i < len; i++)
        {
            if (ran <= chances[i])
            {
                randomSelected.Add(options[i]);
            }
        }

        return randomSelected;
    }
}
