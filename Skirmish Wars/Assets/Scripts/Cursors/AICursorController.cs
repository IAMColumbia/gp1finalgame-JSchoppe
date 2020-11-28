using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Priority
{
    High, Low
}

public class AICursorController : CursorController
{
    public AICursorController(TileGrid grid)
        : base(grid)
    {

    }

    public float speed;
    public bool isClicked;

    public List<CursorAction> actions;

    public override event Action<Vector2> Clicked;
    public override event Action<Vector2> Released;

    public void ClearActions()
    {
        throw new NotImplementedException();
    }

    public void AddAction(CursorAction action, Priority priority)
    {
        throw new NotImplementedException();
    }

}