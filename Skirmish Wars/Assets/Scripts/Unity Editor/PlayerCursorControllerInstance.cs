using UnityEngine;
using SkirmishWars.Unity;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for the player cursor controller.
    /// </summary>
    public sealed class PlayerCursorControllerInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The cursor renderer that will be driven by this controller.")]
        [SerializeField] private CursorRenderer cursorRenderer = null;
        [Tooltip("Defines the screen space that the cursor resides in.")]
        [SerializeField] private Camera holdingCamera = null;
        [Tooltip("The script that listen for mouse events.")]
        [SerializeField] private MouseListener listener = null;
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped player cursor controller from the scene.
        /// </summary>
        /// <returns>The cursor controller instance (with no notion of monobehaviour).</returns>
        public PlayerCursorController GetInstance(TileGrid grid)
        {
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            PlayerCursorController newController =
                new PlayerCursorController(grid, listener, holdingCamera);
            cursorRenderer.DrivingController = newController;
            Destroy(this);
            return newController;
        }
        #endregion
    }
}
