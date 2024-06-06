using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Slider valueSlide;
    public float progress; 
    public AsyncOperation operation;

    public IEnumerator LoadingLoadScene(string sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return null;
    }

    public IEnumerator LoadingLoadSceneInt(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return null;
    }


    public void Update()
    {
        if (!operation.isDone)
        {
            print("update");
            float progress = Mathf.Clamp01(operation.progress / .9f);

            valueSlide.value = progress;
        }
    }

}
