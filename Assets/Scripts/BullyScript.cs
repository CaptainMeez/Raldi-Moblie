using System;
using UnityEngine;

public class BullyScript : MonoBehaviour
{
	private void Start()
	{
		audioDevice = base.GetComponent<AudioSource>();
		waitTime = UnityEngine.Random.Range(60f, 120f);
	}

	private void Update()
	{
		if (waitTime > 0f)
			waitTime -= Time.deltaTime;
		else if (!active)
			Activate();

		if (active)
		{
			activeTime += Time.deltaTime;

			if (activeTime >= 180f & (base.transform.position - GameControllerScript.current.player.transform.position).magnitude >= 120f)
				Reset(false);
		}

		if (guilt > 0f)
			guilt -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
		RaycastHit raycastHit;

		if (Physics.Raycast(base.transform.position + new Vector3(0f, 4f, 0f), direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player" & (base.transform.position - GameControllerScript.current.player.transform.position).magnitude <= 30f & active)
		{
			if (!spoken)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Taunts[num]);
				spoken = true;
			}

			guilt = 10f;
		}
	}

	private void Activate()
	{
		wanderer.GetNewTargetHallway();
		base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);

		while ((base.transform.position - GameControllerScript.current.player.transform.position).magnitude < 20f)
		{
			wanderer.GetNewTargetHallway();
			base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);
		}

		active = true;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			if (GameControllerScript.current.item[0] == 0 & GameControllerScript.current.item[1] == 0 & GameControllerScript.current.item[2] == 0 & GameControllerScript.current.item[3] == 0 & GameControllerScript.current.item[4] == 0)
				audioDevice.PlayOneShot(aud_Denied);
			else if (!GameControllerScript.current.IteminInventory(17))
			{
				int attempts = 0;
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
				bool failed;
				while (GameControllerScript.current.item[num] == 0 && attempts < 1000)
				{
					num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
					attempts += 1;
				}
				failed = attempts == 1000;
				if (!failed)
				{
					GameControllerScript.current.LoseItem(num);
					int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
					audioDevice.PlayOneShot(aud_Thanks[num2]);
					Reset(false);
				} else {
					audioDevice.PlayOneShot(aud_Denied);
				}
			} 
			else 
			{
				GameControllerScript.current.RemoveItem(17);
				Reset(true);

				if (FindObjectOfType<PlayerStats>().data.objectasks[6] == 0)
					FindObjectOfType<Objectasks>().CollectObjectask(6);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Principal of the Thing" & guilt > 0f)
			Reset(false);
	}

	private void Reset(bool pickedup)
	{
		base.transform.position = base.transform.position - new Vector3(0f, 20f, 0f);

		if (!pickedup)
			waitTime = UnityEngine.Random.Range(60f, 120f);
		else 
			waitTime = UnityEngine.Random.Range(120f, 240f);
		
		
		active = false;
		activeTime = 0f;
		spoken = false;
	}

	public Transform wanderTarget;
	public AILocationSelectorScript wanderer;

	public float waitTime;
	public float activeTime;
	public float guilt;

	public bool active;
	public bool spoken;

	// Token: 0x0400001C RID: 28
	private AudioSource audioDevice;

	public AudioClip[] aud_Taunts = new AudioClip[2];
	public AudioClip[] aud_Thanks = new AudioClip[2];

	public AudioClip aud_Denied;
}
