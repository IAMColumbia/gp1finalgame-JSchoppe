using System.Collections.Generic;
using UnityEngine;
using SkirmishWars.UnityEditor;

namespace SkirmishWars.Tests
{
    public sealed class AttackPhaseTest : MonoBehaviour
    {
        private enum TestType : byte
        {
            NonIntersectingMovement,
            EqualConfrontation,
            EqualConfrontationThreeTeam,
            FavoredConfrontation,
            HitPointAdvantage,
            UnitDefeated
        }

        [SerializeField] private GameObject infantryPrefab = null;

        [SerializeField] private AttackPhase phase = null;

        [SerializeField] private TileGridInstance gridInstance = null;
        [SerializeField] private DamageTableInstance damageTable = null;
        [SerializeField] private TestType test = TestType.NonIntersectingMovement;

        private void Start()
        {
            TileGrid grid = gridInstance.GetInstance();
            DamageTable table = damageTable.GetInstance();
            phase.grid = grid;
            phase.damageTable = table;

            phase.Completed += () => { Debug.Log("Phase Complete"); };

            switch (test)
            {
                case TestType.NonIntersectingMovement: TestNonIntersecting(); break;
                case TestType.EqualConfrontation: TestEqualConfrontation(); break;
                case TestType.EqualConfrontationThreeTeam: TestThreeTeam(); break;
                case TestType.FavoredConfrontation: TestFavored(); break;
                case TestType.HitPointAdvantage: TestHitPointAdvantage(); break;
                case TestType.UnitDefeated: TestUnitDefeated(); break;
            }
            phase.Begin();

            void TestNonIntersecting()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(1, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(1, 1));
                soldierAPath.AddLast(new Vector2Int(1, 2));
                soldierAPath.AddLast(new Vector2Int(2, 2));
                soldierAPath.AddLast(new Vector2Int(2, 3));
                soldierAPath.AddLast(new Vector2Int(2, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 4);
                soldierB.TeamID = 1;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierBPath.AddLast(new Vector2Int(3, 1));
                soldierB.MovePath = soldierBPath;
            }
            void TestEqualConfrontation()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(3, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(3, 1));
                soldierAPath.AddLast(new Vector2Int(3, 2));
                soldierAPath.AddLast(new Vector2Int(3, 3));
                soldierAPath.AddLast(new Vector2Int(3, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 5);
                soldierB.TeamID = 1;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 5));
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierB.MovePath = soldierBPath;
            }
            void TestHitPointAdvantage()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(3, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(3, 1));
                soldierAPath.AddLast(new Vector2Int(3, 2));
                soldierAPath.AddLast(new Vector2Int(3, 3));
                soldierAPath.AddLast(new Vector2Int(3, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 5);
                soldierB.TeamID = 1;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 5));
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierB.HitPoints = 0.5f;
                soldierB.MovePath = soldierBPath;
            }
            void TestThreeTeam()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(3, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(3, 1));
                soldierAPath.AddLast(new Vector2Int(3, 2));
                soldierAPath.AddLast(new Vector2Int(3, 3));
                soldierAPath.AddLast(new Vector2Int(3, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 5);
                soldierB.TeamID = 1;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 5));
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierB.MovePath = soldierBPath;

                CombatUnit soldierC =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierC.Location = new Vector2Int(6, 5);
                soldierC.TeamID = 2;
                LinkedList<Vector2Int> soldierCPath = new LinkedList<Vector2Int>();
                soldierCPath.AddLast(new Vector2Int(5, 3));
                soldierCPath.AddLast(new Vector2Int(4, 3));
                soldierCPath.AddLast(new Vector2Int(3, 3));
                soldierCPath.AddLast(new Vector2Int(2, 3));
                soldierC.MovePath = soldierCPath;
            }
            void TestFavored()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(3, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(3, 1));
                soldierAPath.AddLast(new Vector2Int(3, 2));
                soldierAPath.AddLast(new Vector2Int(3, 3));
                soldierAPath.AddLast(new Vector2Int(3, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 5);
                soldierB.TeamID = 0;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 5));
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierB.MovePath = soldierBPath;

                CombatUnit soldierC =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierC.Location = new Vector2Int(6, 5);
                soldierC.TeamID = 1;
                LinkedList<Vector2Int> soldierCPath = new LinkedList<Vector2Int>();
                soldierCPath.AddLast(new Vector2Int(5, 3));
                soldierCPath.AddLast(new Vector2Int(4, 3));
                soldierCPath.AddLast(new Vector2Int(3, 3));
                soldierCPath.AddLast(new Vector2Int(2, 3));
                soldierC.MovePath = soldierCPath;
            }
            void TestUnitDefeated()
            {
                CombatUnit soldierA =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierA.Location = new Vector2Int(3, 1);
                soldierA.TeamID = 0;
                LinkedList<Vector2Int> soldierAPath = new LinkedList<Vector2Int>();
                soldierAPath.AddLast(new Vector2Int(3, 1));
                soldierAPath.AddLast(new Vector2Int(3, 2));
                soldierAPath.AddLast(new Vector2Int(3, 3));
                soldierAPath.AddLast(new Vector2Int(3, 4));
                soldierA.MovePath = soldierAPath;

                CombatUnit soldierB =
                    Instantiate(infantryPrefab).GetComponent<CombatUnitInstance>().GetInstance(grid);
                soldierB.Location = new Vector2Int(3, 5);
                soldierB.TeamID = 1;
                LinkedList<Vector2Int> soldierBPath = new LinkedList<Vector2Int>();
                soldierBPath.AddLast(new Vector2Int(3, 5));
                soldierBPath.AddLast(new Vector2Int(3, 4));
                soldierBPath.AddLast(new Vector2Int(3, 3));
                soldierBPath.AddLast(new Vector2Int(3, 2));
                soldierB.HitPoints = 0.2f;
                soldierB.MovePath = soldierBPath;
            }
        }
    }
}
