using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilFinaleCutscene : MonoBehaviour
{
    public Animator neil;
    public Animator player;
    public Animator transitionToTheEnd;
    public AudioClip die;
    
    public void Hit()
    {
        IEnumerator Plead()
        {
            GameControllerScript.current.schoolMusic.Stop();

            GameControllerScript.current.audioDevice.PlayOneShot(finaleEnd);

            player.SetTrigger("Confront");
            cameraa.SetTrigger("End");
    
            yield return new WaitForSeconds(6);

            neil.SetTrigger("Die");
            GameControllerScript.current.audioDevice.PlayOneShot(die);

            transitionToTheEnd.gameObject.SetActive(true);
            yield return new WaitForSeconds(6);
            UnityEngine.SceneManagement.SceneManager.LoadScene("TheEnd");
        }

        StartCoroutine(Plead());
    }

    public void TrySave()
    {
        IEnumerator Plead()
        {
            GameControllerScript.current.schoolMusic.Stop();

            GameControllerScript.current.audioDevice.PlayOneShot(finaleEnd);

            player.SetTrigger("Confront2");
            cameraa.SetTrigger("End");
    
            yield return new WaitForSeconds(6);

            neil.SetTrigger("Die");
            GameControllerScript.current.audioDevice.PlayOneShot(die);

            transitionToTheEnd.gameObject.SetActive(true);
            yield return new WaitForSeconds(6);
            UnityEngine.SceneManagement.SceneManager.LoadScene("TheEnd");
        }

        StartCoroutine(Plead());
    }

    public AudioClip finaleEnd;
    public Animator cameraa;
}
