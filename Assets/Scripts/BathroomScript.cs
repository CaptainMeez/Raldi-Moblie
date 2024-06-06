using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomScript : MonoBehaviour
{
    void Update()
    {
        if (!GameControllerScript.current.player.washedHands && GameControllerScript.current.player.justPooped && !GameControllerScript.current.inBathroom)
            GameControllerScript.current.player.ResetGuilt("wash", 0.1f);
    }

    private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
			GameControllerScript.current.inBathroom = true;
	}

    private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
			GameControllerScript.current.inBathroom = false;
	}
}
