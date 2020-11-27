using UnityEngine;

public sealed class PlayerCursorTest : MonoBehaviour
{
    [SerializeField] private TileGrid grid = null;

    private void Start()
    {
        grid.ParseSceneUnitsOntoGrid();
    }
}
