using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BaldiScript : MonoBehaviour
{
	public AILocationSelectorScript wanderer;
	public bool respectPrivacy = true;
	public float deadTime = 15f;
	public float baseTime;
	public float speed;
	public float timeToMove;
	public float baldiAnger;
	public float baldiTempAnger;
	public float baldiHighCrash = 0.2f;
	public float baldiWait;
	public float baldiSpeedScale;
	public float antiHearingTime;
	public float angerRate;
	public float angerRateRate;
	public float angerFrequency;
	public float timeToAnger;
	public float coolDown;
	public float sayshitcooldown;
	private float moveFrames;
	private float currentPriority;
	private bool sawalready;

	public Transform wanderTarget;

	private Vector3 previous;

	public NavMeshAgent agent;

	private LayerMask layerIgnore;
	public LayerMask neilIgnoreRay;
	public LayerMask baldiIgnoreRay;

	public Animator baldicator;
	public Animator baldiAnimator;

	private AudioSource baldiAudio;

	public AudioClip seeBathroom;
	public AudioClip slap;
	public AudioClip explode;
	public AudioClip BAL_SeePlayer;
	public AudioClip risingSuspense;
	public AudioClip RAL_GetHigh;
	public AudioClip RAL_ACrackpipe;
	public AudioClip[] neillines = new AudioClip[9];
	public AudioClip[] ishaanlines = new AudioClip[9];

	public bool raldiSpeeed = false;
	public bool isDead = false;
	public bool db;
	public bool antiHearing;
	public bool endless;
	private bool playedAudio = false;
	private float finalecheckcooldown = 0;

	public GameObject[] poses;

	public Sprite ishaan;

	private void Start()
	{
		respectPrivacy = !GameControllerScript.current.neilMode;
		sayshitcooldown = UnityEngine.Random.Range(60f,120f);
		baldiHighCrash = 1;
		timeToMove = baseTime;

		if (GameControllerScript.current.neilMode)
			layerIgnore = neilIgnoreRay;
		else
			layerIgnore = baldiIgnoreRay;

		if (PlayerPrefs.GetFloat("no_remorse") == 1)
			respectPrivacy = false;
	}

	private void Awake()
	{
		baldiAudio = base.GetComponent<AudioSource>();
		agent = base.GetComponent<NavMeshAgent>();

		Wander();
	}

	private void Update()
	{
		//print("priority: " + currentPriority);
		if (GameControllerScript.current.neilMode)
		{
			if (sayshitcooldown > 0)
			{
				if (GameControllerScript.current.notebooks >= 2)
					sayshitcooldown -= Time.deltaTime;
			} else {
				if (!GameControllerScript.current.ishaanMode)
					baldiAudio.PlayOneShot(neillines[UnityEngine.Random.Range(0,9)]);
				else
					baldiAudio.PlayOneShot(ishaanlines[UnityEngine.Random.Range(0,9)]);
				sayshitcooldown = UnityEngine.Random.Range(60f,120f);
			}
		}
		if (timeToMove > 0f)
			timeToMove -= 1f * Time.deltaTime;
		if (timeToMove <= 0f || raldiSpeeed)
			Move();

		if (coolDown > 0f)
			coolDown -= 1f * Time.deltaTime;

		if (baldiTempAnger > 0f)
			baldiTempAnger -= 0.02f * Time.deltaTime;
		else
			baldiTempAnger = 0f;

		if (antiHearingTime > 0f)
			antiHearingTime -= Time.deltaTime;
		else
			antiHearing = false;

		if (baldiHighCrash > 0.2f)
			baldiHighCrash -= 1f * Time.deltaTime;
		else
			baldiHighCrash = 0.2f;

		if (isDead && deadTime > 1f)
			deadTime -= Time.deltaTime;
		else if (isDead && deadTime < 1f)
		{
			isDead = false;
			poses[0].SetActive(true);
			poses[1].SetActive(false);
		}

		if (endless)
		{
			if (timeToAnger > 0f)
				timeToAnger -= 1f * Time.deltaTime;
			else
			{
				timeToAnger = angerFrequency;
				GetAngry(angerRate);
				angerRate += angerRateRate;
			}
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
		RaycastHit raycastHit;

		if (moveFrames > 0f)
		{
			moveFrames -= 1f;
			agent.speed = speed;
		}
		else
			agent.speed = 0f;
		
		if ((Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, ~layerIgnore, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player" || (GameControllerScript.current.dogActive && !GameControllerScript.current.player.inbathroom) || GameControllerScript.current.chipfloke.isSolitareConfined) && !GameControllerScript.current.player.inGas)
		{
			if (GameControllerScript.current.player.inbathroom)
			{
				if (!respectPrivacy)
				{
					db = true;
					TargetPlayer();
				} else {
					if (!sawalready)
					{
						Wander();
						timeToMove = 0.05f;
						baldiHighCrash = 3f;
						antiHearing = true;
						antiHearingTime = 3f;
						sawalready = true;

						baldiAudio.PlayOneShot(seeBathroom);
						baldicator.SetTrigger("Bathroom");
					}
				}
			} else {
				sawalready = false;
				db = true;
				TargetPlayer();
			}
		}
		else
		{
			db = false;
			sawalready = false;
		}

		if (!playedAudio && db)
		{
			playedAudio = true;
			if (!(PlayerPrefs.GetFloat("you_never_know") == 1))
			{
				if (!GameControllerScript.current.neilMode)
					baldiAudio.PlayOneShot(BAL_SeePlayer);

				if (SettingsManager.DynamicFOV == 2 && !GameControllerScript.current.player.gameOver)
					GameControllerScript.current.player.playerCameras[0].GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView += 50;
			}
		}
		else if (!db && playedAudio)
			playedAudio = false;
	}

	public void BecomeNeil()
	{
		poses[0].SetActive(false);
		poses[2].SetActive(true);

		if (GameControllerScript.current.ishaanMode)
		{
			poses[2].GetComponent<SpriteRenderer>().sprite = ishaan;
			poses[4].GetComponent<Light>().color = Color.red;
		}
	}

	public void Kill(float time)
	{
		isDead = true;
		deadTime = time;
		baldiAudio.PlayOneShot(explode);

		poses[0].SetActive(false);
		poses[1].SetActive(true);

		IEnumerator Discord()
		{
			yield return new WaitForSeconds(1f);
			GameControllerScript.current.hitmanNotif.SetActive(true);
			yield return new WaitForSeconds(3f);
			GameControllerScript.current.hitmanNotif.SetActive(false);
		}

		StartCoroutine(Discord());
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	public void TargetPlayer(int index = 0)
	{
		agent.SetDestination(GameControllerScript.current.player.transform.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	private void Move()
	{
		if (!isDead)
		{
			if (base.transform.position == previous & coolDown <= 0f)
				Wander();

			moveFrames = 10f;
			
			if (baldiHighCrash > 0.2)
				timeToMove = (baldiWait / UnityEngine.Random.Range(1f, 2f)) / (baldiHighCrash * 5);
			else
				timeToMove = (baldiWait / UnityEngine.Random.Range(1f, 2f));
			if (GameControllerScript.current.mode == "hard")
			{
				if (finalecheckcooldown <= 0)
				{
					//print("checked");
					previous = base.transform.position;
					finalecheckcooldown = 0.5f;
				} else {
					if (raldiSpeeed)
						finalecheckcooldown -= Time.deltaTime;
				}
			} else {
				previous = base.transform.position;
			}
			if (!raldiSpeeed)
			{
				if (!GameControllerScript.current.neilMode)
				{
					baldiAudio.PlayOneShot(slap);
					baldiAnimator.SetTrigger("slap");
				}
			}
		}
	}

	public void GetAngry(float value)
	{
		baldiAnger += value;

		if (baldiAnger < 0.5f)
			baldiAnger = 0.5f;

		baldiWait = (-3f * baldiAnger / (baldiAnger + 2f / baldiSpeedScale) + 3f);

		FindObjectOfType<BaldiCharacterManager>().GetAngry(value);
	}

	public void GetTempAngry(float value)
	{
		baldiTempAnger += value;
	}

	public void Hear(Vector3 soundLocation, float priority, bool glow = false)
	{
		if (!glow)
		{
			if (!antiHearing && (priority >= currentPriority))
			{
				agent.SetDestination(soundLocation);
				currentPriority = priority;

				if (GameControllerScript.current.spoopMode)
				{
					baldicator.SetTrigger("Heard");
					baldicator.SetTrigger("Hear");
				}

				if (GameControllerScript.current.mrBeast.beasting)
				{
					GameControllerScript.current.mrBeast.agent.SetDestination(soundLocation);
					GameControllerScript.current.mrBeast.beasticator.SetTrigger("Heard");
					GameControllerScript.current.mrBeast.beasticator.SetTrigger("Hear");
				}
			} else {
				if (GameControllerScript.current.spoopMode && !antiHearing)
				{
					baldicator.SetTrigger("Hear");
					baldicator.SetTrigger("Unsure");
				}
			}
		}
		else {
			if (GameControllerScript.current.spoopMode)
			{
				baldicator.SetTrigger("Hear");
				baldicator.SetTrigger("Glow");
			}
		}

		FindObjectOfType<BaldiCharacterManager>().Hear(soundLocation, priority);
	}

	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearing = true;
		antiHearingTime = t;
	}
}
