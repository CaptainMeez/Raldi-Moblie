using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020000C3 RID: 195
public class NotebookScript : MonoBehaviour
{
	public bool canCollect = true;
	public AudioClip ping;
	
	// Token: 0x0600098F RID: 2447 RVA: 0x00023FFB File Offset: 0x000223FB
	private void Start()
	{
		up = true;
		audioDevice = base.GetComponent<AudioSource>();
	}

	public void GoUp()
	{
		base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
		up = true;
		audioDevice.Play();
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00024018 File Offset: 0x00022418
	private void Update()
	{
		if (GameControllerScript.current.mode == "endless")
		{
			if (respawnTime > 0f)
			{
				if ((base.transform.position - GameControllerScript.current.player.transform.position).magnitude > 60f)
				{
					respawnTime -= Time.deltaTime;
				}
			}
			else if (!up)
			{
				GoUp();
			}
		}
		if (RaldiInputManager.current.GetInteractDown() && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < GameControllerScript.current.player.interactDistance))
			{
				Collect();
			}
		}
	}

	public void Ping()
	{
		if (up)
		{
			foreach(Transform objectt in base.transform)
			{
				objectt.gameObject.layer = 8;
			}

			audioDevice.PlayOneShot(ping);

			IEnumerator WaitToDisable()
			{
				yield return new WaitForSeconds(ping.length);

				foreach(Transform objectt in base.transform)
				{
					objectt.gameObject.layer = 0;
				}
			}

			StartCoroutine(WaitToDisable());
		}
	}

	public void Collect()
	{
		float modifier = (1 + PlayerPrefs.GetFloat("hot_headed"))/(1 + PlayerPrefs.GetFloat("someones_tired"));

		if (GameControllerScript.current.player.stamina < 100)
			GameControllerScript.current.player.stamina = 100;
		if (GameControllerScript.current.hardSecondPhase && !GameControllerScript.current.secondPhase10thCollected)
		{
			GameControllerScript.current.secondPhase10thCollected = true;
			GameControllerScript.current.hard10THBook.GoUp();
		}
		
		if (GameControllerScript.current.crafters.angry)
		{
			if (FindObjectOfType<PlayerStats>().data.objectasks[5] == 0)
			{
				FindObjectOfType<Objectasks>().CollectObjectask(5);
			}
		}

		base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
		up = false;
		respawnTime = 120f;

		GameControllerScript.current.CollectNotebook();
		GameControllerScript.current.StopAllEvents();

		if (GameControllerScript.current.notebooks == 0 && FindObjectOfType<TutorRaldi>() != null)
		{
			if (FindObjectOfType<TutorRaldi>().talking)
				FindObjectOfType<TutorRaldi>().TalkAreaHidden();
		}

		if (!GameControllerScript.current.mrBeast.beasting && (GameControllerScript.current.mode != "hard" && !GameControllerScript.current.mode2016) && GameControllerScript.current.mode != "story_cow") // Check if MrBeast is active.
		{
			if (!GameControllerScript.current.neilMode) // Meme Book
			{
				RenderSettings.skybox = GameControllerScript.current.defaultSky;
				if (GameControllerScript.current.notebooks == 2) // Question Game
				{	
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameControllerScript.current.questionGame);
				}
				else // Meme Book
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(learningGame);
					gameObject.GetComponent<MathGameScript>().psc = GameControllerScript.current.player;
					gameObject.GetComponent<MathGameScript>().gc = GameControllerScript.current;
					gameObject.GetComponent<MathGameScript>().baldiScript = GameControllerScript.current.baldi;
				}
			}
			else // Neil Book
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameControllerScript.current.neilBook);
				gameObject.GetComponent<NeilBook>().gc = GameControllerScript.current;
			}
		} 
		else 
		{
			GameControllerScript.current.beastnotebooks += 1;

			if (GameControllerScript.current.player.stamina < 100)
				GameControllerScript.current.player.stamina = 100;

			if (GameControllerScript.current.mode == "endless")
				GameControllerScript.current.baldi.baldiAnger -= 0.25f;
			else 
				GameControllerScript.current.baldi.GetAngry(1f * modifier);
		}
		
		if (GameControllerScript.current.mode == "hard" || GameControllerScript.current.mode2016)
		{
			GameControllerScript.current.GiveScore(100);
			GameControllerScript.current.baldi.speed += 1.25f;

			if (GameControllerScript.current.notebooks == GameControllerScript.current.notebooksToCollect && GameControllerScript.current.notebooksToCollect == 10 && GameControllerScript.current.mode == "hard")
			{
				foreach(NotebookScript books in FindObjectsOfType<NotebookScript>())
				{
					if (books != this)
						books.GoUp();
				}
				GameControllerScript.current.notebooksToCollect = 19;
				GameControllerScript.current.UpdateNotebookCount();
				GameControllerScript.current.ActivateMorbinTime();
				GameControllerScript.current.hardSecondPhase = true;
				GameControllerScript.current.secondPhase10thCollected = false;
			}
		}

		if (GameControllerScript.current.mode2016 && GameControllerScript.current.notebooks == 1)
		{
			GameControllerScript.current.entrance_0.Lower();
			GameControllerScript.current.entrance_1.Lower();
			GameControllerScript.current.entrance_2.Lower();
			GameControllerScript.current.entrance_3.Lower();
		}

		if ((GameControllerScript.current.notebooks == GameControllerScript.current.notebooksToCollect) && (GameControllerScript.current.mode == "hard" || GameControllerScript.current.mrBeast.beasting || GameControllerScript.current.mode2016) && !(GameControllerScript.current.mode == "endless"))
		{
			GameControllerScript.current.entrance_0.Raise();
			GameControllerScript.current.entrance_1.Raise();
			GameControllerScript.current.entrance_2.Raise();
			GameControllerScript.current.entrance_3.Raise();

			GameControllerScript.current.finaleMode = true;
		}

		if (GameControllerScript.current.notebooks == 2)
        {
            GameControllerScript.current.raldiStyle.SetActive(false);
        }

		if (GameControllerScript.current.mode == "story_cow")
		{
			GameControllerScript.current.polishCow.GetComponent<UnityEngine.AI.NavMeshAgent>().speed += 1f;
		}
	}

	public float respawnTime;

	public bool up;

	public GameObject learningGame;

	private AudioSource audioDevice;
}
