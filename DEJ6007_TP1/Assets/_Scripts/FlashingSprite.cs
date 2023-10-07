using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingSprite : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    bool isVisible = true;
    float lowerLimitVisible = 0.5f;
    float upperLimitVisible = 2f;
    float lowerLimitInvisible = 0.1f;
    float upperLimitInvisible = 1f;

    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FlashSprite());
    }

    // Cette image "flashe" à intervalles semi-aléatoire.
    IEnumerator FlashSprite()
    {
        while (true)
        {
            if (isVisible)
            {
                yield return new WaitForSeconds(Random.Range(lowerLimitVisible, upperLimitVisible));
                isVisible = false;
            } else
            {
                yield return new WaitForSeconds(Random.Range(lowerLimitInvisible, upperLimitInvisible));
                isVisible = true;
            }
            thisSpriteRenderer.enabled = isVisible;
        }
    }
}
