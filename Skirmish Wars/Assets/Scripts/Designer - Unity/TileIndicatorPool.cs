using UnityEngine;

// TODO abstract this away from monobehaviour.
// TODO abstract a base class for a generic pool.

/// <summary>
/// Manages a collection of tile indicators.
/// </summary>
public sealed class TileIndicatorPool : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("The prefab used to display a tile indicator.")]
    [SerializeField] private GameObject tileIndicatorPrefab = null;
    [Tooltip("The total number of indicators in the pool.")]
    [SerializeField] private int poolSize = 30;
    private void OnValidate()
    {
        poolSize.Clamp(1, int.MaxValue);
    }
    #endregion
    #region Private Fields
    private SpriteRenderer[] pool;
    #endregion
    #region Pool Initialization
    private void Awake()
    {
        pool = new SpriteRenderer[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(tileIndicatorPrefab).GetComponent<SpriteRenderer>();
            pool[i].transform.parent = transform;
        }
        ClearIndicators();
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Disables rendering for all indicators in the pool.
    /// </summary>
    public void ClearIndicators()
    {
        foreach (SpriteRenderer renderer in pool)
            renderer.enabled = false;
    }
    /// <summary>
    /// Sets the locations of the indicators in the pool.
    /// </summary>
    /// <param name="locations">The grid tiles to place the indicators on.</param>
    /// <param name="ontoGrid">The grid to place relative to.</param>
    public void SetIndicators(Vector2Int[] locations, TileGrid ontoGrid)
    {
        // Render as many requested locations as possible.
        int i;
        for (i = 0; i < locations.Length; i++)
        {
            if (i > pool.Length - 1)
                break;
            pool[i].transform.position = ontoGrid.GridToWorld(locations[i]);
            pool[i].enabled = true;
        }
        // If the load was under the pool size make sure
        // any remaining renderers in the pool are disabled.
        while (i < pool.Length)
        {
            pool[i].enabled = false;
            i++;
        }
    }
    #endregion
}
