using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaldiMinigameBus : MonoBehaviour
{
    private GameControllerScript controller;

    public GameObject whiteFade;
    public AudioSource raldiSource;
    public AudioClip letsMinigame;
    public AudioClip battleBus;
    public AudioClip busDrop;
    bool dropped = false;
    
    public void Drop()
    {
        if (!dropped)
        {
            dropped = true;
            controller = GameControllerScript.current;

            controller.player.inNotebook = true;
            controller.HideChar();
            controller.StopAllEvents();
            controller.allowEvents = false;

            controller.audioDevice.PlayOneShot(battleBus);

            IEnumerator Transition()
            {
                whiteFade.SetActive(true);
                yield return new WaitForSeconds(2.5f);
                controller.FNFMinigame();
                raldiSource.gameObject.SetActive(false);
                yield return new WaitForSeconds(5.5f);
                whiteFade.SetActive(false);
            }

            StartCoroutine(Transition());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            raldiSource.PlayOneShot(letsMinigame);
        }
    }
}
