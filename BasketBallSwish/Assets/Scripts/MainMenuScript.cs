using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    public GameObject tutorialPopUp;

	public void PlaySingleGame()
    {
        StartCoroutine(LoadSceneAsynchronously("GameScene"));       
    }
    public void PlayTournamentGame()
    {
        StartCoroutine(LoadSceneAsynchronously("TournamentScene"));
    }

    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);
            slider.value = progress * 30;
            yield return null;
        }
    }

    public void PlayTutorial()
    {        
        StartCoroutine(WaitToStartTutorial());        
    }

    public void StartTutorial()
    {
        StartCoroutine(LoadSceneAsynchronously("TutorialScene"));
    }

    IEnumerator WaitToStartTutorial()
    {
        yield return new WaitForSeconds(0.20f);
        tutorialPopUp.SetActive(true);
    }

    public void quit()
    {
        Application.Quit();
    }
}
