using System;
using UnityEngine;

public class AlarmClockScript : MonoBehaviour
{
	private float timeLeft = 30;
	private float lifeSpan;
	private bool rang;

	public AudioClip ring;
	public AudioSource audioDevice;

	private void Awake()
	{
		float modifier = 1;			
		
		if (PlayerPrefs.GetFloat("make_this_quick") == 1) modifier = 2;

		timeLeft = timeLeft / modifier;

		lifeSpan = timeLeft + 5;
	}

	private void Update()
	{
		if (this.timeLeft >= 0f)
			this.timeLeft -= Time.deltaTime;
		else if (!this.rang)
			this.Alarm();

		if (this.lifeSpan >= 0f)
			this.lifeSpan -= Time.deltaTime;
		else
			UnityEngine.Object.Destroy(base.gameObject, 0f);
	}

	private void Alarm()
	{
		this.rang = true;
		GameControllerScript.current.baldi.Hear(base.transform.position, 8f);
		this.audioDevice.clip = this.ring;
		this.audioDevice.loop = false;
		this.audioDevice.Play();
	}
}
