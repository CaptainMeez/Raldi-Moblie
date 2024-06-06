using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorRaldi : MonoBehaviour
{
    public AudioSource raldiSinging;
    public AudioSource schoolMusic;
    public AudioClip raldiHi;
    public GameObject talkArea;
    public bool talking = false;

    private void Awake()
    {
        float schoolMusicPos = schoolMusic.time;

        talking = true;

        schoolMusic.Pause();
        raldiSinging.PlayOneShot(raldiHi);
        talkArea.SetActive(true);

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(raldiHi.length);
            schoolMusic.UnPause();
            talkArea.SetActive(false);
            talking = false;
        }

        StartCoroutine(WaitTime());
    }

    public void TalkAreaHidden() {talkArea.SetActive(false); schoolMusic.UnPause();}
}
