using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CursorRenderer : MonoBehaviour
{

    public SpriteRenderer cursorSprite;
    public SpriteRenderer highlightSprite;

    public Vector2Int TileLocation { get { throw new NotImplementedException(); } }
    public Vector2 RawLocation
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public void Update() { throw new NotImplementedException(); }
}