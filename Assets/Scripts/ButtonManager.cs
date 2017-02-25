using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadStorySelectScreen()
    {
        SceneManager.LoadScene("_StorySelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
