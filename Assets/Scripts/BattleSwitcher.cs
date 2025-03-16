using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    // Reference to the TransitionManager in the scene.
    public SceneTransitionManager transitionManager;

    // The name of the battle system scene (must be added in Build Settings).
    public string battleSceneName = "BattleSystem";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the scene transition.
            transitionManager.TransitionToScene(battleSceneName);
        }
    }
}
