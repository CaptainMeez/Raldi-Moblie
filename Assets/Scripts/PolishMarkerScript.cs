using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolishMarkerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shatter;
    public AudioClip putdown;
    public AudioClip pickup;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;
        
        if (!Input.GetMouseButtonDown(0) || Time.timeScale == 0f || GameControllerScript.current.inventoryfull)
			return;
		
		if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 10 && raycastHit.transform.gameObject.tag == "PMarker")
		{
			GameControllerScript.current.CollectItem(25);
			Destroy(gameObject);
		}
    }

    public void Break()
    {
        GameControllerScript.current.audioDevice.PlayOneShot(shatter, 3f);
        Destroy(gameObject);
    }
}
