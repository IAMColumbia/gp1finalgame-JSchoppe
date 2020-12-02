using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// TODO holy **** this is crappy.
// Refactor this for the love of god please.

namespace SkirmishWars.UnityRenderers
{
    /// <summary>
    /// Draws a series of directional sprites along a path.
    /// </summary>
    public sealed class SpriteChainRenderer : MonoBehaviour
    {
        [SerializeField] private ChainSpriteSet spriteSet = new ChainSpriteSet();
        [SerializeField] private GameObject spritePrefab = null;

        private List<SpriteRenderer> renderers;

        private int chainLength;

        private void OnDestroy()
        {
            // TODO may want to use object pooling here.
            foreach (SpriteRenderer renderer in renderers)
                Destroy(renderer.gameObject);
        }

        private void Awake()
        {
            renderers = new List<SpriteRenderer>();
            chainLength = 0;
        }

        public Vector2[] Chain
        {
            set
            {
                chainLength = value.Length;
                if (chainLength > 1)
                {
                    while (renderers.Count < chainLength)
                    {
                        GameObject newPrefab = Instantiate(spritePrefab);
                        newPrefab.transform.parent = transform;
                        renderers.Add(newPrefab.GetComponent<SpriteRenderer>());
                    }
                    int i;
                    for (i = 0; i < chainLength; i++)
                    {
                        renderers[i].enabled = true;
                        renderers[i].transform.position = value[i];
                    }
                    for (i = 0; i < chainLength; i++)
                    {
                        if (i == 0)
                        {
                            renderers[i].sprite = spriteSet.start;
                            renderers[i].transform.eulerAngles = Vector3.forward *
                                VectorToOrthoAngle(renderers[1].transform.position - renderers[0].transform.position);
                        }
                        else if (i == chainLength - 1)
                        {
                            renderers[i].sprite = spriteSet.end;
                            renderers[i].transform.eulerAngles = Vector3.forward *
                                VectorToOrthoAngle(renderers[i].transform.position - renderers[i - 1].transform.position);
                        }
                        else
                        {
                            Vector2 incomingDirection =
                                renderers[i].transform.position - renderers[i - 1].transform.position;
                            Vector2 outgoingDirection =
                                renderers[i + 1].transform.position - renderers[i].transform.position;

                            float incomingAngle = VectorToOrthoAngle(incomingDirection);

                            renderers[i].transform.eulerAngles = Vector3.forward * incomingAngle;

                            if (incomingDirection.Equals(outgoingDirection))
                                renderers[i].sprite = spriteSet.straight;
                            else if (incomingDirection.x > 0f)
                            {
                                if (outgoingDirection.y > 0f)
                                    renderers[i].sprite = spriteSet.left;
                                else
                                    renderers[i].sprite = spriteSet.right;
                            }
                            else if (incomingDirection.x < 0f)
                            {
                                if (outgoingDirection.y > 0f)
                                    renderers[i].sprite = spriteSet.right;
                                else
                                    renderers[i].sprite = spriteSet.left;
                            }
                            else if (incomingDirection.y > 0f)
                            {
                                if (outgoingDirection.x > 0f)
                                    renderers[i].sprite = spriteSet.right;
                                else
                                    renderers[i].sprite = spriteSet.left;
                            }
                            else if (incomingDirection.y < 0f)
                            {
                                if (outgoingDirection.x > 0f)
                                    renderers[i].sprite = spriteSet.left;
                                else
                                    renderers[i].sprite = spriteSet.right;
                            }
                        }
                    }
                    while (i < renderers.Count)
                    {
                        renderers[i].enabled = false;
                        i++;
                    }
                }
                else
                    foreach (SpriteRenderer renderer in renderers)
                        renderer.enabled = false;
            }
        }

        private float VectorToOrthoAngle(Vector2 direction)
        {
            // Helper function that returns the degree
            // angle to the nearest pole.
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x < 0f)
                    return 90f;
                else
                    return -90f;
            }
            else
            {
                if (direction.y < 0f)
                    return 180f;
                else
                    return 0f;
            }
        }

        public bool IsVisible
        {
            set
            {
                for (int i = 0; i < chainLength; i++)
                    renderers[i].enabled = value;
            }
        }
    }

}
