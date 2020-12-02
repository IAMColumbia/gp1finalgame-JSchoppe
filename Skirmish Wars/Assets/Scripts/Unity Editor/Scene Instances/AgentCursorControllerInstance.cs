using UnityEngine;
using SkirmishWars.UnityRenderers;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for the player cursor controller.
    /// </summary>
    public sealed class AgentCursorControllerInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The cursor renderer that will be driven by this controller.")]
        [SerializeField] private CursorRenderer cursorRenderer = null;
        [Tooltip("Controls the speed of the agent cursor in tiles per second.")]
        [SerializeField] private float speed = 1f;
        private void OnValidate()
        {
            speed.Clamp(0.1f, float.MaxValue);
        }
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped agent cursor controller from the scene.
        /// </summary>
        /// <returns>The cursor controller instance (with no notion of monobehaviour).</returns>
        public AgentCursorController GetInstance(TileGrid grid)
        {
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            AgentCursorController newController =
                new AgentCursorController(grid, speed);
            cursorRenderer.DrivingController = newController;
            Destroy(this);
            return newController;
        }
        #endregion
    }
}
