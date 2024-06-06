using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class Script : MonoBehaviour
{
	public GameObject doorTrigger;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" & !this.played)
		{
			this.audioDevice.Play();

			played = true;
			
			IEnumerator NeilTalking()
			{
				yield return new WaitForSeconds(audioDevice.clip.length);
				doorTrigger.SetActive(true);
			}

			StartCoroutine(NeilTalking());
		}
	}

	public AudioSource audioDevice;
	private bool played;
}
