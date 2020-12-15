using System;
using System.Collections.Generic;
using UnityEngine; // Only used for common random function.

/// <summary>
/// Provides extension methods for generic collections.
/// </summary>
public static class GenericCollectionExtensions
{
    /// <summary>
    /// Retrieves a random element from this collection.
    /// </summary>
    /// <param name="collection">The collection to pull from.</param>
    /// <returns>A random element from the collection.</returns>
    /// <exception cref="ArgumentException">Throws when the collection is empty.</exception>
    public static T RandomElement<T>(this IList<T> collection)
    {
        if (collection.Count == 0)
            throw new ArgumentException(
                "Random Element cannot be performed on an empty collection!");
        // Pull a random element from this collection.
        int i = UnityEngine.Random.Range(0, collection.Count);
        return collection[i];
    }
}
