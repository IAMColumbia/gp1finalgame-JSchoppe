using System;
using System.Collections.Generic;

/// <summary>
/// Provides extension methods for the LinkedList collection type.
/// </summary>
public static class LinkedListExtensions
{
    /// <summary>
    /// Removes elements from the end of the list until fromElement is reached.
    /// </summary>
    /// <param name="list">The linked list to truncate.</param>
    /// <param name="toElement">The value or reference to truncate towards.</param>
    public static void Truncate<T>(this LinkedList<T> list, T toElement)
        where T : IEquatable<T>
    {
        while (list.Count > 0 && !list.Last.Value.Equals(toElement))
            list.RemoveLast();
    }
}
