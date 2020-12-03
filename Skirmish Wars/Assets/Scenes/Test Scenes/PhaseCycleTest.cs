using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkirmishWars.Tests
{
    public sealed class PhaseCycleTest : MonoBehaviour
    {
        [SerializeField] private PhaseManager phaseManager = null;

        private TileGrid grid;

        private void Awake()
        {
            IDesignerParser parser = new UnitySceneParser();
            grid = parser.GetFirstTileGrid();

            parser.GetAllPreplacedActors(grid);
            parser.GetAllPreplacedCommanders(grid);

            StartCoroutine(WaitInitThenStart());
        }

        private IEnumerator WaitInitThenStart()
        {
            yield return new WaitForEndOfFrame();
            phaseManager.StartCycle(grid);
        }
    }
}
