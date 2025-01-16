using System.Collections.Generic;

public class ListQueueConverter
{
    public static Queue<Queue<T>> ListToQueue<T>(List<List<T>> list)
    {
        Queue<Queue<T>> resultQueue = new Queue<Queue<T>>();
        foreach (List<T> inner in list)
        {
            resultQueue.Enqueue(new Queue<T>(inner));
        }
        return resultQueue;
    }

    public static List<List<T>> QueueToList<T>(Queue<Queue<T>> queue)
    {
        List<List<T>> resultList = new List<List<T>>();
        foreach (Queue<T> inner in queue)
        {
            resultList.Add(new List<T>(inner));
        }
        return resultList;
    }
}
