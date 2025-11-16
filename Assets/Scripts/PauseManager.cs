using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    Transform pausePanel;
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    AsyncOperation asyncLoad;
    private void Awake()
    {
        Time.timeScale = 1f;
        pausePanel = transform.GetChild(0);
        pausePanel.gameObject.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !TransitionManager.instance.fadeInProgress)
        {
            Debug.Log("Pause Toggled");
            pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
            Time.timeScale = pausePanel.gameObject.activeSelf ? 0f : 1f;
            if(pausePanel.gameObject.activeSelf)
            {
                Debug.Log("Cursor Unlocked");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Debug.Log("Cursor Locked");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void ResumeGame()
    {
        pausePanel.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Used in game over panel and pause panel
        Time.timeScale = 1f;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Gameplay");
        asyncLoad.allowSceneActivation = false;
        TransitionManager.instance.onFadeComplete.AddListener(LoadGameplayScene);
        StartCoroutine(TransitionManager.instance.FadeToBlack());
    }

    void LoadGameplayScene()
    {
        TransitionManager.instance.onFadeComplete.RemoveListener(LoadGameplayScene);
        asyncLoad.allowSceneActivation = true;  
    }

    public void QuitGame()
    {
        // Used in game over panel and pause panel
        Time.timeScale = 1f;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;
        TransitionManager.instance.onFadeComplete.AddListener(LoadMainMenuScene);
        StartCoroutine(TransitionManager.instance.FadeToBlack());
    }

    void LoadMainMenuScene()
    {
        TransitionManager.instance.onFadeComplete.RemoveListener(LoadMainMenuScene);
        asyncLoad.allowSceneActivation = true;  
    }
}
