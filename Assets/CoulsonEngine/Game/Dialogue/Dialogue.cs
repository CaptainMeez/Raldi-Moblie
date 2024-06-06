using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

// Customizable dialogue boxes that contain options, sentences, icons, and more.
namespace CoulsonEngine.Game.Dialogue
{
	[System.Serializable]
	public class Dialogue
	{
		public DialogueSentence[] sentences;

		public static Dialogue LoadFromFile(string dialogue)
		{
			var jsonFile = Resources.Load<TextAsset>($"lang/en-us/dialogue/{dialogue}");
			string json = jsonFile.text;
			Dialogue fetchedDialogue = JsonUtility.FromJson<Dialogue>(json);

			return fetchedDialogue;
		}
	}

	[System.Serializable]
	public class DialogueSentence
	{
		public string character;
		
		[TextArea(3, 10)]
		public string sentence;
		public bool isRight;
		public bool keepOriginalText;

		[Header("Options")]
		public bool showOptions;
		
		public DialogueOptions[] options;
	}

	[System.Serializable]
	public class DialogueOptions
    {
        public string option;
        public PostOptionDialogueSentence[] nextDialogue;
    }

	[System.Serializable]
	public class PostOptionDialogueSentence
	{
		public string character;
		
		[TextArea(3, 10)]
		public string sentence;
		public bool isRight;
		public bool keepOriginalText;
	}
}