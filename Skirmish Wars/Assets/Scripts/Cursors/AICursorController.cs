using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public enum Priority
{
    High, Low
}

public class AICursorController : CursorController
{
    public float speed;
    public bool isClicked;

    public List<CursorAction> actions;

    public void ClearActions()
    {
        throw new NotImplementedException();
    }

    public void AddAction(CursorAction action, Priority priority)
    {
        throw new NotImplementedException();
    }

    protected override void Update()
    {
        
    }

}