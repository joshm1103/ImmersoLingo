using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button cafeButton;
    public Button marketButton;
    public Button trainStationButton;
    public Button quitButton;

    private void Start()
    {
        SetUpSceneButton(cafeButton, SceneNames.Cafe);
        SetUpSceneButton(marketButton, SceneNames.Market);
        SetUpSceneButton(trainStationButton, SceneNames.TrainStation);
        quitButton.onClick.AddListener(Quit);
    }

    // buttons for scenes that arent in built
    private void SetUpSceneButton(Button button, string sceneName)
    {
        button.interactable = Application.CanStreamedLevelBeLoaded(sceneName);
        button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}