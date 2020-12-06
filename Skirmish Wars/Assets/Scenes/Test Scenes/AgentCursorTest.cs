using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkirmishWars.UnityEditor;

namespace SkirmishWars.Tests
{
    public sealed class AgentCursorTest : MonoBehaviour
    {

        private enum TestType : byte
        {
            SimpleUnitMove
        }
        [SerializeField] private TestType test = TestType.SimpleUnitMove;
        [SerializeField] private TileGridInstance gridInstance = null;
        [SerializeField] private AgentCommanderInstance agentCommanderInstance = null;

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
