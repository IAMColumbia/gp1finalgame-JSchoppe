using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Commander : MonoBehaviour
{
    protected virtual void OnCommandPhaseBegin() { }
    protected virtual void OnCommandPhaseEnd() { }

    protected virtual void OnClick() { }
    protected virtual void OnRelease() { }

    private IEnumerator WhileDragging()
    {
        throw new NotImplementedException();
    }

    public Team team;
    public TileGrid grid;
    public List<CombatUnit> units;
    public CursorController controller;
}