[System.Serializable]
public class RangeValue
{
    public int middle = 0;
    public int range = 0;

    public RangeValue(int middle, int range)
    {
        this.middle = middle;
        this.range = range;
    }

    public int GetRandomInRange()
    {
        return UnityEngine.Random.Range(middle - range, middle + range + 1);
    }
}
