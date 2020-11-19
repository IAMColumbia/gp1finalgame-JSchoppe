using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class SpriteChainRenderer : MonoBehaviour
{
    public ChainSpriteSet sprites;
    public SpriteRenderer[] renderers;

    public TileGrid grid;
    public Vector2Int[] Chain { get; set; }

    public bool IsVisible { get; set; }
}