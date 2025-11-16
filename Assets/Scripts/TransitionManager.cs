using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public UnityEvent onFadeComplete;
    public bool fadeInProgress;
    [SerializeField] Image blackScreen;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            blackScreen.gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator FadeToBlack()
    {
        fadeInProgress = true;
        blackScreen.color = new Color(0, 0, 0, 0);
        blackScreen.gameObject.SetActive(true);
        float fadeDuration = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        onFadeComplete.Invoke();
        StartCoroutine(FadeToClear());
    }

    public IEnumerator FadeToClear()
    {
        float fadeDuration = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(t / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeInProgress = false;
        blackScreen.gameObject.SetActive(false);
    }
}
