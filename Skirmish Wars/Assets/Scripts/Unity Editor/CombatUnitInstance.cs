using UnityEngine;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for combat units.
    /// </summary>
    public sealed class CombatUnitInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Controls how this unit acts towards other units.")]
        [SerializeField] private byte teamID = 0;
        [Tooltip("The initial spawning state of this unit.")]
        [SerializeField] private CombatUnitState initialState = new CombatUnitState();
        [Tooltip("The renderer that will observe the created combat unit.")]
        [SerializeField] private CombatUnitRenderer unitRenderer = null;
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped combat unit from the scene.
        /// </summary>
        /// <param name="grid">The grid to place this combat unit on.</param>
        /// <returns>The combat unit instance (with no notion of monobehaviour).</returns>
        public CombatUnit GetInstance(TileGrid grid)
        {
            CombatUnit newUnit = new CombatUnit(grid, teamID, initialState);
            // Snap this new unit onto the grid
            // and make the grid aware of them.
            newUnit.Location = grid.WorldToGrid(transform.position);
            grid.actors[newUnit.Location].Add(newUnit);
            // If there is an assigned unit renderer,
            // link it to observe this instance.
            if (unitRenderer != null)
                unitRenderer.DrivingUnit = newUnit;
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return newUnit;
        }
        #endregion
    }
}
