using UnityEngine;
using SkirmishWars.UnityEditor;

namespace SkirmishWars.Tests
{
    public sealed class PlayerCursorTest : MonoBehaviour
    {
        [SerializeField] private TileGridInstance gridInstance = null;

        private void Start()
        {
            TileGrid grid = gridInstance.GetInstance();

            IDesignerParser parser = new UnitySceneParser();

            parser.GetAllPreplacedActors(grid);

            parser.GetAllPreplacedCommanders(grid);

        }
    }
}
