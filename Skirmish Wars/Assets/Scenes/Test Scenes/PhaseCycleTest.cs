using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkirmishWars.Tests
{
    public sealed class PhaseCycleTest : MonoBehaviour
    {
        [SerializeField] private PhaseManager phaseManager = null;

        private TileGrid grid;
        private DamageTable damageTable;

        private void Awake()
        {
            IDesignerParser parser = new UnitySceneParser();
            grid = parser.GetFirstTileGrid();

            damageTable = parser.GetFirstDamageTable();

            parser.GetAllPreplacedActors(grid);
            parser.GetAllPreplacedCommanders(grid, damageTable);

            StartCoroutine(WaitInitThenStart());
        }

        private IEnumerator WaitInitThenStart()
        {
            yield return new WaitForEndOfFrame();
            phaseManager.StartCycle(grid);
        }
    }
}
