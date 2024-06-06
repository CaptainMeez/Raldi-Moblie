using System;
using System.Collections;
using System.Linq;

using UnityEngine;

public class SwingingDoorScript : MonoBehaviour
{
	private AudioSource myAudio;

	public MeshCollider barrier;

	public MeshRenderer inside;
	public MeshRenderer outside;

	public GameObject obstacle;

	public Material closed;
	public Material open;
	public Material locked;

	public AudioClip doorOpen;

	private float openTime;
	private float lockTime;

	public bool ignoreRequirement = false;
	public bool bDoorOpen;
	public bool bDoorLocked;
	private bool requirementMet;
	
	private IEnumerator Start()
	{
		myAudio = base.GetComponent<AudioSource>();
		bDoorLocked = true;

		yield return null;

		if (!ignoreRequirement)
		{
			ignoreRequirement = GameControllerScript.current.mode == "hard";
		}
	}

	private void Update()
	{
		if (GameControllerScript.current != null)
		{
			if (!requirementMet && (GameControllerScript.current.notebooks >= 2 || ignoreRequirement))
			{
				requirementMet = true;
				Unlock();
			}
		}
		
		if (openTime > 0f)
			openTime -= 1f * Time.deltaTime;
		if (lockTime > 0f)
			lockTime -= Time.deltaTime;
		else if (bDoorLocked & requirementMet)
			Unlock();
			
		if (openTime <= 0f & bDoorOpen & !bDoorLocked)
			Close();
	}

	public void Open()
	{
		if (!bDoorLocked)
		{
			bDoorOpen = true;
			inside.material = open;
			outside.material = open;
			openTime = 2f;
		}
	}

	public void PlayerOpen()
	{
		myAudio.PlayOneShot(doorOpen, 1f);

		if (GameControllerScript.current.baldi.isActiveAndEnabled)
		{
			FindObjectOfType<PlayerScript>().playerAnimator.speed = 1f;
			FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("OpenSwingDoor");
			GameControllerScript.current.baldi.Hear(base.transform.position, 1f);
		}

		Open();
	}

	private void Close()
	{
		bDoorOpen = false;
		inside.material = closed;
		outside.material = closed;
	}

	public void Lock(float time)
	{
		barrier.enabled = true;
		obstacle.SetActive(true);
		bDoorLocked = true;
		lockTime = time;
		inside.material = locked;
		outside.material = locked;
	}

	private void Unlock()
	{
		barrier.enabled = false;
		obstacle.SetActive(false);
		bDoorLocked = false;
		Close();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!bDoorLocked && !bDoorOpen)
			{
				if (GameControllerScript.current != null)
				{
					if (GameControllerScript.current.notebooks >= 2 || ignoreRequirement)
						PlayerOpen();
				}
				else
					Open();
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag != "Player") // Ignore player open, since player has his own condition in OnTriggerEnter.
			Open();
	}
}
