using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkirmishWars.UnityEditor;

namespace SkirmishWars.Tests
{
    /// <summary>
    /// Implements tests for the agent cursor.
    /// </summary>
    public sealed class AgentCursorTest : MonoBehaviour
    {
        #region Inspector References
        [SerializeField] private TileGridInstance gridInstance = null;
        [SerializeField] private AgentCommanderInstance agentCommanderInstance = null;
        [SerializeField] private TestType test = default;
        private enum TestType : byte
        {
            SimpleUnitMove
        }
        #endregion

        private void Awake()
        {
            TileGrid grid = gridInstance.GetInstance();

            UnitySceneParser parser = new UnitySceneParser();
            grid.Actors.AddRange(parser.GetAllPreplacedActors(grid));

            AgentCommander commander = agentCommanderInstance.GetInstance(grid, default);
            AgentCursorController agentCursor = (AgentCursorController)commander.controller;
            agentCursor.IsEnabled = true;
            switch (test)
            {
                case TestType.SimpleUnitMove: TestSimpleUnitMove(); break;
            }

            void TestSimpleUnitMove()
            {
                agentCursor.AddAction(new CursorAction
                {
                    path = grid.GridToWorld(new Vector2Int[]
                    {
                        new Vector2Int(1, 6),
                        new Vector2Int(1, 5),
                        new Vector2Int(2, 5),
                        new Vector2Int(2, 4)
                    }),
                    holdsClick = true
                }, OrderPriority.Queued);
            }
        }
    }
}
