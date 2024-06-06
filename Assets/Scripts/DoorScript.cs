using System;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class DoorScript : MonoBehaviour
{
	private void Awake()
	{
		myAudio = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (lockTime > 1f)
			lockTime -= 1f * Time.deltaTime;
		else if (bDoorLocked)
			UnlockDoor();

		if (openTime > 0f)
			openTime -= 1f * Time.deltaTime;

		if (openTime <= 0f & bDoorOpen)
		{
			barrier.enabled = true;
			invisibleBarrier.enabled = true;
			bDoorOpen = false;
			inside.material = closed;
			outside.material = closed;

			if (silentOpens <= 0)
				myAudio.PlayOneShot(doorClose, 1f);
		}

		if (RaldiInputManager.current.GetInteractDown() && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			PlayerScript player = null;
			PlayerMovementScript movement = null;
			Transform targetTransform;
			
			bool isNormal = true;

			// Account for scenes using exclusively PlayerMovementScript
			if (FindObjectOfType<PlayerMovementScript>() == null)
			{
				if (GameControllerScript.current != null)
					player = GameControllerScript.current.player;
				else
					player = FindObjectOfType<PlayerScript>();

				targetTransform = player.transform;
			}
			else
			{
				movement = FindObjectOfType<PlayerMovementScript>();
				targetTransform = movement.transform;
				isNormal = false;
			}
			

			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.transform.gameObject == gameObject && Vector3.Distance(targetTransform.transform.position, base.transform.position) < openingDistance))
			{	
				if (!bDoorLocked)
				{
					OpenDoor();

					if (GameControllerScript.current != null)
					{
						if (GameControllerScript.current.baldi.isActiveAndEnabled & silentOpens <= 0)
							GameControllerScript.current.baldi.Hear(base.transform.position, 1f);
					}
						
					if (silentOpens > 0)
						silentOpens--;
						

					if (isNormal)
					{
						player.playerAnimator.speed = 1f;
						player.playerAnimator.SetTrigger("OpenDoor");
					}
					else
					{
						movement.playerAnimator.speed = 1f;
						movement.playerAnimator.SetTrigger("OpenDoor");
					}
				}
				else
				{
					myAudio.PlayOneShot(doorLockOpen, 1f);
				}
			}
		}
	}

	public void OpenDoor()
	{		
		if (silentOpens <= 0 && !bDoorOpen)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
		}
		barrier.enabled = false;
		invisibleBarrier.enabled = false;
		bDoorOpen = true;
		inside.material = open;
		outside.material = open;
		openTime = 3f;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!bDoorLocked && (other.CompareTag("NPC") || (other.tag == "Player" && GameControllerScript.current.player.isMonke && Input.GetButton("Run"))))
		{
			OpenDoor();
		} else if (!bDoorLocked && other.tag == "Player" && openTime < 0.1)
		{
			openTime = 0.1f;
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00021404 File Offset: 0x0001F804
	public void LockDoor(float time)
	{
		bDoorLocked = true;
		lockTime = time;

		myAudio.PlayOneShot(doorLock, 1f);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00021414 File Offset: 0x0001F814
	public void UnlockDoor()
	{
		bDoorLocked = false;

		myAudio.PlayOneShot(doorUnlock, 1f);
	}

	// Token: 0x17000396 RID: 918
	public bool DoorLocked
	{
		get
		{
			return bDoorLocked;
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00021425 File Offset: 0x0001F825
	public void SilenceDoor()
	{
		silentOpens = 4;
	}

	// Token: 0x040005C3 RID: 1475
	public float openingDistance;
	// Token: 0x040005C6 RID: 1478
	public MeshCollider barrier;

	// Token: 0x040005C7 RID: 1479
	public MeshCollider trigger;

	// Token: 0x040005C8 RID: 1480
	public MeshCollider invisibleBarrier;

	// Token: 0x040005C9 RID: 1481
	public MeshRenderer inside;

	// Token: 0x040005CA RID: 1482
	public MeshRenderer outside;

	// Token: 0x040005CB RID: 1483
	public AudioClip doorOpen;

	// Token: 0x040005CC RID: 1484
	public AudioClip doorClose;

	public AudioClip doorLockOpen;
	public AudioClip doorUnlock;
	public AudioClip doorLock;

	// Token: 0x040005CD RID: 1485
	public Material closed;

	// Token: 0x040005CE RID: 1486
	public Material open;

	// Token: 0x040005CF RID: 1487
	private bool bDoorOpen;

	// Token: 0x040005D0 RID: 1488
	private bool bDoorLocked;

	// Token: 0x040005D1 RID: 1489
	public int silentOpens;

	// Token: 0x040005D2 RID: 1490
	private float openTime;

	// Token: 0x040005D3 RID: 1491
	public float lockTime;

	// Token: 0x040005D4 RID: 1492
	private AudioSource myAudio;
}
