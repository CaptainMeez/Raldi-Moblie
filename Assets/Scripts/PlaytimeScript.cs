using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200001E RID: 30
public class PlaytimeScript : MonoBehaviour
{
	public GameObject dropoffPoint;
	public AudioClip waterphone;
	public Transform kidnapPos;
	public GameControllerScript gc;
	public bool gotHammered = false;

	public bool isKidnapped;
	public float kidnapTime = 0f;

	public LayerMask ignoreRaycast;

	public GameObject van;

	// Token: 0x06000068 RID: 104 RVA: 0x00003982 File Offset: 0x00001D82
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.Wander();
		if (GameControllerScript.current.mode == "hard")
			agent.acceleration *= 2;

		if (PlayerPrefs.GetFloat("i_see_you") == 1)
			van.layer = ignoreRaycast;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000039A4 File Offset: 0x00001DA4
	private void Update()
	{
		if (this.coolDown > 0f /*&& !isKidnapped*/)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}

		if (isKidnapped && this.kidnapTime > 1f)
		{
			this.kidnapTime -= 1f * Time.deltaTime;
			gc.player.cc.enabled = false;
			gc.playerTransform.position = kidnapPos.position;
			if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && gotHammered)
				kidnapTime = 0;
			GameControllerScript.current.chipfloke.VanManCatch();
		}
		else
		{
			if (isKidnapped)
			{
				isKidnapped = false;
				gc.playerTransform.position = dropoffPoint.transform.position;
				gc.player.cc.enabled = true;
				this.playCool = 15f;
			}
		}

		if (this.playCool >= 0f)
		{
			this.playCool -= Time.deltaTime;
		}
	}

	public void PlaySound(AudioClip sound)
	{
		this.audioDevice.PlayOneShot(waterphone);
		this.audioDevice.PlayOneShot(sound);
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00003A34 File Offset: 0x00001E34
	private void FixedUpdate()
	{
		if (!this.ps.jumpRope)
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player" & (base.transform.position - this.player.position).magnitude <= 80f & this.playCool <= 0f)
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

	public AudioClip ohShit;

	public void GetCaught()
	{
		audioDevice.PlayOneShot(ohShit);
		kidnapTime = 0f;
		playCool = 30f;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003BCC File Offset: 0x00001FCC
	private void Wander()
	{
		this.wanderer.GetNewTargetHallway();
		this.agent.SetDestination(this.wanderTarget.position);
		this.agent.speed = 35f;
		this.playerSpotted = false;
		this.audVal = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
		if (!this.audioDevice.isPlaying)
		{
			PlaySound(this.aud_Random[this.audVal]);
		}
		this.coolDown = 1f;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00003C60 File Offset: 0x00002060
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.agent.speed = 45f;
		this.coolDown = 0.2f;
		if (!this.playerSpotted)
		{
			this.playerSpotted = true;
			PlaySound(this.aud_LetsPlay);
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00003CD3 File Offset: 0x000020D3
	public void Disappoint()
	{
		this.audioDevice.Stop();
		PlaySound(this.aud_Sad);

		if (FindObjectOfType<PlayerStats>().data.objectasks[0] == 0)
		{
			FindObjectOfType<Objectasks>().CollectObjectask(0);
		}
	}

	public bool db;

	// Token: 0x04000076 RID: 118
	public bool playerSeen;

	// Token: 0x04000077 RID: 119
	public bool disappointed;

	// Token: 0x04000078 RID: 120
	public int audVal;

	// Token: 0x0400007A RID: 122
	public Transform player;

	// Token: 0x0400007B RID: 123
	public PlayerScript ps;

	// Token: 0x0400007C RID: 124
	public Transform wanderTarget;

	// Token: 0x0400007D RID: 125
	public AILocationSelectorScript wanderer;

	// Token: 0x0400007E RID: 126
	public float coolDown;

	// Token: 0x0400007F RID: 127
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

	public AudioClip whereTheFuck;

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
