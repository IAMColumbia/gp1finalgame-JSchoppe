using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class TileActor
{
    public TileGrid grid;

    public Team team;

    public virtual void OnClick() { throw new NotImplementedException(); }
    public virtual void OnDragNewTile(Vector2Int newTile) { throw new NotImplementedException(); }
    public virtual void OnRelease(Vector2Int finalTile) { throw new NotImplementedException(); }
}