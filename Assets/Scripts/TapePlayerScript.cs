using System;
using UnityEngine;

public class TapePlayerScript : MonoBehaviour
{
	private AudioSource audioDevice;
	public AudioClip[] funnyVHSs = new AudioClip[5];
	
	public Sprite closedSprite;
	public SpriteRenderer sprite;

	private float timer = 30f;

	private void Start()
	{
		timer = UnityEngine.Random.Range(120f,240f);
		audioDevice = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (PlayerPrefs.GetFloat("radio_tape") == 1 && GameControllerScript.current.spoopMode)
		{
			timer -= Time.deltaTime;
			if (timer < 0 && GameControllerScript.current.baldi.isActiveAndEnabled)
			{
				Play();
				timer = UnityEngine.Random.Range(120f,240f);
			}
		}
		if (audioDevice.isPlaying & Time.timeScale == 0f)
			audioDevice.Pause();
		else if (Time.timeScale > 0f & GameControllerScript.current.baldi.antiHearingTime > 0f)
			audioDevice.UnPause();
	}

	public void Play()
	{
		sprite.sprite = closedSprite;
		audioDevice.clip = funnyVHSs[Mathf.FloorToInt(UnityEngine.Random.Range(0,funnyVHSs.Length))];
		audioDevice.Play();

        if (GameControllerScript.current.baldi.isActiveAndEnabled) 
			GameControllerScript.current.baldi.ActivateAntiHearing(30f);
	}
}
