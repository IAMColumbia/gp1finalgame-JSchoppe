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
        /// Called when this mouse listener registers a click on LMB.
        /// </summary>
        public event Action<Vector2> LeftClicked;
        /// <summary>
        /// Called when this mouse listener registers a release on LMB.
        /// </summary>
        public event Action<Vector2> LeftReleased;
        /// <summary>
        /// Called when this mouse listener registers a click on RMB.
        /// </summary>
        public event Action<Vector2> RightClicked;
        /// <summary>
        /// Called when this mouse listener registers a release on RMB.
        /// </summary>
        public event Action<Vector2> RightReleased;
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
            // Listen for left mouse button press and release.
            if (context.ReadValueAsButton() && !context.performed)
                LeftClicked?.Invoke(ScreenLocation);
            else if (!context.ReadValueAsButton())
                LeftReleased?.Invoke(ScreenLocation);
        }
        public void OnSecondaryMouseButtonAction(InputAction.CallbackContext context)
        {
            // Listen for right mouse button press and release.
            if (context.ReadValueAsButton() && !context.performed)
                RightClicked?.Invoke(ScreenLocation);
            else if (!context.ReadValueAsButton())
                RightReleased?.Invoke(ScreenLocation);
        }
        #endregion
    }
}
