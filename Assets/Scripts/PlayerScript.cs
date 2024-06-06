using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using TMPro;

using Cinemachine;

using Rewired;

public class PlayerScript : MonoBehaviour
{
	public CinemachineVirtualCamera playerVCam;
	
	public NavMeshAgent diarrheaAI;

	public AudioSource audioSource;
	public AudioSource mockSound;

	public Animator playerAnimator;
	public Animator monkeAnimator;
	public Transform holdPos;

	public float interactDistance = 15f;
	public float poopMultiplier = 1f;
	public float poopCooldown = 150f;
	public float diarrheaFartCooldown = 0f;
	public float outsidetime;
	public float bathroomtime;
	private float immunitytime;
	private float defaultFOV;
	private float targetFOV;

	public TextMeshProUGUI staminaText;

	public SpriteRenderer itemHold;
	
	public AudioClip badPoop;
	public AudioClip risingAction;
	public AudioClip impact;
	public AudioClip monkeTransition;
	public AudioClip monkeLoop;

	public GameObject pickupPrefab;
	public GameObject gumScreen;
	public GameObject poopArea;
	public GameObject diarrheaCamera;
	public GameObject poopTrail;
	public GameObject diarrheaText;

	public bool inNotebook;
	public bool justPooped = false;
	public bool washedHands = true;
	public bool do3DMovement = false;
	public bool inGas = false;
	public bool playerCanPoop = true;
	public bool staminaByDefault = false;
	public bool infiniteStamina = false;
	public bool diarTimeStarted = false;
	public bool diarrheaing = false;
	public bool hasToPoop;
	public bool outside;
	public bool inbathroom;
	public bool allowDiarrhea = true;
	public bool isMonke = false;
	public bool allowMockRunning = false;
	private bool mocking = false;

	public GameObject[] playerCameras;
	public AudioClip[] farts;
	public float[] DefaultVariables = new float[2];

	public GameObject[] playerModels;

	public bool gameOver;
	public bool jumpRope;
	public bool dt_jumpRope;
	public bool sweeping;
	public bool hugging;
	public bool bootsActive;
	public int principalBugFixer;
	public float sweepingFailsave;
	public float fliparoo;
	public float flipaturn;
	private Quaternion playerRotation;
	public Vector3 frozenPosition;

	public float mouseSensitivity;
	public float walkSpeed;
	public float runSpeed;
	public float maxStamina;
	public float staminaRate;
	public float guilt;
	private float mockModifier = 1f;

	private Vector3 moveDirection;

	// Token: 0x04000705 RID: 1797
	private float playerSpeed;

	// Token: 0x04000706 RID: 1798
	public float stamina;

	// Token: 0x04000707 RID: 1799
	public CharacterController cc;

	// Token: 0x04000708 RID: 1800
	public NavMeshAgent gottaSweep;

	// Token: 0x04000709 RID: 1801
	public NavMeshAgent firstPrize;

	// Token: 0x0400070A RID: 1802
	public Transform firstPrizeTransform;

	// Token: 0x0400070D RID: 1805
	public string guiltType;

	// Token: 0x0400070E RID: 1806
	public GameObject jumpRopeScreen;

	private Player input;

	private void Awake()
	{
		if (PlayerPrefs.GetFloat("whos_the_drug_now") == 1)
		{
			walkSpeed = walkSpeed * 2;
			runSpeed = runSpeed * 2;
		}
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
		defaultFOV = PlayerPrefs.GetFloat("FOV");
		targetFOV = PlayerPrefs.GetFloat("FOV");

		input = ReInput.players.GetPlayer(0);
		
		stamina = maxStamina;
		playerRotation = base.transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
		principalBugFixer = 1;
		flipaturn = 1f;
		DefaultVariables[0] = walkSpeed;
		DefaultVariables[1] = runSpeed;

		if (FindObjectOfType<GameControllerScript>() != null)
			FindObjectOfType<GameControllerScript>().itemSprite = itemHold;

		if (PlayerPrefs.GetFloat("wheres_the_bathroom") == 1)
		{
			poopCooldown = -1f;
			hasToPoop = true;
		}
	}

	public Transform dropPos;
	private PickupScript spawnitem;
	public void DropItem(int itemIndex)
	{
		if (GameControllerScript.current.item[itemIndex] != 0)
		{
			spawnitem = GameObject.Instantiate<GameObject>(pickupPrefab, new Vector3(dropPos.position.x, 4, dropPos.position.z), dropPos.rotation).GetComponent<PickupScript>();
			spawnitem.id = GameControllerScript.current.item[itemIndex];
			spawnitem.droppedItem = true;
			GameControllerScript.current.LoseItem(itemIndex);
		}
	}

	bool bump = false;
	public Animator cameraHolder;
	public Animator cameraAnimator;

	public void BumpCamera()
	{
		if (!bump)
			cameraHolder.Play("CameraBumpRight");
		else
			cameraHolder.Play("CameraBumpLeft");

		bump = !bump;
	} 

	public void ResetSpeed()
	{
		walkSpeed = DefaultVariables[0];
		runSpeed = DefaultVariables[1];
	}

	private void Update()
	{
		// Movement
		if (!diarTimeStarted && (!inNotebook)) { MouseMove(); if (!autoWalk) PlayerMove(); }
		else if (inNotebook) bugfix = true;

		// Stamina and Guilt
		StaminaCheck();
		GuiltCheck();

		// Update player FOV
		playerVCam.m_Lens.FieldOfView = Mathf.Lerp(playerVCam.m_Lens.FieldOfView, targetFOV * mockModifier / (stamina <= 0 ? 1.3f : 1), 0.1f);

		// Player Immunity
		if (immunitytime > 0)
			immunitytime -= Time.deltaTime;
		else
			immunitytime = 0;

		// Outside Time
		if (outsidetime > 0) 
		{
			outsidetime -= Time.deltaTime;
			outside = true;
		} 
		else 
			outside = false;

		// Bathroom Time
		if (bathroomtime > 0) 
		{
			bathroomtime -= Time.deltaTime;
			inbathroom = true;
		} 
		else 
			inbathroom = false;

		// Auto walk forward
		if (autoWalk)
		{
			playerSpeed = walkSpeed;
			playerSpeed *= Time.deltaTime;
			moveDirection = (base.transform.forward).normalized * playerSpeed;
			cc.Move(moveDirection);
		}
			
		if ((jumpRope || dt_jumpRope) && (base.transform.position - frozenPosition).magnitude >= 1f)
			DeactivateJumpRope();

		if (sweepingFailsave > 0f)
			sweepingFailsave -= Time.deltaTime;
		else
		{
			sweeping = false;
			hugging = false;
		}

		if (diarrheaing && diarrheaFartCooldown > 0f && playerCanPoop && !FindObjectOfType<ObjectasksScreen>().inmenu && !inNotebook && !GameControllerScript.current.gamePaused)
			diarrheaFartCooldown -= Time.deltaTime;
		else if (diarrheaing && diarrheaFartCooldown < 0f && playerCanPoop && !FindObjectOfType<ObjectasksScreen>().inmenu && !inNotebook && !GameControllerScript.current.gamePaused)
		{
			if (!(PlayerPrefs.GetFloat("DisableFarts") == 2))
				audioSource.PlayOneShot(farts[UnityEngine.Random.Range(0, farts.Length)]);

			diarrheaFartCooldown = 0.05f;
		}

		if (poopCooldown > 0f && !GameControllerScript.current.gamePaused && !GameControllerScript.current.learningActive && (GameControllerScript.current.spoopMode || PlayerPrefs.GetFloat("wheres_the_bathroom") == 1) && playerCanPoop && !FindObjectOfType<ObjectasksScreen>().inmenu && !inNotebook && !GameControllerScript.current.chipfloke.isSolitareConfined && !GameControllerScript.current.gamePaused)
			poopCooldown -= Time.deltaTime;
		else if (poopCooldown < 0f && playerCanPoop && !FindObjectOfType<ObjectasksScreen>().inmenu && !inNotebook && !GameControllerScript.current.gamePaused)
		{
			hasToPoop = true;

			if (!(PlayerPrefs.GetFloat("blind") == 1))
			{
				foreach(GameObject highlights in GameControllerScript.current.bathroomHighlights)
				{
					highlights.SetActive(true);
				}
			}

			if (allowDiarrhea)
				poopMultiplier += 0.0001f;
		}

		if (isMonke && hasToPoop && !inNotebook && !GameControllerScript.current.gamePaused)
			{
				hasToPoop = false;

				GameJolt.API.Trophies.TryUnlock(184396);

				if (!(PlayerPrefs.GetFloat("blind") == 1))
				{
					foreach(GameObject highlights in GameControllerScript.current.bathroomHighlights)
					{
						highlights.SetActive(false);
					}
				}
				
				poopCooldown = 30f;

				if (!(PlayerPrefs.GetFloat("DisableFarts") == 2))
				{
					audioSource.PlayOneShot(farts[UnityEngine.Random.Range(0, farts.Length)]);
					audioSource.PlayOneShot(farts[UnityEngine.Random.Range(0, farts.Length)]);
				}
				
				audioSource.PlayOneShot(badPoop);
				
				GameObject.Instantiate<GameObject>(poopArea, new Vector3(base.transform.position.x, 5, base.transform.position.z), base.transform.rotation);

				IEnumerator WaitTime()
				{
					poopTrail.SetActive(true);
					yield return new WaitForSeconds(5);
					poopTrail.SetActive(false);
				}

				StartCoroutine(WaitTime());
			}

		if (poopMultiplier > 3 && !diarTimeStarted && playerCanPoop && !FindObjectOfType<ObjectasksScreen>().inmenu && !inNotebook && !GameControllerScript.current.gamePaused)
			{
				GameControllerScript.current.HideChar();

				diarTimeStarted = true;

				cc.enabled = false;
				
				foreach (GameObject sanePersonCamera in playerCameras)
				{
					sanePersonCamera.SetActive(false);
				}

				diarrheaCamera.SetActive(true);
				diarrheaText.SetActive(true);

				IEnumerator WaitTime()
				{
					audioSource.PlayOneShot(impact);
					yield return new WaitForSeconds(1f);
					audioSource.PlayOneShot(impact);
					yield return new WaitForSeconds(1f);
					audioSource.PlayOneShot(impact);

					audioSource.PlayOneShot(risingAction);
					yield return new WaitForSeconds(risingAction.length + 0.5f);

					diarrheaing = true;
					diarrheaAI.enabled = true;
					poopTrail.SetActive(true);
					GameControllerScript.current.ShowChar();
				}

				StartCoroutine(WaitTime());
			}

		// DEBUG
		if (GameControllerScript.current.debug)
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				walkSpeed = walkSpeed * 2;
				runSpeed = runSpeed * 2;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				walkSpeed = walkSpeed / 2;
				runSpeed = runSpeed / 2;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				walkSpeed = DefaultVariables[0];
				runSpeed = DefaultVariables[1];
			}
		}
	}

	private void LateUpdate()
	{
		if (hasToPoop && UnityEngine.Random.Range(0, 100) == 40 && !GameControllerScript.current.gamePaused && !GameControllerScript.current.learningActive && !diarTimeStarted && playerCanPoop && !inNotebook)
		{
			if (!(PlayerPrefs.GetFloat("DisableFarts") == 2))
				audioSource.PlayOneShot(farts[UnityEngine.Random.Range(0, farts.Length)]);

			poopMultiplier -= 0.0001f;
		}

		if (poopCooldown < 30 && UnityEngine.Random.Range(0, 500) == 250 && !GameControllerScript.current.gamePaused && !GameControllerScript.current.learningActive && !diarTimeStarted && !inNotebook && !(PlayerPrefs.GetFloat("DisableFarts") == 2))
			audioSource.PlayOneShot(farts[UnityEngine.Random.Range(0, farts.Length)]);

		if (diarrheaing && !inNotebook)
			diarrheaAI.SetDestination(GameControllerScript.current.baldi.transform.position);
	}

	private void MouseMove()
	{
		if (bugfix)
		{	
			playerRotation.eulerAngles = transform.rotation.eulerAngles;
			bugfix = false;
		}

		float sens = mouseSensitivity;
		
		if (input.controllers.GetLastActiveController().type == ControllerType.Joystick)
			sens = mouseSensitivity * 3;
		else
			sens = mouseSensitivity / 7;

		playerRotation.eulerAngles = new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y, fliparoo);
		playerRotation.eulerAngles = playerRotation.eulerAngles + Vector3.up * input.GetAxis("LookHorizontal") * sens * Time.timeScale * flipaturn;

		/*if (do3DMovement)
		{
			playerRotation.eulerAngles = playerRotation.eulerAngles + Vector3.left * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale * flipaturn;
			base.transform.position = new Vector3(base.transform.position.x, 4, base.transform.position.z);
		}*/
			
		base.transform.eulerAngles = playerRotation.eulerAngles;
	}

	public bool autoWalk = false;
	bool bugfix = false;
	

	private void PlayerMove()
	{
		float speedStuff = walkSpeed;

		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		
		vector = base.transform.forward * input.GetAxis("MoveVertical");
		vector2 = base.transform.right * input.GetAxis("MoveHorizontal");
	
		if (stamina > 0f)
		{
			if (input.GetButton("Run") || staminaByDefault)
			{
				if (SettingsManager.DynamicFOV == 2 && targetFOV != defaultFOV + 15)
					targetFOV = defaultFOV + 15;

				playerSpeed = runSpeed;
				speedStuff = runSpeed;
				
				if (cc.velocity.magnitude > 1f & !hugging & !sweeping)
					ResetGuilt("running", 0.1f);
			}
			else
			{
				if (SettingsManager.DynamicFOV == 2)
					targetFOV = defaultFOV;

				playerSpeed = walkSpeed;
			}
		}
		else
		{
			playerSpeed = walkSpeed;
		}

		float monkmod = isMonke ? 2 : 1;

		playerSpeed *= Time.deltaTime * monkmod * jumpModifier * mockModifier / poopMultiplier;
		moveDirection = (vector + vector2).normalized * playerSpeed;

		if (!bootsActive)
		{
			if (sweeping)
			{
				Vector3 finalVelocity = gottaSweep.velocity;

				if (GameControllerScript.current.mode == "story_double")
				{
					if (Vector3.Distance(base.transform.position, gottaSweep.transform.position) > Vector3.Distance(base.transform.position,  FindObjectOfType<BaldiCharacterManager>().gottaSweep.gameObject.transform.position))
						finalVelocity = FindObjectOfType<BaldiCharacterManager>().gottaSweep.GetComponent<NavMeshAgent>().velocity;
					else if (Vector3.Distance(base.transform.position, firstPrize.transform.position) < Vector3.Distance(base.transform.position, FindObjectOfType<BaldiCharacterManager>().prize.gameObject.transform.position))
						finalVelocity = gottaSweep.velocity;
				}	
				else
				{
					finalVelocity = gottaSweep.velocity;
				}

				moveDirection = finalVelocity * Time.deltaTime + moveDirection * 0.3f;
			}	
			else if (hugging)
			{
				Vector3 finalVelocity;
				GameObject closestPrize;

				if (GameControllerScript.current.mode == "story_double")
				{
					finalVelocity = firstPrize.velocity + FindObjectOfType<BaldiCharacterManager>().prize.agent.velocity;
					
					if (Vector3.Distance(base.transform.position, firstPrize.transform.position) > Vector3.Distance(base.transform.position, FindObjectOfType<BaldiCharacterManager>().prize.gameObject.transform.position))
						closestPrize = FindObjectOfType<BaldiCharacterManager>().prize.gameObject;
					else if (Vector3.Distance(base.transform.position, firstPrize.transform.position) < Vector3.Distance(base.transform.position, FindObjectOfType<BaldiCharacterManager>().prize.gameObject.transform.position))
						closestPrize = firstPrize.gameObject;
					else
						closestPrize = firstPrize.gameObject;
				}	
				else
				{
					finalVelocity = firstPrize.velocity;
					closestPrize = firstPrize.gameObject;
				}

				moveDirection = 
				(
					finalVelocity * 1.2f * Time.deltaTime + (new Vector3
					(
						closestPrize.transform.position.x, 
						transform.position.y, 
						closestPrize.transform.position.z
					) + new Vector3
					(
						(float)Mathf.RoundToInt(closestPrize.transform.forward.x), 
						0f, 
						(float)Mathf.RoundToInt(closestPrize.transform.forward.z)
					) * 3f - base.transform.position)
				) * (float)principalBugFixer;
			}		
		}

		if (!jumpRope && !dt_jumpRope)
			cc.Move(moveDirection);
		
		if (Input.GetAxis("Forward") != 0 || Input.GetAxis("Strafe") != 0)
		{
			if (playerAnimator.GetBool("Walking") == false)
			{
				playerAnimator.SetTrigger("Walk");
				playerAnimator.SetBool("Walking", true);
			}

			playerAnimator.speed = cc.velocity.magnitude / 16;
		}
		else
		{
			if (playerAnimator.GetBool("Walking") == true)
			{
				playerAnimator.SetTrigger("StopWalking");
				playerAnimator.SetBool("Walking", false);
			}
				
			playerAnimator.speed = 1f;
		}
	}

	public void BecomeMonke()
	{
		if (!GameControllerScript.current.mrBeast.beasting && GameControllerScript.current.mode != "hard")
		{
			GameControllerScript.current.schoolMusic.clip = monkeLoop;
			GameControllerScript.current.schoolMusic.Play();
		}

		isMonke = true;
		playerModels[0].SetActive(false);
		playerModels[1].SetActive(true);
		GameControllerScript.current.audioDevice.PlayOneShot(monkeTransition);
	}

	public void DisableMonke(bool playMonkeSound = true)
	{
		if ((!GameControllerScript.current.mrBeast.beasting && !GameControllerScript.current.finaleMode) || GameControllerScript.current.neilMode)
			GameControllerScript.current.schoolMusic.Stop();

		isMonke = false;
		playerModels[1].SetActive(false);
		playerModels[0].SetActive(true);

		if (playMonkeSound)
			GameControllerScript.current.audioDevice.PlayOneShot(monkeTransition);

		if (cameraAnimator.GetBool("Walking") == true)
		{
			cameraAnimator.SetTrigger("StopMonkeWalk");
			monkeAnimator.SetTrigger("MonkeStopWalk");
			cameraAnimator.SetBool("Walking", false);
		}
	}

	private void StaminaCheck()
	{
		if (PlayerPrefs.GetFloat("blind") == 1)
        {
            staminaText.gameObject.SetActive(false);
        }

		if (!GameControllerScript.current.inHitmanCutscene)
		{
			if (cc.velocity.magnitude > 0.1f && cc.enabled)
			{
				if (input.GetButton("Run")  && stamina > 0f && !infiniteStamina)
				{
					stamina -= (staminaRate + (staminaRate * (outside ? -1 : 0))) * Time.deltaTime;
				}
				if (stamina < 0f & stamina > -5f)
				{
					stamina = -5f;
				}
				if (outside && !input.GetButton("Run"))
					stamina += staminaRate * Time.deltaTime;
			}
			else if (stamina < maxStamina)
			{
				stamina += (staminaRate + (staminaRate * (outside ? 1 : 0))) * Time.deltaTime;
			}
		}
		int staminaa = Mathf.RoundToInt(stamina / maxStamina * 100f);
		string display = staminaa.ToString();

		if (staminaa <= 0)
			display = "0";
			
		if (staminaa < 25)
			staminaText.color = Color.red;
		else
			staminaText.color = Color.white;

		if (staminaText != null)
			staminaText.text = display + "%";
	}

	public void OnPlayerTeleport()
	{
		// Van Man Jumprope
		if (jumpRope)
			GameControllerScript.current.vanman.PlaySound(GameControllerScript.current.vanman.whereTheFuck);
		
		// Explode GameBoy Color
		for (int i = 0; i < 5; i++)
		{
			if (GameControllerScript.current.IteminInventory(24)) 
			{
				Animator[] theAnimators = GameControllerScript.current.explosionAnimators;
				Animator theAnimator = GameControllerScript.current.hotbarAnimator;

                if (PlayerPrefs.GetFloat("back_to_the_basics") == 1) theAnimators = GameControllerScript.current.basicExplosionAnimators;
                if (PlayerPrefs.GetFloat("back_to_the_basics") == 1) theAnimator = GameControllerScript.current.basicHotbarAnimator;

                theAnimator.SetTrigger("kaboom");
				GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.robloxrocketsound);
				GameControllerScript.current.LoseItem(i);
				theAnimators[i].SetTrigger("kaboom");
			}
		}

		// Mr Beast Challenge
		if (GameControllerScript.current.mrBeast.inChallenge)
		{
			GameControllerScript.current.mrBeast.inChallenge = false;
			GameControllerScript.current.mrBeast.challenge.SetActive(false);
			GameControllerScript.current.mrBeast.coolDown = 30f;
		}
	}

	public GameObject dtJumpRope;

	private void OnTriggerEnter(Collider other)
	{
		if ((other.transform.name == "Baldi" && !FindObjectOfType<BaldiScript>().isDead) || other.transform.name == "DTBaldi")
		{
			if (!GameControllerScript.current.IteminInventory(18)) 
			{
				if (!(immunitytime > 0))
				{
					gameOver = true;
					RenderSettings.skybox = GameControllerScript.current.blackSky;
					GameControllerScript.current.hud.SetActive(false);
				}
			} 
			else 
			{
				if (immunitytime == 0)
				{
					if (FindObjectOfType<PlayerStats>().data.objectasks[4] == 0)
						FindObjectOfType<Objectasks>().CollectObjectask(4);
	
					GameControllerScript.current.baldi.timeToMove = GameControllerScript.current.baldi.RAL_ACrackpipe.length + 0.2f + GameControllerScript.current.baldi.RAL_GetHigh.length + 0.2f;
					immunitytime = 5;
					GameControllerScript.current.baldi.baldiHighCrash = 13;
					GameControllerScript.current.RemoveItem(18);

					IEnumerator PlayHighSounds()
					{
						GameControllerScript.current.baldi.GetComponent<AudioSource>().PlayOneShot(GameControllerScript.current.baldi.RAL_ACrackpipe);
						yield return new WaitForSeconds(GameControllerScript.current.baldi.RAL_ACrackpipe.length + 0.2f);
						GameControllerScript.current.baldi.GetComponent<AudioSource>().PlayOneShot(GameControllerScript.current.baldi.RAL_GetHigh);
						GameControllerScript.current.baldi.GetComponent<AudioSource>().PlayOneShot(GameControllerScript.current.baldi.risingSuspense);
					}

					StartCoroutine(PlayHighSounds());
				}
			}
		}
		
		if (other.gameObject == GameControllerScript.current.vanman.gameObject && !jumpRope && GameControllerScript.current.vanman.playCool <= 0f)
		{
			jumpRopeScreen.SetActive(true);
			jumpRope = true;
			frozenPosition = base.transform.position;
		}
		
		if (other.gameObject == GameControllerScript.current.bcm.playtime.gameObject && !dt_jumpRope && GameControllerScript.current.bcm.playtime.playCool <= 0f)
		{
			dtJumpRope.SetActive(true);
			dt_jumpRope = true;
			frozenPosition = base.transform.position;
		}

		if (other.transform.tag == "ChalkArea")
			inGas = true;
		if (other.transform.tag == "goingOutside")
			outside = true;
		if (other.transform.tag == "goingInside")
			outside = false;
	}

	public float jumpModifier = 1f;	

	public IEnumerator KeepTheHudOff()
	{
		while (gameOver)
		{
			GameControllerScript.current.hud.SetActive(false);
			jumpRopeScreen.SetActive(false);
			yield return new WaitForEndOfFrame();
		}

		yield break;
	}

	private void OnTriggerStay(Collider other)
	{
		GameObject closestPrize;

		if (GameControllerScript.current.mode == "story_double")
		{
			if (Vector3.Distance(base.transform.position, firstPrize.transform.position) > Vector3.Distance(base.transform.position, FindObjectOfType<BaldiCharacterManager>().prize.gameObject.transform.position))
				closestPrize = FindObjectOfType<BaldiCharacterManager>().prize.gameObject;
			else if (Vector3.Distance(base.transform.position, firstPrize.transform.position) < Vector3.Distance(base.transform.position, FindObjectOfType<BaldiCharacterManager>().prize.gameObject.transform.position))
				closestPrize = firstPrize.gameObject;
			else
				closestPrize = firstPrize.gameObject;
		}	
		else
			closestPrize = firstPrize.gameObject;

		if (other.transform.name == "Gotta Sweep")
		{
			sweeping = true;
			sweepingFailsave = 1f;
		}
		else if (other.transform.name == "1st Prize" && closestPrize.GetComponent<NavMeshAgent>().velocity.magnitude > 5f)
		{
			hugging = true;
			sweepingFailsave = 0.1f;
		}

		if (other.name == "Gum" && other.GetComponentInChildren<SpriteRenderer>().sprite != BeansScript.spriteNPCGum)
		{
			StartCoroutine(Stucked());
			UnityEngine.Object.Destroy(other.gameObject, 0f);
			UnityEngine.Object.FindObjectOfType<BeansScript>().SorryPlayer();
		}

		if (other.name == "PortalOpenTrigger")
			GameControllerScript.current.OpenPortal();
	}

	private IEnumerator Stucked()
	{
		float ogw; 
		float ogr;

		ogw = walkSpeed;
		ogr = runSpeed;

		gumScreen.SetActive(true);
		walkSpeed = 1f;
		runSpeed = 1f;
		playerSpeed = walkSpeed;
		yield return new WaitForSeconds(10f);
		walkSpeed = DefaultVariables[0];
		runSpeed = DefaultVariables[1];
		playerSpeed = DefaultVariables[0];
		gumScreen.SetActive(false);
		yield break;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Office Trigger")
			ResetGuilt("escape", GameControllerScript.current.jailDoor.lockTime - 1);
		else if (other.transform.name == "Gotta Sweep")
			sweeping = false;
		else if (other.transform.name == "1st Prize")
			hugging = false;

		if (other.transform.tag == "ChalkArea")
			inGas = false;
	}

	public void ResetGuilt(string type, float amount)
	{
		if (amount >= guilt)
		{
			guilt = amount;
			guiltType = type;
		}
	}

	private void GuiltCheck()
	{
		if (guilt > 0f)
			guilt -= Time.deltaTime;
	}

	public void ActivateJumpRope()
	{
		jumpRopeScreen.SetActive(true);
		jumpRope = true;
		frozenPosition = base.transform.position;
	}

	public void DeactivateJumpRope()
	{
		dtJumpRope.SetActive(false);
		jumpRopeScreen.SetActive(false);
		jumpRope = false;
		dt_jumpRope = false;
	}

	public void ActivateBoots()
	{
		bootsActive = true;
		base.StartCoroutine(BootTimer());
	}

	private IEnumerator BootTimer()
	{
		float time = 15f;

		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}

		bootsActive = false;
		yield break;
	}
}
