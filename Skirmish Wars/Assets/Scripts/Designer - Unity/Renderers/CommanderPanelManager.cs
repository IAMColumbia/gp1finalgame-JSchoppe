using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkirmishWars.UnityRenderers
{
    public sealed class CommanderPanelManager : MonoBehaviour
    {
        [SerializeField] private CommanderPanelRenderer templatePanel = null;


        public void InitializeCommanders(IList<Commander> commanders)
        {
            Transform parent = templatePanel.transform.parent;
            CommanderPanelRenderer current = templatePanel;
            float verticalTravel = templatePanel.GetComponent<RectTransform>().rect.height;
            for (int i = 0; i < commanders.Count; i++)
            {
                if (i != 0)
                {
                    current = Instantiate(templatePanel.gameObject, parent).GetComponent<CommanderPanelRenderer>();
                    current.GetComponent<RectTransform>().anchoredPosition += Vector2.down * i * verticalTravel;
                }
                current.DrivingCommander = commanders[i];
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
