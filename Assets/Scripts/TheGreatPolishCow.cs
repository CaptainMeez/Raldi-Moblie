using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGreatPolishCow : MonoBehaviour
{
    public PlayerMovementScript player;

    public Camera mainCamera;
    public GameObject playerVcam;
    public GameObject cowVcam;
    public AudioSource sounds;
    public AudioSource music;
    public AudioSource reallyStupidSource;
    public AudioClip run;
    public AudioClip cowTalk;
    public Animator cow;

    bool didFunny = false;

    public LayerMask funnyMask;
    private LayerMask originalMask;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !didFunny)
        {
            IEnumerator FunnyCow()
            {
                originalMask = mainCamera.cullingMask;
                mainCamera.cullingMask = funnyMask;

                didFunny = true;
                playerVcam.SetActive(false);
                cowVcam.SetActive(true);
                sounds.PlayOneShot(cowTalk);
                reallyStupidSource.Stop();
                player.disableMovement = true;

                yield return new WaitForSeconds(cowTalk.length + 0.5f);

                music.clip = run;
                music.Play();

                cow.SetTrigger("Chase");
                player.disableMovement = false;
                playerVcam.SetActive(true);
                cowVcam.SetActive(false);

                mainCamera.cullingMask = originalMask;
            }

            StartCoroutine(FunnyCow());
        }
    }
}
