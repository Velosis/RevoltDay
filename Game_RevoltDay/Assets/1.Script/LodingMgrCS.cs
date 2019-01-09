using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingMgrCS : MonoBehaviour {
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    public float _value;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    string nextSceneName;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("2.Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 2.0f, timer);

                if (progressBar.fillAmount == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                Debug.Log("너니?");
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0.0f;
                }
            }
        }
    }
}
