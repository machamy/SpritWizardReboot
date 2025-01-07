[System.Serializable]
public class RangeValue
{
    public int middle = 0;
    public int range = 0;

    public int GetRandomInRange()
    {
        return UnityEngine.Random.Range(middle - range, middle + range + 1);
    }
}
