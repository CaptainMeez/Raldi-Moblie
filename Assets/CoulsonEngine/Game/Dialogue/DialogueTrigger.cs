using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start dialogue from anywhere in the game.
namespace CoulsonEngine.Game.Dialogue
{
	public class DialogueTrigger : MonoBehaviour 
	{
		public Dialogue dialogue;
		public DialogueManager dialogueManager;

		public void TriggerDialogue(float delay = 0) => StartCoroutine(trigger(delay));

		IEnumerator trigger(float del = 0)
		{
			yield return new WaitForSeconds(del);
			dialogueManager.StartDialogue(dialogue);
		}
	}
}
