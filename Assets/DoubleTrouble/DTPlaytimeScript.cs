using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200001E RID: 30
public class DTPlaytimeScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.Wander();
	}

	private void Update()
	{
		if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;

		if (this.playCool >= 0f)
			this.playCool -= Time.deltaTime;
		else if (this.animator.GetBool("disappointed"))
		{
			this.playCool = 0f;
			this.animator.SetBool("disappointed", false);
		}
	}

	private void FixedUpdate()
	{
		if (!GameControllerScript.current.player.dt_jumpRope)
		{
			Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
			RaycastHit raycastHit;

			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player" && (base.transform.position - GameControllerScript.current.player.transform.position).magnitude <= 80f && this.playCool <= 0f)
			{
				this.playerSeen = true;
				this.TargetPlayer();
			}
			else if (this.playerSeen & this.coolDown <= 0f)
			{
				this.playerSeen = false;
				this.Wander();
			}
			else if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
			{
				this.Wander();
			}

			this.jumpRopeStarted = false;
		}
		else
		{
			if (!this.jumpRopeStarted)
			{
				this.agent.Warp(base.transform.position - base.transform.forward * 10f);
			}

			this.jumpRopeStarted = true;
			this.agent.speed = 0f;
			this.playCool = 15f;
		}
	}

	private void Wander()
	{
		this.wanderer.GetNewTargetHallway();
		this.agent.SetDestination(this.wanderTarget.position);
		this.agent.speed = 15f;
		this.playerSpotted = false;
		this.audVal = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));

		if (!this.audioDevice.isPlaying)
			this.audioDevice.PlayOneShot(this.aud_Random[this.audVal]);

		this.coolDown = 1f;
	}


	private void TargetPlayer()
	{
		this.animator.SetBool("disappointed", false);
		this.agent.SetDestination(GameControllerScript.current.player.transform.position);
		this.agent.speed = 20f;
		this.coolDown = 0.2f;

		if (!this.playerSpotted)
		{
			this.playerSpotted = true;
			this.audioDevice.PlayOneShot(this.aud_LetsPlay);
		}
	}

	public void Disappoint()
	{
		this.animator.SetBool("disappointed", true);
		this.audioDevice.Stop();
		this.audioDevice.PlayOneShot(this.aud_Sad);
	}

	public bool playerSeen;

	public int audVal;

	public Animator animator;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	public float playCool;

	// Token: 0x04000080 RID: 128
	public bool playerSpotted;

	// Token: 0x04000081 RID: 129
	public bool jumpRopeStarted;

	// Token: 0x04000082 RID: 130
	private NavMeshAgent agent;

	// Token: 0x04000083 RID: 131
	public AudioClip[] aud_Numbers = new AudioClip[10];

	// Token: 0x04000084 RID: 132
	public AudioClip[] aud_Random = new AudioClip[2];

	// Token: 0x04000085 RID: 133
	public AudioClip aud_Instrcutions;

	// Token: 0x04000086 RID: 134
	public AudioClip aud_Oops;

	// Token: 0x04000087 RID: 135
	public AudioClip aud_LetsPlay;

	// Token: 0x04000088 RID: 136
	public AudioClip aud_Congrats;

	// Token: 0x04000089 RID: 137
	public AudioClip aud_ReadyGo;

	// Token: 0x0400008A RID: 138
	public AudioClip aud_Sad;

	// Token: 0x0400008B RID: 139
	public AudioSource audioDevice;
}
