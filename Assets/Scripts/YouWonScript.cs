using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using GameJolt;

// Token: 0x020000D5 RID: 213
public class YouWonScript : MonoBehaviour
{
	public AudioClip tink;
	public AudioClip caching;
	public AudioClip newrecordsound;
	public AudioClip objectaskcomplete;
	public AudioClip victory;
	public AudioSource audiodevice;
	public AudioSource music;
	public TextMeshProUGUI text;
	public TextMeshProUGUI newrecordtext;
	public float minutes;
	public float seconds;
	public bool minutesdone;
	public bool secondsdone;
	public bool done;
	private int countupminutes = 0;
	private int countupseconds = 0;
	private float waituntilswitch = 0;
	public bool newrecord;
	public GameObject modifiersText;
	private string[] unlockPrefs = {"vanman", "chipfloke", "mrbeast", "prize", "raldi", "crafters", "bloke", "pintobeans"};    

	private void Start()
	{
		bool modifiersOn = false;

		foreach(Modifier modifier in ModifierMenu.modifiers)
		{
			if (PlayerPrefs.GetFloat(ModifierMenu.ConvertToInternal(modifier.name)) == 1)
				modifiersOn = true;
		}

		if (GameJolt.API.GameJoltAPI.Instance != null)
			TryGetAchievement();

		FindObjectOfType<PlayerStats>().TryLoad();

		if (modifiersOn)
		{
			modifiersText.SetActive(true);
			float record = FindObjectOfType<PlayerStats>().data.modifrecordTime;
			float current = PlayerPrefs.GetFloat("CurSessionPlaytime");
			
			if (current < record)
			{
				FindObjectOfType<PlayerStats>().data.modifrecordTime = current;
				FindObjectOfType<PlayerStats>().Save();
				newrecord = true;
			}
		}
		else
		{
			FindObjectOfType<PlayerStats>().data.storyModeWon = true;

			float record = FindObjectOfType<PlayerStats>().data.recordTime;
			float current = PlayerPrefs.GetFloat("CurSessionPlaytime");
			
			if (current < record)
			{
				FindObjectOfType<PlayerStats>().data.recordTime = current;
				FindObjectOfType<PlayerStats>().Save();
				newrecord = true;
			}
			
			minutes = (Mathf.Floor(PlayerPrefs.GetFloat("CurSessionPlaytime")/60));
			seconds = (Mathf.Floor(PlayerPrefs.GetFloat("CurSessionPlaytime")) % 60);
		}
		
		newrecordtext.enabled = false;
		text.text = "0:00";
		this.delay = 5f;
		music.PlayOneShot(victory);
	}

	public void TryGetAchievement()
	{
		GameJolt.API.Trophies.TryUnlock(188676);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00026797 File Offset: 0x00024B97
	private void Update()
	{
		this.delay -= Time.deltaTime;
		if (this.delay <= 0f && !done)
		{
			this.delay = 0.05f;
			if (countupminutes != minutes)
				countupminutes += 1;
			else if (countupseconds != seconds)
				countupseconds += 1;
			else
				done = true;
			if (countupseconds < 10)
				text.text = countupminutes + ":0" + countupseconds;
			else
				text.text = countupminutes + ":" + countupseconds;
			if (done)
			{
				audiodevice.PlayOneShot(caching);
				if (newrecord)
				{
					newrecordtext.enabled = true;
					audiodevice.PlayOneShot(newrecordsound);
				}
			}
			else
				audiodevice.PlayOneShot(tink);
		}
		if (Input.GetKeyDown(KeyCode.Return)) {
			audiodevice.PlayOneShot(caching);
			if (seconds < 10)
				text.text = minutes + ":0" + seconds;
			else
				text.text = minutes + ":" + seconds;
			if (newrecord)
			{
				newrecordtext.enabled = true;
				audiodevice.PlayOneShot(newrecordsound);
			}
			waituntilswitch = 1f;
		}
		if (waituntilswitch != 0)
		{
			waituntilswitch += Time.deltaTime;
			if (waituntilswitch > 1.1f)
				SceneManager.LoadScene("MainMenu");
		}
	}
	// Token: 0x0400071A RID: 1818
	private float delay;
}
