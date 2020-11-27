using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Cursor controller that follows the mouse device.
/// </summary>
public sealed class PlayerCursorController : CursorController
{
    [Tooltip("The camera that holds the cursor.")]
    [SerializeField] private Camera playerCamera = null;

    public override event Action<Vector2> Clicked;
    public override event Action<Vector2> Released;

    protected override void Update()
    {
        // Retrieve the mouse position
        // using the new input system.
        cursor.Location = 
            playerCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        base.Update();
    }

    public void OnPrimaryMouseButtonAction(InputAction.CallbackContext context)
    {
        // Listen for mouse button press and release.
        if (context.ReadValueAsButton() && !context.performed)
            Clicked?.Invoke(cursor.Location);
        else if (!context.ReadValueAsButton())
            Released?.Invoke(cursor.Location);
    }
}
