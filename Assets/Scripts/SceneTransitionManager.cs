using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Tooltip("The UI Image used for fade transitions.")]
    public Image fadeImage;

    [Tooltip("How long the fade out takes in seconds.")]
    public float fadeDuration = 1f;

    void Awake()
    {
        // Set up the singleton instance.
        if (Instance == null)
        {
            Instance = this;
            // Optionally, persist this manager across scenes:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this function to trigger a fade-out transition and switch to the given scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load (ensure it's added in Build Settings).</param>
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitch(sceneName));
    }

    private IEnumerator FadeAndSwitch(string sceneName)
    {
        float timer = 0f;

        // Fade from transparent (alpha 0) to black (alpha 1).
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            Color currentColor = fadeImage.color;
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        // After the fade, load the new scene.
        SceneManager.LoadScene(sceneName);
    }
}
