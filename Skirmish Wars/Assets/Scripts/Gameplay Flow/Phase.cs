using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Phase : MonoBehaviour
{
    public Grid grid;

    public abstract event Action Completed;
    public abstract void Begin();
}