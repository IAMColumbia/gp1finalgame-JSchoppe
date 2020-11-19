using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public event Action Clicked;
    public event Action Released;

    public CursorRenderer cursor;

    protected virtual void Start()
    {
        throw new NotImplementedException();
    }

    protected virtual void Update()
    {
        throw new NotImplementedException();
    }
}