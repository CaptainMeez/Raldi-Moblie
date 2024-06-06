using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedPlayer : MonoBehaviour
{
    public GameObject battle;
    public Animator animation;
    public AudioSource source;
    public AudioSource glitch;
    public AudioClip talk;
    public Material corruptedSkybox;
    public GameObject soul;
    public GameObject player;
    public GameObject objects;
    public AudioClip undertaleMoment;

    public void Corrupt()
    {
        source.PlayOneShot(talk);
        animation.Play("CorruptedPlayerIngameTurn");

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(4);
            RenderSettings.skybox = corruptedSkybox;
            glitch.Play();
            objects.SetActive(true);
            yield return new WaitForSeconds(4);
            glitch.Stop();
            yield return new WaitForSeconds(5);
            soul.SetActive(true);
            source.PlayOneShot(undertaleMoment);
            yield return new WaitForSeconds(1.5f);
            soul.SetActive(false);
            player.SetActive(false);
            GameObject.Instantiate<GameObject>(battle, new Vector3(0, 1000, 0), battle.transform.rotation);
        }

        StartCoroutine(WaitTime());
    }
}
