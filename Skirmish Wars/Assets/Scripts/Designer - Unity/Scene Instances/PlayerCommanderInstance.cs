using UnityEngine;
using SkirmishWars.Unity;
using SkirmishWars.UnityRenderers;

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
        [Tooltip("The listener for remaining buttom commands.")]
        [SerializeField] private CommanderButtonsListener buttonsListener = null;
        [Tooltip("The pause curtain for this player.")]
        [SerializeField] private PauseCurtainRenderer pauseCurtain = null;
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped player commander from the scene.
        /// </summary>
        /// <returns>The tile grid instance (with no notion of monobehaviour).</returns>
        public PlayerCommander GetInstance(TileGrid grid)
        {
            // Create the new commander and add it to the grid.
            PlayerCommander commander = 
                new PlayerCommander(teamID, grid, controller.GetInstance(grid), buttonsListener);
            grid.Commanders.Add(commander);
            // Bind the pause curtain to the new player.
            // TODO this approach does not support multiple human players.
            pauseCurtain.DrivingCommander = commander;
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return commander;
        }
        #endregion
    }
}
