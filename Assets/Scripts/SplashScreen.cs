using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public Text _progressText;

	void Start () {
        StartCoroutine(AsynchronousLoad(1));
	}

    IEnumerator AsynchronousLoad(int scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            _progressText.text = ((int)(progress * 100)).ToString() + "%";

            // Loading completed
            if (ao.progress == 0.9f)
            {
                    ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
