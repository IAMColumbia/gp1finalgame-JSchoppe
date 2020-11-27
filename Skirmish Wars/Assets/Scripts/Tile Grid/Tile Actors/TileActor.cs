using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class TileActor : MonoBehaviour
{

    [SerializeField] private TileGrid grid = null;
    [SerializeField] private Team team = Team.Alpha;

    public TileGrid Grid { get { return grid; } }

    public Team Team { get { return team; } }


    public virtual void OnClick()
    {
        
    }
    public virtual void OnDragNewTile(Vector2Int newTile)
    {
        
    }
    public virtual void OnRelease(Vector2Int finalTile)
    {

    }
}
