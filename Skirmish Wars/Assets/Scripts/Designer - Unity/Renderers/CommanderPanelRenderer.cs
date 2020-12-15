using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkirmishWars.UnityRenderers
{
    public sealed class CommanderPanelRenderer : MonoBehaviour
    {
        [SerializeField] private Image panelBackground = null;
        [SerializeField] private Image unitCountUnit = null;
        [SerializeField] private Text unitCountText = null;

        private Commander drivingCommander;
        public Commander DrivingCommander
        {
            set
            {
                drivingCommander = value;
                Team newTeam = TeamsSingleton.FromID(drivingCommander.teamID);
                panelBackground.color = newTeam.style.baseColor;
                unitCountUnit.color = newTeam.style.baseColor;
            }
        }

        private void Update()
        {
            // TODO relocate this to a listener/dispatcher.
            unitCountText.text = $"x {drivingCommander.units.Count}";
        }

    }
}
