using System.Collections;
using UnityEngine;

public class ProximityIndicator : MonoBehaviour
{
    [Tooltip("Reference to the player's transform")]
    public Transform player;

    [Tooltip("The indicator GameObject (child of the NPC) to show/hide")]
    public GameObject indicator;

    [Tooltip("Distance within which the indicator appears")]
    public float detectionRadius = 5f;

    [Tooltip("Duration of the pop-up/pop-down animation in seconds")]
    public float animationDuration = 0.3f;

    // Keeps track of the currently running animation coroutine.
    private Coroutine currentAnimation = null;

    // Tracks the current state of the indicator.
    private bool isIndicatorShown = false;

    void Start()
    {
        // Ensure the indicator starts hidden with scale zero.
        if (indicator != null)
        {
            indicator.SetActive(false);
            indicator.transform.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        if (player == null || indicator == null)
            return;

        // Determine the distance from the player.
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            if (!isIndicatorShown)
            {
                // Cancel any hide animation, if running.
                if (currentAnimation != null)
                    StopCoroutine(currentAnimation);

                // Activate and animate the indicator popping up.
                indicator.SetActive(true);
                currentAnimation = StartCoroutine(AnimateIndicator(true));
                isIndicatorShown = true;
            }
        }
        else
        {
            if (isIndicatorShown)
            {
                // Cancel any show animation, if running.
                if (currentAnimation != null)
                    StopCoroutine(currentAnimation);

                // Animate the indicator popping down.
                currentAnimation = StartCoroutine(AnimateIndicator(false));
                isIndicatorShown = false;
            }
        }
    }

    IEnumerator AnimateIndicator(bool show)
    {
        float elapsed = 0f;
        // Determine start and target scales.
        Vector3 startScale = indicator.transform.localScale;
        Vector3 targetScale = show ? Vector3.one : Vector3.zero;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            indicator.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        indicator.transform.localScale = targetScale;
        // If we're hiding the indicator, disable it after the animation.
        if (!show)
        {
            indicator.SetActive(false);
        }

        currentAnimation = null;
    }
}
