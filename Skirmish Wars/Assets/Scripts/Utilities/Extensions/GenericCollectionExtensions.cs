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
    public static T RandomElement<T>(this IList<T> collection)
    {
        // TODO returning default(T) may make this
        // bad for debugging certian types.
        if (collection.Count == 0)
            return default;
        else
        {
            int i = Random.Range(0, collection.Count);
            return collection[i];
        }
    }
}
