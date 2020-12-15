using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkirmishWars.Unity
{
    /// <summary>
    /// Buttons listener implemented using the new Unity Input System.
    /// </summary>
    public sealed class CommanderButtonsListener : MonoBehaviour
    {
        #region Exposed Events
        /// <summary>
        /// Called when this buttons listener registers a pause button press.
        /// </summary>
        public event Action PausePressed;
        /// <summary>
        /// Called when this buttons listener registers an exit button press.
        /// </summary>
        public event Action ExitPressed;
        #endregion
        #region New Input Implementation
        public void OnPauseButtonAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() && !context.performed)
                PausePressed?.Invoke();
        }
        public void OnExitButtonAction(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() && !context.performed)
                ExitPressed?.Invoke();
        }
        #endregion
    }
}
