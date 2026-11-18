using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuPanel;      // the PauseMenu canvas
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;
    public float distance = 1.5f;     // how far in front of the player it pops up

    private readonly PauseMenuState state = new PauseMenuState();
    private InputAction menuAction;

    private void Awake()
    {
        menuAction = new InputAction("Menu", InputActionType.Button);
        menuAction.AddBinding("<XRController>{LeftHand}/{MenuButton}"); // Quest left ☰ button
        menuAction.AddBinding("<Keyboard>/escape");                     // for testing in the editor

        resumeButton.onClick.AddListener(Close);
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.MainMenu));
        quitButton.onClick.AddListener(MainMenuController.Quit);

        menuPanel.SetActive(false);
    }

    private void OnEnable()
    {
        menuAction.performed += OnMenuPressed;
        menuAction.Enable();
    }

    private void OnDisable()
    {
        menuAction.performed -= OnMenuPressed;
        menuAction.Disable();
    }

    private void OnDestroy() => menuAction.Dispose();

    private void OnMenuPressed(InputAction.CallbackContext _)
    {
        state.Toggle();
        if (state.IsOpen) PlaceInFrontOfPlayer();
        menuPanel.SetActive(state.IsOpen);
    }

    private void Close()
    {
        state.Close();
        menuPanel.SetActive(false);
    }

    // Puts the menu at eye level in front of wherever the player is looking (ignores looking up/down)
    private void PlaceInFrontOfPlayer()
    {
        Transform cam = Camera.main.transform;
        Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        menuPanel.transform.position = cam.position + forward * distance;
        menuPanel.transform.rotation = Quaternion.LookRotation(forward);
    }
}