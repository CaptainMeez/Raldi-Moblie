using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFountain : MonoBehaviour
{
    public AudioClip sip;

    void Update()
    {
        if (RaldiInputManager.current.GetInteractDown() && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15 && raycastHit.transform == base.transform)
			{
                if (!(GameControllerScript.current.player.stamina >= GameControllerScript.current.player.maxStamina))
                    GameControllerScript.current.player.stamina = GameControllerScript.current.player.maxStamina;

                GetComponent<AudioSource>().PlayOneShot(sip);
            }
		}
    }
}
