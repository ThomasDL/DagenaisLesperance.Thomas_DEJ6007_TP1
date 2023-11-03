using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingSprite : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    bool isVisible = true;
    float lowerLimitVisible = 0.4f;
    float upperLimitVisible = 1.5f;
    float lowerLimitInvisible = 0.1f;
    float upperLimitInvisible = 0.9f;

    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FlashSprite());
    }

    // Cette image "flashe" � intervalles semi-al�atoire.
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
