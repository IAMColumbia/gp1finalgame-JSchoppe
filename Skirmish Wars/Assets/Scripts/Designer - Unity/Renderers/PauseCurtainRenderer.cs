using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkirmishWars.UnityRenderers
{

    public sealed class PauseCurtainRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject curtainObject = null;

        private PlayerCommander drivingCommander;
        public PlayerCommander DrivingCommander
        {
            set
            {
                if (drivingCommander != null)
                    drivingCommander.PauseStateChanged -= OnPauseChanged;
                drivingCommander = value;
                drivingCommander.PauseStateChanged += OnPauseChanged;
            }
        }

        public void OnPauseChanged(bool isPaused)
        {
            curtainObject.SetActive(isPaused);
        }
    }
}
