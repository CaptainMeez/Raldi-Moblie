using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Start dialogue from anywhere in the game.
namespace CoulsonEngine.Game.Dialogue
{
	public class FileDialogueTrigger : MonoBehaviour 
	{
		public string dialogueFile;
		public DialogueManager dialogueManager;
		public UnityEvent onComplete;
		private bool alreadyLoadedFile = false;

		[HideInInspector] public Dialogue dialogue;
		
		public void TriggerDialogue(float delay = 0) => StartCoroutine(Trigger(delay));
	
		IEnumerator Trigger(float del = 0)
		{
			if (!dialogueManager.InDialogue)
			{
				if (!alreadyLoadedFile)
				dialogue = Dialogue.LoadFromFile(dialogueFile);

				yield return new WaitForSeconds(del);

				dialogueManager.StartDialogue(dialogue);

				yield return null;
				
				dialogueManager.whenDone = onComplete;
			}
		}

		public void ReloadDialogueVarriable()
		{
			dialogue = Dialogue.LoadFromFile(dialogueFile);
			alreadyLoadedFile = true;
		}
	}
}
