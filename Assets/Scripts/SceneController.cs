using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public enum SceneType
    {
        MenuScene,
        GameScene
    }

    [SerializeField] private string menuSceneName = "MenuScene";
    [SerializeField] private string gameSceneName = "GameScene";

    public void SwitchScene(SceneType scene)
    {
        string sceneName = "";

        switch (scene)
        {
            case SceneType.MenuScene:
                sceneName = menuSceneName;
                break;
            case SceneType.GameScene:
                sceneName = gameSceneName;
                break;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void SwitchToMenuScene()
    {
        SwitchScene(SceneType.MenuScene);
    }

    public void SwitchToGameScene()
    {
        SwitchScene(SceneType.GameScene);
    }
}