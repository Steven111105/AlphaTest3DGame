using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject settingsPanel;
    public TMP_Text highScoreText;

    AsyncOperation asyncLoad;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Highest Score: " + highScore.ToString();
    }
    public void StartGame()
    {
        StartCoroutine(TransitionManager.instance.FadeToBlack());
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Gameplay");
        asyncLoad.allowSceneActivation = false;
        TransitionManager.instance.onFadeComplete.AddListener(LoadGameplayScene);
    }

    void LoadGameplayScene()
    {
        TransitionManager.instance.onFadeComplete.RemoveListener(LoadGameplayScene);
        asyncLoad.allowSceneActivation = true;  
    }

    public void ToggleSettings()
    {
        // Back button on settings panel uses this too (Set in inspector)
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
