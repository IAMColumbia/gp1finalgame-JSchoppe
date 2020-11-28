using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class PlayerCommander : Commander
{
    public PlayerCommander(byte teamID, TileGrid grid, CursorController controller)
        : base(teamID, grid, controller)
    {

    }


    private void Update()
    {
        
    }

    private void ToggleSpying()
    {

    }

    private void TogglePause()
    {

    }
}