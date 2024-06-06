using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeastScript : MonoBehaviour
{
	private GameControllerScript gc;
	private AudioSource audioPlayer;

	public GameObject quarter;
	public GameObject spawnLocation;
	public GameObject mrBeast;
	private GameObject clone;
	public float DefaultSpawnTime = 10;
	public float SpawnTime = 10;
	public float roomCheckTime = 60f;
	public AudioClip mrBeastPissed;
	public AudioClip mrBeastPissedChallenge;
	public AudioClip mrBeastwait;
	public AudioClip mrbeastMegaloLmao;
	public GameObject playerCamera;
	public GameObject cutsceneCamera;
	public GameObject cafeteriaCamera;
	public GameObject beastJumpscareCamera;

	private void Start()
	{
		if (PlayerPrefs.GetFloat("spilling_wallet") == 1)
			DefaultSpawnTime = 5;
		gc = GameControllerScript.current;
		audioPlayer = GetComponent<AudioSource>();
		this.agent = base.GetComponent<UnityEngine.AI.NavMeshAgent>();
		this.Wander();

		if (PlayerPrefs.GetFloat("StreamerMode") == 2)
		{
			music.clip = laMusic;
			music.Play();
		}
	}

	private void Update()
	{
		gc = GameControllerScript.current;
		if (inChallenge)
			checkingRoom = false;
        if (this.coolDown > 0f)
			this.coolDown -= 1f * Time.deltaTime;

		if (roomCheckTime > 0f)
			roomCheckTime -= Time.deltaTime;


		if (this.challengeTime > 1f)
			this.challengeTime -= 1f * Time.deltaTime;
        else if (inChallenge)
        {
            inChallenge = false;
            gc.audioDevice.PlayOneShot(congrats);
			gc.GiveScore(500);
			gc.GiveRewardItem();

            challenge.SetActive(false);

            coolDown = 30f;
        }
	}

	public bool gettingMad = false;
	public bool beasting = false;
	public Animator beasticator;
	public AudioSource music;
	public AudioClip laMusic;

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		SpawnTime -= Time.deltaTime;

		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player")
		{
            if (!beasting && !inChallenge && (coolDown <= 0) && !checkingRoom && !(this.challengeTime > 1f))
			{
				//print("hey, do you want to participate in my youtubea challenge");
				if (!db)
                source.PlayOneShot(hey);

				this.db = true;
				this.TargetPlayer();
			}
			else if (beasting)
			{
				this.TargetPlayer();
			}
		}
		else
		{
			this.db = false;
			if (this.agent.velocity.magnitude <= 1f)
			{
				this.Wander();

				if (checkingRoom && Vector3.Distance(new Vector3(base.transform.position.x, beastRoom.position.y, base.transform.position.z), beastRoom.position) < 2f)
				{
					checkingRoom = false;

					if (gc.beastCardCollected && !gettingMad)
					{
						gettingMad = true;

						float ogSpeed = agent.speed;

						agent.speed = 0;

						IEnumerator WaitTime()
						{
							music.Stop();

							GameControllerScript.current.hud.SetActive(false);

							FindObjectOfType<PlayerStats>().Save();

							playerCamera.SetActive(false);
							cutsceneCamera.SetActive(true);
							gc.baldiTutor.SetActive(false);
							gc.baldi.gameObject.SetActive(false);
							gc.chipfloke.gameObject.SetActive(false);
							gc.crafters.gameObject.SetActive(false);
							gc.vanman.gameObject.SetActive(false);
							gc.gottaWeep.gameObject.SetActive(false);
							gc.bully.gameObject.SetActive(false);
							gc.firstPrize.gameObject.SetActive(false);
							gc.beans.gameObject.SetActive(false);
							gc.present.gameObject.SetActive(false);
							gc.polishCow.gameObject.SetActive(false);

							gc.player.enabled = false;
							gc.allowEvents = false;

							audioPlayer.PlayOneShot(mrBeastPissed);

							yield return new WaitForSeconds(mrBeastPissed.length + 0.5f);
							
							if (gc.notebooks < 10)
							{
								gc.beastlovania.clip = mrbeastMegaloLmao;
								gc.beastlovania.Play();
							}

							playerCamera.SetActive(true);
							cutsceneCamera.SetActive(false);

							gc.ShowChar();
							gc.present.gameObject.SetActive(false);
							gc.player.enabled = true;
							agent.speed = ogSpeed * 1.4f;

							beasting = true;
							GameControllerScript.current.RemoveScore(3000);

							GameControllerScript.current.hud.SetActive(true);
						}

						StartCoroutine(WaitTime());
					}
				}
			}
		}
		if (SpawnTime <= 0) {
			clone = Instantiate(quarter, new Vector3(mrBeast.transform.position.x,4,mrBeast.transform.position.z), mrBeast.transform.rotation, spawnLocation.transform);
			clone.SetActive(true);
			SpawnTime = DefaultSpawnTime;
		}
	}

	public Transform beastRoom;
	public bool checkingRoom = false;
	public bool killingPlayer = false;
	public AudioClip mrBeastPlayerKill;
	public Animator solitareTransition;
	public Material playerOrangeShirt;
	public MeshRenderer playerShirtSprites;
	public GameObject solitareText;
	public AudioClip badCredits;

	private void Wander()
	{
		this.wanderer.GetNewTarget();
		
		if (roomCheckTime <= 0f)
		{
			agent.SetDestination(beastRoom.position);	
			roomCheckTime = 60f;	
			checkingRoom = true;
		}
		else if (!checkingRoom)
		{
			this.agent.SetDestination(this.wanderTarget.position);	
		}
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
			for (int i = 0; i < 5; i++)
			{
				if (GameControllerScript.current.item[i] == 24) {
					Animator[] theAnimators = GameControllerScript.current.explosionAnimators;

                    if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                        theAnimators = GameControllerScript.current.basicExplosionAnimators;
					
					Animator theAnimator = GameControllerScript.current.hotbarAnimator;

                    if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                        theAnimator = GameControllerScript.current.basicHotbarAnimator;

                    theAnimator.SetTrigger("kaboom");
					GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.robloxrocketsound);
					GameControllerScript.current.LoseItem(i);
				    theAnimators[i].SetTrigger("kaboom");
				}
			}
			if (!beasting && !inChallenge && (coolDown <= 0) && !checkingRoom && !(this.challengeTime > 1f) && !(gc.beastCardCollected || PlayerPrefs.GetFloat("mrbeasts_bad_day") == 1))
			{
				GameControllerScript.current.vanman.kidnapTime = 0f;

				inChallenge = true;
				agent.Warp(new Vector3(-75f, 0f, 305f));

				this.cc.enabled = false;
				other.transform.position = new Vector3(-75f, 4f, 285f);
				this.cc.enabled = true;

				source.PlayOneShot(challengeInstruct);

				challengeTime = 15f;

				challenge.SetActive(true);
			}
            else if (!killingPlayer && beasting)
			{
				agent.speed = 0f;
				killingPlayer = true;
				gc.player.enabled = false;
				playerCamera.SetActive(false);
				beastJumpscareCamera.SetActive(true);
				audioPlayer.PlayOneShot(mrBeastPlayerKill, 3f);
				solitareTransition.gameObject.SetActive(true);
				other.transform.position = new Vector3(-75f, -2000f, 285f);
				gc.beastlovania.Stop();
				
				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(1.5f);
					audioPlayer.Stop();
					yield return new WaitForSeconds(0.5f);
					agent.speed = 20f;
					beastJumpscareCamera.SetActive(false);
					playerCamera.SetActive(true);
					FindObjectOfType<PrincipalScript>().TriggerSolitare(player);
					solitareText.SetActive(true);

					foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
					{
						if (sources.gameObject.name != "Principal of the Thing" && sources.gameObject.name != "SchoolMusic")
						{
							sources.mute = true;
						}
					}

					gc.schoolMusic.clip = badCredits;
					gc.schoolMusic.loop = false;
					gc.schoolMusic.Play();

					yield return new WaitForSeconds(6);
					solitareTransition.gameObject.SetActive(false);
				}

				StartCoroutine(WaitTime());
			} 
			else if ((gc.beastCardCollected || PlayerPrefs.GetFloat("mrbeasts_bad_day") == 1) && !gettingMad)
			{
				GameControllerScript.current.vanman.kidnapTime = 0f;

				agent.Warp(new Vector3(-75f, 0f, 305f));

				this.cc.enabled = false;
				other.transform.position = new Vector3(-75f, 4f, 285f);
				this.cc.enabled = true;

				gettingMad = true;

				float ogSpeed = agent.speed;

				agent.speed = 0;

				IEnumerator WaitTime()
				{
					source.PlayOneShot(challengeInstruct);
					yield return new WaitForSeconds(1f);
					source.Stop();
					source.PlayOneShot(mrBeastwait);
					music.Stop();
					gc.player.inNotebook = true;
					yield return new WaitForSeconds(1f);

					FindObjectOfType<PlayerStats>().Save();
					
					GameControllerScript.current.hud.SetActive(false);

					playerCamera.SetActive(false);
					cafeteriaCamera.SetActive(true);
					gc.baldi.gameObject.SetActive(false);
					gc.chipfloke.gameObject.SetActive(false);
					gc.crafters.gameObject.SetActive(false);
					gc.vanman.gameObject.SetActive(false);
					gc.gottaWeep.gameObject.SetActive(false);
					gc.bully.gameObject.SetActive(false);
					gc.firstPrize.gameObject.SetActive(false);
					gc.beans.gameObject.SetActive(false);
					gc.present.gameObject.SetActive(false);
					gc.polishCow.gameObject.SetActive(false);

					gc.player.enabled = false;
					gc.allowEvents = false;

					audioPlayer.PlayOneShot(mrBeastPissedChallenge);

					yield return new WaitForSeconds(mrBeastPissedChallenge.length + 0.5f);
							
					if (gc.notebooks < 10 && gc.mode != "hard")
					{
						gc.beastlovania.clip = mrbeastMegaloLmao;
						gc.beastlovania.Play();
					}

					playerCamera.SetActive(true);
					cafeteriaCamera.SetActive(false);

					gc.ShowChar();
					gc.present.gameObject.SetActive(false);
					gc.player.enabled = true;
					gc.player.inNotebook = false;
					agent.speed = ogSpeed * 1.4f;

					beasting = true;
					if (!FindObjectOfType<ATMScript>().canInteract)
						GameControllerScript.current.RemoveScore(3000);

					GameControllerScript.current.hud.SetActive(true);
				}

				StartCoroutine(WaitTime());
			}
		}
    }

    public CharacterController cc;
    public bool inChallenge = false;

	// Token: 0x04000676 RID: 1654
	public bool db;

	// Token: 0x04000677 RID: 1655
	public Transform player;

	// Token: 0x04000678 RID: 1656
	public Transform wanderTarget;

    public GameObject challenge;

	// Token: 0x04000679 RID: 1657
	public AILocationSelectorScript wanderer;

	// Token: 0x0400067A RID: 1658
	public float challengeTime = 0f;
    public float coolDown;

	// Token: 0x0400067B RID: 1659
	public UnityEngine.AI.NavMeshAgent agent;

    public AudioSource source;
    public AudioClip hey;
    public AudioClip challengeInstruct;
    public AudioClip congrats;
}
