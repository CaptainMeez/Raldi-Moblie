using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoolTransition : MonoBehaviour
{
    public GameObject objectt;
    
    public void Transition(string scene)
    {
        IEnumerator Load()
        {
            objectt.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(scene);
        }

        StartCoroutine(Load());
    }
}
