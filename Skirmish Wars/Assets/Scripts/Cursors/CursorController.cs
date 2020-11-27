using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Base class for cursor controllers.
/// </summary>
public abstract class CursorController : MonoBehaviour
{
    /// <summary>
    /// Called when this cursor clicks.
    /// </summary>
    public abstract event Action<Vector2> Clicked;

    public event Action<Vector2> Drag;
    /// <summary>
    /// Called when this cursor releases.
    /// </summary>
    public abstract event Action<Vector2> Released;

    [SerializeField] protected TileGrid grid = null;


    [SerializeField] protected CursorRenderer cursor = null;

    protected virtual void Start()
    {
        Clicked += OnClick;
        Released += OnRelease;
    }

    private Coroutine dragRoutine;
    private void OnClick(Vector2 location)
    {
        cursor.RenderState = RenderedCursorState.Held;
        dragRoutine = StartCoroutine(OnDragUpdate());
    }

    private void OnRelease(Vector2 location)
    {
        StopCoroutine(dragRoutine);
        cursor.RenderState = RenderedCursorState.Active;
    }

    private IEnumerator OnDragUpdate()
    {
        while (true)
        {
            yield return null;
            Drag?.Invoke(cursor.Location);
        }
    }

    protected virtual void Update()
    {
        // Lock the tile highlight effect to the nearest grid tile.
        cursor.TileLocation =
            grid.GridToWorld(grid.WorldToGrid(cursor.Location));
    }
}