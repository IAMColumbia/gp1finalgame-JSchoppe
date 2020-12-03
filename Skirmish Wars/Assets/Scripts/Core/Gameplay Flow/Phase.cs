using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// TODO remove monobehaviour dependency.

public abstract class Phase : MonoBehaviour
{
    // TODO when monobehaviour is pryed off,
    // move this to the constructor.
    public TileGrid grid;

    public abstract event Action Completed;
    public abstract void Begin();
}