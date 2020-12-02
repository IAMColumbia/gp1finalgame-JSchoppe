using UnityEngine;

namespace SkirmishWars.UnityRenderers
{
    /// <summary>
    /// Implements a combat unit renderer that
    /// observes a combat unit instance.
    /// </summary>
    public sealed class CombatUnitRenderer : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The renderer used to render the unit.")]
        [SerializeField] private SpriteRenderer unitSprite = null;
        [Tooltip("The renderer used to render the hitpoints.")]
        [SerializeField] private SpriteRenderer hitpointsSprite = null;
        [Tooltip("The collection of sprites for the hitpoint numbering.")]
        [SerializeField] private NumberSpriteSet numberSpriteSet;
        [Tooltip("The sprite chain used to draw the unit path.")]
        [SerializeField] private SpriteChainRenderer movementChain = null;
        #endregion
        #region State Fields
        private Vector2[] worldPath;
        private CombatUnit drivingUnit;
        #endregion
        #region Observer Implementation
        /// <summary>
        /// The combat unit that this renderer is observing.
        /// </summary>
        public CombatUnit DrivingUnit
        {
            set
            {
                // Unbind from previous combat unit.
                if (drivingUnit != null)
                    RemoveListeners(drivingUnit);
                // Bind to new combat unit.
                drivingUnit = value;
                AddListeners(drivingUnit);
                // Initialize visual properties.
                OnHitPointsChanged(drivingUnit.hitPoints);
                OnTeamChanged(TeamsSingleton.FromID(drivingUnit.TeamID));
                // TODO intialization of more properties may
                // be needed to use object pooling.
            }
        }
        private void AddListeners(CombatUnit dispatcher)
        {
            dispatcher.PathChanged += OnPathChanged;
            dispatcher.TeamChanged += OnTeamChanged;
            dispatcher.Teleported += OnTeleported;
            dispatcher.MovementAnimating += OnMovementAnimating;
            dispatcher.HitPointsChanged += OnHitPointsChanged;
        }
        private void RemoveListeners(CombatUnit dispatcher)
        {
            dispatcher.PathChanged -= OnPathChanged;
            dispatcher.TeamChanged -= OnTeamChanged;
            dispatcher.Teleported -= OnTeleported;
            dispatcher.MovementAnimating -= OnMovementAnimating;
            dispatcher.HitPointsChanged -= OnHitPointsChanged;
        }
        #endregion
        #region Observer Listeners
        private void OnHitPointsChanged(float newHitPoints)
        {
            // If the new hit points are less than zero,
            // then this unit should be destroyed.
            if (newHitPoints < 0f)
            {
                RemoveListeners(drivingUnit);
                // TODO may want to implement object pool here.
                Destroy(gameObject);
                Destroy(movementChain.gameObject);
            }
            // Round up for the unit hitpoints.
            int hpNumber = Mathf.CeilToInt(newHitPoints * 10f);
            if (hpNumber > 9)
                hitpointsSprite.enabled = false;
            else
            {
                hitpointsSprite.enabled = true;
                // Update the hitpoint number displayed.
                hitpointsSprite.sprite = numberSpriteSet[hpNumber];
            }
        }
        private void OnTeamChanged(Team team)
        {
            // Set the visual properties of this unit.
            unitSprite.color = team.style.baseColor;
            unitSprite.flipX = team.style.flipX;
        }
        private void OnTeleported(Vector2Int newLocation)
        {
            // Move the transform to the new tile location.
            transform.position = drivingUnit.Grid.GridToWorld(newLocation);
        }
        private void OnPathChanged(Vector2Int[] newPath)
        {
            // Update the local world path and the
            // linked movement chain.
            worldPath = drivingUnit.Grid.GridToWorld(newPath);
            movementChain.Chain = worldPath;
        }
        private void OnMovementAnimating(float interpolant)
        {
            // Animate towards the next tile along the observed
            // units path.
            unitSprite.transform.position = 
                Vector2.Lerp(worldPath[0], worldPath[1], interpolant);
        }
        #endregion
    }
}
