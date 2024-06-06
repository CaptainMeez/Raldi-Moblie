using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameControllerScript gc;
    public NeilHallwayScript hallway;
    public Transform player;
    public bool canCollect = true;
    public GameObject image;
    public AudioSource source;
    public AudioClip collect;
    public CoulsonEngine.Game.Dialogue.FileDialogueTrigger dialogue;

    private void Update()
	{
		if ((Input.GetMouseButtonDown(0) /*|| Gamepad.current.rightTrigger.wasPressedThisFrame*/) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, player.position) < 15 && raycastHit.transform.gameObject.tag == "Item" && raycastHit.transform == base.transform && canCollect)
			{
                gc.neilKeysCollected++;
                canCollect = false;
                image.SetActive(false);

                gc.player.inNotebook = true;

                source.pitch = UnityEngine.Random.Range(0.9f, 1.9f);
                source.PlayOneShot(collect);

                if (!GameControllerScript.current.ishaanKeys)
                    dialogue.dialogueFile = "neilkey0" + gc.neilKeysCollected;
                else
                    dialogue.dialogueFile = "ishaankey";

                dialogue.onComplete.AddListener(EnablePlayer);
                dialogue.TriggerDialogue(1);

                if (gc.neilKeysCollected == 5)
                    dialogue.onComplete.AddListener(hallway.Enable);
		    }
		}
    }

    public void EnablePlayer()
    {
        gc.player.inNotebook = false;
    }
}
