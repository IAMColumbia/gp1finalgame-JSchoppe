using UnityEngine;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for player commanders.
    /// </summary>
    public sealed class PlayerCommanderInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Controls how this commander acts towards other commanders.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("The cursor controller for this commander.")]
        [SerializeField] private PlayerCursorControllerInstance controller = null;
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped tilegrid from the scene.
        /// </summary>
        /// <returns>The tile grid instance (with no notion of monobehaviour).</returns>
        public PlayerCommander GetInstance(TileGrid grid)
        {
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return new PlayerCommander(teamID, grid, controller.GetInstance(grid));
        }
        #endregion
    }
}
