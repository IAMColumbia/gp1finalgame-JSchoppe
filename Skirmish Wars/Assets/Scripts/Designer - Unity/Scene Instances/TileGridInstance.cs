using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [Header("Terrain Definition")]
        [Tooltip("The tilemap that the terrain exists on.")]
        [SerializeField] private Tilemap terrainMap = null;
        [Tooltip("Defines how the tilemap is processed into terrain.")]
        [SerializeField] private SpriteInteractionsPair[] terrainData = null;
        [Serializable]
        private class SpriteInteractionsPair
        {
            public Sprite whenSpriteUsed;
            public UnitInteraction[] interactions;
        }
        [Serializable]
        private struct UnitInteraction
        {
            public UnitType unitType;
            public UnitTerrainData interactionData;
        }
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
            // Create the base grid with the inspector arguments.
            TileGrid grid = new TileGrid(
                new Vector2Int
                {
                    x = width,
                    y = height
                },
                gridUnit,
                transform.position
            );
            // Fill in the terrain data based on the sprites
            // in the tilemap.
            Sprite spriteAtCoords;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    spriteAtCoords = terrainMap.GetSprite(new Vector3Int(x, y, 0));
                    // Find the corresponding sprite.
                    // TODO this is slow but only runs once so I'm not super concerned.
                    foreach (SpriteInteractionsPair pair in terrainData)
                    {
                        if (pair.whenSpriteUsed == spriteAtCoords)
                        {
                            // Process the inspector data into a dictionary.
                            Dictionary<UnitType, UnitTerrainData> parsedData
                                = new Dictionary<UnitType, UnitTerrainData>();
                            foreach (UnitInteraction interaction in pair.interactions)
                                parsedData[interaction.unitType] = interaction.interactionData;
                            // Use the parsed data to fill in accessible terrain data.
                            // TODO I think this is creating redundant instances of TileTerrain.
                            // Maybe refactor this so that the Terrain dictionary points to
                            // single instances instead of identical copy instances.
                            grid.Terrain[new Vector2Int(x, y)]
                                = new TileTerrain(parsedData);
                            break;
                        }
                    }
                }
            }
            // Destroy this script's Monobehaviour baggage,
            // and return the lightweight instance.
            Destroy(this);
            return grid;
        }
        #endregion
    }
}
