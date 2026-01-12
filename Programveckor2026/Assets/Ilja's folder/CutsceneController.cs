using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public float cutsceneDuration = 5f;
    public string nextSceneName = "GameScene";

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Auto-transition after time
        if (timer >= cutsceneDuration)
        {
            LoadNextScene();
        }

        // Optional: skip cutscene
        if (Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
