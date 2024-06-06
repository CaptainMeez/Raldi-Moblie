using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Token: 0x020000C1 RID: 193
public class GameOverScript : MonoBehaviour
{
	public Animator anim;
	public AudioClip impact;
	public AudioClip newBestImpact;
	public AudioClip gameOver;
	public AudioClip end;
	public AudioSource gameOverSource;
	public Image newBestImage;
	public TextMeshProUGUI newBestText;
	public TextMeshProUGUI notebooksText;
	public TextMeshProUGUI exitsText;
	public int notebooks;
	public int exits;
	private float percentage;
	private bool newBest = false;
	bool canCont = false;
	public GameObject needHelp;
	private bool modifiersOn;

	private void Start()
	{	
		foreach(Modifier modifier in ModifierMenu.modifiers)
		{
			if (PlayerPrefs.GetFloat(ModifierMenu.ConvertToInternal(modifier.name)) == 1)
				modifiersOn = true;
		}
		FindObjectOfType<PlayerStats>().TryLoad();

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		if (!(PlayerPrefs.GetInt("cursessionendless") == 1))
		{
			if (notebooks == 0 && exits == 0)
			{
				notebooks = PlayerPrefs.GetInt("recentNormalModeNotebooks");
				exits = PlayerPrefs.GetInt("recentNormalModeExits");
			}

			notebooksText.text = "Notebooks:" + notebooks;
			exitsText.text = "Exits:" + exits;

			if (notebooks != 10)
				exitsText.enabled = false; 

			if ((notebooks > FindObjectOfType<PlayerStats>().data.recordNormalNotebooks || exits > FindObjectOfType<PlayerStats>().data.recordNormalExits) && modifiersOn)
			{
				percentage = Mathf.Ceil((notebooks * 9) + (exits * 2.5f));
				print(percentage);
				newBest = true;
				newBestText.text = percentage + "%";

				FindObjectOfType<PlayerStats>().data.recordNormalNotebooks = notebooks;
				FindObjectOfType<PlayerStats>().data.recordNormalExits = exits;
				FindObjectOfType<PlayerStats>().Save();
			} 
			else 
			{
				newBestImage.enabled = false;
				newBestText.enabled = false;
			}
		} 
		else 
		{
			newBest = false;
			notebooks = PlayerPrefs.GetInt("recentNormalModeNotebooks");
			notebooksText.text = "Notebooks:" + notebooks;
			exitsText.enabled = false;

			if (PlayerPrefs.GetInt("record endless") == 1)
			{
				newBest = true;
				newBestText.text = notebooks + " Notebooks";
			} 
			else 
			{
				newBestImage.enabled = false;
				newBestText.enabled = false;
			}
		}

		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
		this.image.sprite = this.images[num];

		if (UnityEngine.Random.Range(0, 50) == 25)
			needHelp.SetActive(true);

		IEnumerator Music()
		{
			if (!newBest)
				gameOverSource.PlayOneShot(impact);
			else
				gameOverSource.PlayOneShot(newBestImpact);
			
			yield return new WaitForSeconds(1.4f);
			newBestImage.enabled = false;
			newBestText.enabled = false;
			canCont = true;
			gameOverSource.clip = gameOver;
			gameOverSource.loop = true;
			gameOverSource.Play();
		}

		StartCoroutine(Music());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) && canCont)
		{
			canCont = false;

			IEnumerator Wait()
			{
				gameOverSource.Stop();
				gameOverSource.PlayOneShot(end);
				anim.SetTrigger("End");
				yield return new WaitForSeconds(end.length);
				SceneManager.LoadSceneAsync("School");
			}

			StartCoroutine(Wait());
		}
		if (Input.GetKeyDown(KeyCode.Escape) && canCont)
		{
			canCont = false;

			SceneManager.LoadSceneAsync("MainMenu");
		}
	}

	public Image image;
	public Sprite[] images = new Sprite[5];
}
