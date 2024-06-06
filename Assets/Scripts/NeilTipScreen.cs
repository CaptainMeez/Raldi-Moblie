using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeilTipScreen : MonoBehaviour
{
    private Animator animator;
    private AudioSource sounds;
    private bool canEnter = false;
    public AudioClip enter;

    private IEnumerator Start()
    {
       animator = GetComponent<Animator>(); 
       sounds = GetComponent<AudioSource>();

       yield return new WaitForSeconds(2.5f);

       canEnter = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canEnter)
        {
            canEnter = false;

            StartCoroutine(Disable());
        }
    }

    IEnumerator Disable()
    {
        animator.SetTrigger("Leave");
        sounds.Stop();
        sounds.PlayOneShot(enter);
        yield return new WaitForSeconds(2.5f);
        
        PlayerPrefs.SetFloat("StartOnNeilFinale", 1);
        
        SceneManager.LoadSceneAsync("School");
    }
}
