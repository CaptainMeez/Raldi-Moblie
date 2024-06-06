using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueEndingCutscene : MonoBehaviour
{
    public AudioSource music1;
    public AudioSource music2;
    bool doEndingStuffs = false;
    public Animator animation;
    public AudioClip peterGriffin;

    public void Update()
    {
        if (doEndingStuffs)
        {
            if (music1.volume > 0)
                music1.volume -= Time.deltaTime;
        }
    }

    public void End()
    {
        StartCoroutine(DoEnd());
    }

    public IEnumerator DoEnd()
    {
        doEndingStuffs = true;
        yield return new WaitForSeconds(1f);
        music2.PlayOneShot(peterGriffin);
        yield return new WaitForSeconds(1f);
        animation.StopPlayback();
        animation.Play("TrueEndingCutscene02");
        music2.Play();
    }
}
