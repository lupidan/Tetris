using System;
using System.Collections;
using System.Collections.Generic;

public static class IListExtensions
{
    private static Random randomNumberGenerator = new Random();  

    public static void Shuffle<T>(this IList<T> list)  
    {
        if (list.Count <= 0)
            return;

        // Fisher–Yates shuffle
        for (int i = list.Count - 1; i > 0; --i)
        {
            int j = randomNumberGenerator.Next(i);
            T value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }
}
