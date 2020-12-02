using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkirmishWars.Unity
{
    /// <summary>
    /// Mouse listener implemented using the new Unity Input System.
    /// </summary>
    public sealed class MouseListener : MonoBehaviour
    {
        #region Exposed Events
        /// <summary>
        /// Called when this mouse listener registers a click.
        /// </summary>
        public event Action<Vector2> Clicked;
        /// <summary>
        /// Called when this mouse listener registers a release.
        /// </summary>
        public event Action<Vector2> Released;
        #endregion
        #region Exposed Properties
        /// <summary>
        /// The current screen space coordinates of the mouse.
        /// </summary>
        public Vector2 ScreenLocation { get; private set; }
        #endregion
        #region New Input Implementation
        private void Update()
        {
            ScreenLocation = Mouse.current.position.ReadValue();
        }
        public void OnPrimaryMouseButtonAction(InputAction.CallbackContext context)
        {
            // Listen for mouse button press and release.
            if (context.ReadValueAsButton() && !context.performed)
                Clicked?.Invoke(ScreenLocation);
            else if (!context.ReadValueAsButton())
                Released?.Invoke(ScreenLocation);
        }
        #endregion
    }
}
