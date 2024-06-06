using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000C7 RID: 199
public class PolishCow : MonoBehaviour
{
	public AudioClip vocals;

	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		Wander();
		coolDown = 5f;

		if (FindObjectOfType<GameControllerScript>().mode == "hard")
			agent.speed += 5f;

		if (GameControllerScript.current.mode == "story_cow")
		{
			base.GetComponent<AudioSource>().clip = vocals;
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void Update()
	{
		if (this.coolDown > 0f)
			this.coolDown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;

		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player" && coolDown <= 0f)
		{
			this.db = true;
			this.TargetPlayer();
		}
		else
		{
			this.db = false;

			if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
				Wander();
		}
	}

	public AudioClip drop;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && coolDown <= 0f)
		{
			Vector3 dropPos;
			GameObject marker;
			GameControllerScript.current.vanman.kidnapTime = 0f;
			GameObject[] markers = GameObject.FindGameObjectsWithTag("PMarker");
			
			if (markers.Length > 0)
			{
				int yPos = 4;

				marker = markers[Mathf.RoundToInt(UnityEngine.Random.Range(0.0f, markers.Length - 1))];

				if (marker.transform.position.y < -1) // assuming your downstairs
				{
					yPos = yPos - 50;
				}

				dropPos = new Vector3(marker.transform.position.x, yPos, marker.transform.position.z);
				marker.GetComponent<PolishMarkerScript>().Break();
			} 
			else 
			{
				Transform wander = FindObjectOfType<AILocationSelectorScript>().transform;

				FindObjectOfType<AILocationSelectorScript>().GetNewTarget();
					
				dropPos = new Vector3(wander.position.x, player.position.y, wander.position.z);
			}

			coolDown = 60f;

			player.GetComponent<PlayerScript>().cc.enabled = false;
			player.position = dropPos;
			player.GetComponent<PlayerScript>().cc.enabled = true;

			GameControllerScript.current.audioDevice.PlayOneShot(drop);

			db = false;

			GameControllerScript.current.player.OnPlayerTeleport();

			Wander();
		}
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0002440D File Offset: 0x0002280D
	private void Wander()
	{
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0002443C File Offset: 0x0002283C
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

	// Token: 0x04000676 RID: 1654
	public bool db;

	// Token: 0x04000677 RID: 1655
	public Transform player;

	// Token: 0x04000678 RID: 1656
	public Transform wanderTarget;

	// Token: 0x04000679 RID: 1657
	public AILocationSelectorScript wanderer;

	// Token: 0x0400067A RID: 1658
	public float coolDown;

	// Token: 0x0400067B RID: 1659
	private NavMeshAgent agent;
}
