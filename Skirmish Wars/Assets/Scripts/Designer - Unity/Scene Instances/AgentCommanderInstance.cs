using UnityEngine;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for agent commanders.
    /// </summary>
    public sealed class AgentCommanderInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Controls how this commander acts towards other commanders.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("The cursor controller for this agent.")]
        [SerializeField] private AgentCursorControllerInstance controller = null;
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped agent commander from the scene.
        /// </summary>
        /// <returns>The tile grid instance (with no notion of monobehaviour).</returns>
        public AgentCommander GetInstance(TileGrid grid)
        {
            // Create the new commander and add it to the grid.
            AgentCommander commander =
                new AgentCommander(teamID, grid, controller.GetInstance(grid));
            grid.Commanders.Add(commander);
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return commander;
        }
        #endregion
    }
}
