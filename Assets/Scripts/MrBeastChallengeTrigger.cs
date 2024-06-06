using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeastChallengeTrigger : MonoBehaviour
{
    public MrBeastScript mrBeast;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            if (PlayerPrefs.GetFloat("youtube_detention") != 1)
            {
                Debug.Log("Failed");

                mrBeast.inChallenge = false;

                mrBeast.challenge.SetActive(false);
                
                mrBeast.coolDown = 30f;
            } else {
                GameControllerScript.current.player.cc.enabled = false;
				GameControllerScript.current.player.transform.position = new Vector3(-75f, 4f, 285f);
				GameControllerScript.current.player.cc.enabled = true;
                GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.aud_PearlLand);
            }
        }
    }
}
