using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilHallwayScript : MonoBehaviour
{
    public GameControllerScript gc;
    public GameObject objects;
    public GameObject oldTrigger;
    public GameObject newTrigger;
    public GameObject whiteFade;
    public GameObject vhsRoom;
    public GameObject longHallway;
    public Transform playerSpawn;
    public AudioClip neilApproach;
    public Animator neillight;
    bool lightHidden = false;

    public void Enable()
    {
        gc.schoolMusic.Stop();
        objects.SetActive(true);
        oldTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !lightHidden)
        {
            neillight.SetTrigger("Gone");
            lightHidden = true;
        }
    }

    public void TransitionToHallway()
    {
        gc.canPause = false;
        gc.player.inNotebook = true;
        gc.hud.SetActive(false);

        if (!GameControllerScript.current.ishaanKeys)
        {
            gc.schoolMusic.clip = neilApproach;
            gc.schoolMusic.Play();
        }
       

        IEnumerator Transition()
        {
            whiteFade.SetActive(true);
            yield return new WaitForSeconds(3.5f);

            if (!GameControllerScript.current.ishaanKeys)
                longHallway.SetActive(true);
            else
                vhsRoom.SetActive(true);

            gc.player.cc.enabled = false;
            gc.player.transform.position = playerSpawn.transform.position;
            gc.player.transform.rotation = playerSpawn.transform.rotation;
            gc.player.cc.enabled = true;
            gc.player.inNotebook = false;
            yield return new WaitForSeconds(4.5f);
            whiteFade.SetActive(false);
        }

        StartCoroutine(Transition());
    }
}
