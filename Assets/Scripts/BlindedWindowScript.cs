using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindedWindowScript : MonoBehaviour
{
    public Material blinds;
    public Material noblinds;

    public AudioClip blindsSound;
    
    public bool blinded = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15 && raycastHit.transform.gameObject.name.ToLower().Contains("blinded") && raycastHit.transform == base.transform)
			{
				blinded = !blinded;

                GameControllerScript.current.audioDevice.PlayOneShot(blindsSound);

                switch(blinded)
                {
                    case true:
                        base.GetComponent<MeshRenderer>().material = blinds;
                        break;
                    case false:
                        base.GetComponent<MeshRenderer>().material = noblinds;
                        break;
                }
			}
		}
    }
}
