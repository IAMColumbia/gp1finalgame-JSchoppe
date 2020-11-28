using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkirmishWars.UnityEditor
{
    /// <summary>
    /// Inspector wrapper for the tile grid.
    /// </summary>
    public sealed class TileGridInstance : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The number of horizontal tiles.")]
        [SerializeField] private int width = 5;
        [Tooltip("The number of vertical tiles.")]
        [SerializeField] private int height = 5;
        [Tooltip("Width of one tile in unity units.")]
        [SerializeField] private float gridUnit = 1f;
        private void OnValidate()
        {
            width.Clamp(1, int.MaxValue);
            height.Clamp(1, int.MaxValue);
            gridUnit.Clamp(0.1f, float.MaxValue);
        }
        #endregion
        #region Inspector Gizmos
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            // Draw the grid into the scene view for reference.
            Vector3 offset = height * Vector3.up * gridUnit;
            for (int x = 0; x <= width; x++)
            {
                Vector3 along = transform.position + Vector3.right * x * gridUnit;
                Debug.DrawLine(along, along + offset);
            }
            offset = width * Vector3.right * gridUnit;
            for (int y = 0; y <= height; y++)
            {
                Vector3 along = transform.position + Vector3.up * y * gridUnit;
                Debug.DrawLine(along, along + offset);
            }
        }
        #endregion
        #region Retrieval Method
        /// <summary>
        /// Retrieves the unwrapped tilegrid from the scene.
        /// </summary>
        /// <returns>The tile grid instance (with no notion of monobehaviour).</returns>
        public TileGrid GetInstance()
        {
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return new TileGrid(
                new Vector2Int
                {
                    x = width,
                    y = height
                },
                gridUnit,
                transform.position
            );
        }
        #endregion
    }
}
