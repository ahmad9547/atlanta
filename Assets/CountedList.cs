using System.Collections.Generic;
using UnityEngine;

public class CountedList<T>
{
    private List<(T item, int count)> itemsWithCounts = new List<(T, int)>();

    public void Add(T item)
    {
        if (itemsWithCounts.Count > 0 && EqualityComparer<T>.Default.Equals(itemsWithCounts[^1].item, item))
        {
            // If the last item is the same as the new item, increment the count
            itemsWithCounts[^1] = (item, itemsWithCounts[^1].count + 1);
        }
        else
        {
            // Otherwise, add the item with count 1
            itemsWithCounts.Add((item, 1));
        }
    }

    public bool IsEmpty()
    {
        return itemsWithCounts.Count == 0;
    }

    public T Peek()
    {
        if (itemsWithCounts.Count > 0) return itemsWithCounts[^1].item;
        return default;
    }

    public void Remove(T item)
    {
        for (int i = itemsWithCounts.Count - 1; i >= 0; i--)
        {
            if (EqualityComparer<T>.Default.Equals(itemsWithCounts[i].item, item))
            {
                if (itemsWithCounts[i].count == 1)
                {
                    // If count is 1, remove the item
                    itemsWithCounts.RemoveAt(i);
                }
                else
                {
                    // Otherwise, decrement the count
                    itemsWithCounts[i] = (item, itemsWithCounts[i].count - 1);
                }
                break;
            }
        }
    }

    public int GetCount(T item)
    {
        foreach (var (storedItem, count) in itemsWithCounts)
        {
            if (EqualityComparer<T>.Default.Equals(storedItem, item))
            {
                return count;
            }
        }
        return 0; // Item not found
    }
}