using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.IO;
using UnityEditor;

using CoulsonEngine.Text;

// Manager for dialogue boxes.
namespace CoulsonEngine.Game.Dialogue
{

	[System.Serializable]
	public class CharImages
	{
		public string name;
		public Sprite image;
	}

	public class DialogueManager : MonoBehaviour
	{
		public CharImages[] images;
		public TMPro.TextMeshProUGUI nameText;
		public TMPro.TextMeshProUGUI dialogueText;
		private Queue<string> sentences;
		public AudioSource AudioSource;
		public int currentSentence;
		public string curSent;
		public bool InDialogue;
		public Image charImage;
		DialogueSentence[] dialogueSentences;
		[HideInInspector] public UnityEvent whenDone;
		public AudioClip continueAudio;
		public float dialogueDelay;
		public GameObject enterObject;
		public bool dontOffset = false;

		void Start()
		{
			sentences = new Queue<string>();
			AudioSource = base.GetComponent<AudioSource>();
		}

		void Update()
		{
			CheckForInput();

			if (shownAllText)
				enterObject.SetActive(true);
			else
				enterObject.SetActive(false);
		}

		void CheckForInput()
		{
			if ((UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.Z)) && this.InDialogue && !inOption)
			{
				if (shownAllText)
					DisplayNextSentence();
				else
				{
					StopAllCoroutines();
				
					shownAllText = true;
					dialogueText.text = GetSentVarriables(curSent).Replace("%txslow%", "");
				}

				AudioSource.PlayOneShot(continueAudio);
			}
		}
		
		public GameObject box;

		public void StartDialogue(Dialogue dialogue)
		{
			if (FindObjectOfType<PlayerScript>() != null)
				FindObjectOfType<PlayerScript>().inNotebook = true;

			InDialogue = true;
			inOption = false;

			box.SetActive(true);

			currentSentence--;
			dialogueSentences = dialogue.sentences;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			foreach (DialogueSentence sentence in dialogue.sentences)
			{
				string tsentence = sentence.sentence;

				sentences.Enqueue(tsentence);
			}

			DisplayNextSentence();
		}

		bool inOption = false;

		bool shownAllText = false;

		public void DisplayNextSentence()
		{		
			shownAllText = false;

			this.AudioSource.PlayOneShot(this.continueAudio);

			if (sentences.Count == 0) 
			{ 
				EndDialogue();
				if (FindObjectOfType<PlayerScript>() != null)
					FindObjectOfType<PlayerScript>().inNotebook = false;
				return; 
			}

			currentSentence++;
			string sentence = sentences.Dequeue();
			curSent = sentence;

			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));

			bool did = false;

			if (!dontOffset)
			{
				foreach(CharImages img in images)
				{
					if (img.name.ToLower() == dialogueSentences[currentSentence].character.ToLower() && !did)
					{
						did = true;
						charImage.gameObject.SetActive(true);
						charImage.sprite = img.image;
						dialogueText.transform.localPosition = new Vector3(-45.059f, -151.1f, 0);
						dialogueText.rectTransform.sizeDelta = new Vector2(376.118f, 127.9271f);
					}
				}

				if (!did)
				{
					charImage.gameObject.SetActive(false);
					dialogueText.transform.localPosition = new Vector3(19.941f, -151.1f, 0);
					dialogueText.rectTransform.sizeDelta = new Vector2(506.1182f, 127.9271f);
				}
			}	
		}

		string GetSentVarriables(string sent)
		{
			string news = StringFormat.FormatString(sent);

			return news;
		}

		public AudioClip sound;

		IEnumerator TypeSentence(string sentence)
		{
			if(!dialogueSentences[currentSentence].keepOriginalText)
				dialogueText.text = "";
			else
				dialogueText.text += " ";

			string toType = sentence;

			int delayMod = 1;

			if (toType.Contains("%txslow%"))
			{
				toType = toType.Replace("%txslow%", "");
				delayMod = 7;
			}

			toType = GetSentVarriables(toType);

			foreach (char letter in toType.ToCharArray())
			{
				AudioSource.Stop();

				string preset = dialogueSentences[this.currentSentence].character;
		
				dialogueText.text += letter; 

				if (sound != null && letter != ' ')
					AudioSource.PlayOneShot(sound);

				yield return new WaitForSeconds(dialogueDelay * delayMod);
				yield return null;
			}

			shownAllText = true;
		}

		public void EndDialogue(bool noEndEvent = false)
		{
			box.SetActive(false);
			currentSentence = 0;
			dialogueSentences = null;

			sentences.Clear();

			InDialogue = false;
			Time.timeScale = 1f;

			if (FindObjectOfType<PlayerScript>() != null)
				FindObjectOfType<PlayerScript>().inNotebook = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			
			StopAllCoroutines();

			if (!noEndEvent)
			{
				whenDone.Invoke();
				whenDone.RemoveAllListeners();
			}
		}

	}
}