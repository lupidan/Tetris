using System.Collections.Generic;

public static class HashSetExtensions
{
    public static T[] ToArray<T>(this HashSet<T> set)
    {
        if (set == null)
            return null;

        T[] returnedArray = new T[set.Count];
        set.CopyTo(returnedArray);
        return returnedArray;
    }
}
