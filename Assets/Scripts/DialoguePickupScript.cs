using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CoulsonEngine.Game.Dialogue;

public class DialoguePickupScript : MonoBehaviour
{
    public string dialogueFile;
    public DialogueManager dialogueManager;
    [HideInInspector] public Dialogue dialogue;
	private bool alreadyPlayedDialogue = false;
    public GameObject sprite;
    public int id;

	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15 && raycastHit.transform.gameObject.tag == "Item" && raycastHit.transform == base.transform)
			{
				IEnumerator Trigger(float del = 0)
                {
                    if (!dialogueManager.InDialogue && !alreadyPlayedDialogue)
                    {
                        alreadyPlayedDialogue = true;

                        dialogue = Dialogue.LoadFromFile(dialogueFile);

                        yield return new WaitForSeconds(del);

                        dialogueManager.StartDialogue(dialogue);

                        yield return null;
                        
                        dialogueManager.whenDone.AddListener(PickupItem);

                        sprite.SetActive(false);
                    }
                }

                StartCoroutine(Trigger());
            }
		}
	}

    public void PickupItem()
    {
        GameControllerScript.current.CollectItem(id);
    }
}
