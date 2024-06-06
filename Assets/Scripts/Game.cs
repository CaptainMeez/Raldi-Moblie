/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering.PostProcessing;

using TMPro;

public class Game : MonoBehaviour
{
    public static Game current;

	[Header("Debug")]
	public bool debug = true;

	public float monketime = 10;

	[Header("Script References")]
	public CursorControllerScript cursorController;
	public MoneyManager money;
	public PostProcessVolume ppVolume;
	public LoadingScreen loading;
	public LoadingScreen realLoading;
	public PlayerScript player;

	// Post Processing
	[HideInInspector] public ChromaticAberration chromaticEffect;
	[HideInInspector] public Bloom bloomEffect;
	[HideInInspector] public AutoExposure exposure;

	[Header("Cameras")]
	public Camera pcamera;
	public Camera mapCamera;
	public Camera minimapCamera;
	
	[Header("Objects")]
    public Transform playerTransform;
	public Transform cameraTransform;

    public GameObject hud;
    public GameObject blackScreen;
	public GameObject minimapstatic;
	public GameObject debugModeText;
	public GameObject map;
	public GameObject warning;
	public GameObject[] ballposters;

	[Header("Prefabs")]
	public GameObject bsodaSpray;
	public GameObject enderPearl;
	public GameObject alarmClock;
	public GameObject polishMarkerPrefab;

	[Header("Animation")]
	public Animator hudAnimator;
	public Animator hotbarAnimator;
	public Animator[] explosionAnimators = new Animator[5];
	public Animator wiigolfslideranim;

	[Header("Sounds")]
	public AudioClip pickup;
	public AudioClip cash;
	public AudioClip aud_PearlThrow;
	public AudioClip aud_PearlLand;
	public AudioClip aud_Soda;
	public AudioClip aud_Spray;
	public AudioClip aud_GasCanister;
	public AudioClip aud_Nom;
	public AudioClip aud_ammoGrab;
	public AudioClip aud_buzz;
	public AudioClip hammerHit;
	public AudioClip dialing;
	public AudioClip ringing;
	public AudioClip cuzsieRingtone;
	public AudioClip pickUpPhone;
	public AudioClip talkingOnPhone;
	public AudioClip hangUp;
	public AudioClip cuzsieWalking;
	public AudioClip windowBreak;
	public AudioClip schoolDoorOpen;

	[Header("Material")]
	public Material confusingPoster;
	public Material blackSky;
	public Material defaultSky;
	public Material dentedSign;
	public Material brokenVault;
	public Material brokenWindow;

	[Header("Images")]

	public Sprite[] itemSprites = new Sprite[10]; // yes i hate this too, yes im lazy.
	public int[] rewarditems = new int[10];

	[Header("AudioSource")]
	public AudioSource audioDevice;
	public AudioSource schoolMusic;

	[Header("Colors")]	
	[HideInInspector] public Color targetSchoolColor;

	[Header("UI")]
	public TextMeshProUGUI ammotext;
	public TMP_Text itemText;
	public TMP_Text notebookCount;
	public RectTransform boots;
	public GameObject reticle;
	public Slider wiigolfslider;

	public Image[] itemSlot = new Image[5];
	public RawImage[] itemBGS = new RawImage[3];

	[Header("Sprites")]
	public SpriteRenderer itemSprite;

	[Header("Machine Discounts")]
	public int sodaDiscount = 1;
	public int zestyDiscount = 1;
	public int energyDiscount = 1;
	public int pearlDiscount = 1;

	[Header("Varriables")]
	private bool gameBoyOpen = false;

	public float playTime;
	
	public int itemSelected;
	public int notebooks;
	public int notebooksToCollect;
	
    public bool allowInput = true;
	public bool inBathroom;
	public bool inventoryfull;
	public float timepearlhelddown = 0;
	public bool throwingpearl;
    public bool doDropAnimation = true;

	public string[] itemNames = new string[]
	{
		"",
		"Energy flavored Zesty Bar",
		"Yellow Door Lock",
		"Principal's Keys",
		"BSODA",
		"Quarter",
		"Mystery Tape",
		"Alarm Clock",
		"WD-NoSquee (Door Type)",
		"Glock",
		"Big Ol' Boots",
		"Poop Meter",
		"Medkit",
		"Mr Beast's Credit Card",
		"iPhone",
		"Acid Gas",
		"Hammer for Sticky Situations",
		"15 Second Energy",
		"A Crackpipe for Raldi",
		"Get Out of Jail Free Card",
		"Monkenator",
		"Ishaan's Potato",
		"Ender Pearl"
	};

	public string[] itemDescriptions = new string[]
	{
		"",
		"Yum!\nRefills your stamina to 200%.",
		"Lock any yellow door.",
		"Use these to escape Chipfloke's jail cell!... but not by legal means...",
		"Spawns a puddle of soda that pushes back any character in it!",
		"",
		"Ever wanted to piss off Raldi with a repetitive 24/7 radio?",
		"Once this goes off, Raldi will be alerted to it's current location.",
		"Doors will no longer make sounds!",
		"Remember kids!\nIf someone is pissing you off, particularly perverted van drivers, dont fret! Shoot them with a fucking gun.",
		"Does anyone even use these?\nDon't get pushed by Gotta Weep or First Prize.",
		"Ever wondered when you have to poop? The poop meter is here!",
		"Save yourself from bullet wounds quick and easy!",
		"Property of MrBeast.",
		"",
		"Nobody will see you in this extra foggy gas!",
		"Ever in a sticky situation? Hammer.",
		"Get energized for 15 seconds!\n(Not FDA approved.)",
		"Raldi l i k e s his crackpipes.",
		"Get Out of Jail, Free\nThis card may be kept until needed or sold.",
		"NO, GO BACK, I WANT TO BE MONKE!",
		"It's a potato. Nothing special.",
		"This one's been sitting out for a while."
	};

	public int[] item = new int[5];

	public void ExitGame() {realLoading.gameObject.SetActive(true); StartCoroutine(realLoading.LoadingLoadScene("MainMenu"));}
	public void Reset() {loading.gameObject.SetActive(true); StartCoroutine(loading.LoadingLoadSceneInt(SceneManager.GetActiveScene().buildIndex));}
	public void Destroy(GameObject target) => GameObject.Destroy(target);

	public Sprite[] tapes;
	public int curTape;
	public AudioClip bus;

	private void Start()
	{
		FindObjectOfType<PlayerStats>().TryLoad();

		current = this;

		audioDevice = base.GetComponent<AudioSource>();
		notebooksToCollect = FindObjectsOfType<NotebookScript>().Length;
		targetSchoolColor = Color.white;

		// Get post processing profiles
		ppVolume.profile.TryGetSettings(out chromaticEffect);
		ppVolume.profile.TryGetSettings(out bloomEffect);
		ppVolume.profile.TryGetSettings(out exposure);

		itemSelected = 0;

		LockMouse();
		UpdateNotebookCount();

		audioDevice.PlayOneShot(bus);
	}

	public bool holdingpearl;
	private bool endgamelock = false;

	private bool endgametimelock = false;
	public AudioClip aud_timeout;
	public AudioClip aud_morbbegin;
	public AudioClip aud_megamanexplode;
	private string[] ranks = new string[6]{"d","c","b","a","s","p"};

	private bool tickUpChromatic = false;
	private bool tickUpDistortion = false;
	private bool tickUpVig = false;

	private void Update()
	{
		if (debug) debugModeText.SetActive(true);
		else debugModeText.SetActive(false);

		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
			debug = !debug;
		#endif

        PassTime();
		UpdateHUD();
		InputCheck();
		InventoryCheck();
	}

	private void InventoryCheck()
	{
		inventoryfull = true;
		for (int i = 0; i < 5; i++)
		{
			if (item[i] == 0)
				inventoryfull = false;
		}
	}
	
	private void PassTime()
	{
		if (!gamePaused)
		{
			playTime += Time.deltaTime;
		}
	}

	private void UpdateHUD()
	{
		if (player.stamina < 0f & !warning.activeSelf)
			warning.SetActive(true);
		else if (player.stamina > 0f & warning.activeSelf)
			warning.SetActive(false);

        if (item[itemSelected] == 22)
		{
			if (!holdingpearl)
			{
				holdingpearl = true;
				wiigolfslideranim.SetTrigger("hold pearl");
				wiigolfslideranim.SetBool("holding pearl", true);
			}
			if (Input.GetMouseButton(0) && timepearlhelddown < 3)
			{
				timepearlhelddown += Time.deltaTime * 1.5f;
				throwingpearl = true;
			}
			else if (throwingpearl && timepearlhelddown > 0.5)
			{
				throwingpearl = false;
				GameObject pearl = UnityEngine.Object.Instantiate<GameObject>(this.enderPearl, this.playerTransform.position + new Vector3(0,1,0), this.cameraTransform.rotation);
				print("spawn");
				pearl.GetComponent<PearlScript>().speed = timepearlhelddown * 50;
				timepearlhelddown = 0;
				ResetItem();
				audioDevice.PlayOneShot(aud_PearlThrow);
			} 
            else 
            {
				throwingpearl = false;
				timepearlhelddown = 0;
			}
		} 
        else 
        {
			if (holdingpearl)
			{
				timepearlhelddown = 0;
				wiigolfslideranim.SetTrigger("no hold pearl");
				wiigolfslideranim.SetBool("holding pearl", false);
				holdingpearl = false;
			}
		}

		wiigolfslider.value = timepearlhelddown / 3;
		chromaticEffect.intensity.value = Mathf.Lerp(chromaticEffect.intensity, 0.7f, 2 / Time.deltaTime);
		bloomEffect.intensity.value = Mathf.Lerp(bloomEffect.intensity, 5f, 2 * Time.deltaTime);
	}

	bool mapOpen = false;

	private void InputCheck()
	{
		if (allowInput)
		{
			if (!gamePaused)
			{
				if (Input.GetKeyDown(KeyCode.M))
				{
					if (!mapOpen)
						OpenMap();
					else
						CloseMap();
				}

				if (mapOpen && Input.GetKeyDown(KeyCode.Escape))
				{
					CloseMap();
				}

				if (Input.GetKey(KeyCode.Q))
					player.DropItem(itemSelected);
			}

			if (!gamePaused & Time.timeScale != 1f)
				Time.timeScale = 1f;

			if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
				UseItem();

			if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && Time.timeScale != 0f)
				MachineCheck();
                
			if ((Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f))
				DecreaseItemSelection();
			else if ((Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f))
				IncreaseItemSelection();

			if (Time.timeScale != 0f && allowItemSwitch)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					itemSelected = 0;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.itemSelected = 1;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.itemSelected = 2;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.itemSelected = 3;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.itemSelected = 4;
					UpdateItemSelection();
				}
			}
        }
	}
	
	public void UpdateNotebookCount()
	{
		if (!finaleMode)
		{
			if (this.mode.Contains("story") || mode == "hard")
				this.notebookCount.text = this.notebooks.ToString() + "/" + notebooksToCollect.ToString();
			else
				this.notebookCount.text = this.notebooks.ToString();
		}
		else
		{
			nbLabel.text = "Exits";
			notebookCount.text = exitsReached.ToString() + "/4";
		}
	}

	public void OnRythmComplete()
	{
		eventActive = false;
		player.cc.enabled = true;
		player.inNotebook = false;
		player.enabled = true;
		hud.SetActive(true);
		GameObject.Destroy(robject);
		ShowChar();
	}

	public void OnDeltaComplete(bool genocide)
	{
		FindObjectOfType<Objectasks>().CollectObjectask(2);
		killedmrBeast = genocide;
		beastCardCollected = false;
		eventActive = false;
		player.cc.enabled = true;
		canPause = true;
		player.inNotebook = false;
		FindObjectOfType<BeastCardArea>().ui_Card.SetActive(false);
		beastTip.SetActive(false);
		atm.infoTick.SetActive(false);
		player.enabled = true;
		hud.SetActive(true);
		GameObject.Destroy(robject);
		cameraTransform.gameObject.SetActive(true);
		RenderSettings.skybox = defaultSky;
		mrBeast.music.Play();
		mrBeast.agent.speed = 20;
		mrBeast.beasting = false;
		mrBeast.gettingMad = false;
		mrBeastBillboard.enabled = true;
		ShowChar();
		mrBeast.gameObject.SetActive(!genocide);
	}

	public void RareEvents(int random)
	{
		if (random == 0)
		{
			ActivateSpoopMode();
			HideChar();
			rareEventActivated = true;
			eventActive = true;
			player.cc.enabled = false;

			HideChar();
				
			fakeBaldi.SetActive(true);
			allIKnow.SetActive(true);
			allIKnowVideo.Play();

			RenderSettings.ambientLight = allIColor;

			IEnumerator WaitTime()
			{
				float dumbval = 1.89f;
					
				yield return new WaitForSeconds(dumbval);

				player.transform.position = new Vector3(5f, this.player.transform.position.y, 15f);
				baldi.agent.Warp(new Vector3(5f, this.baldi.gameObject.transform.position.y, 35f));
				player.transform.LookAt(new Vector3(fakeBaldi.transform.position.x, player.transform.position.y, fakeBaldi.transform.position.z));
				player.enabled = false;

				yield return new WaitForSeconds(Convert.ToSingle(allIKnowVideo.clip.length) - dumbval);

				ShowChar();
				fakeBaldi.SetActive(false);
				allIKnow.SetActive(false);
				player.cc.enabled = true;
				player.enabled = true;
				eventActive = false;
				RenderSettings.ambientLight = Color.white;
			}

			StartCoroutine(WaitTime());
		}
		else if (random == 1)
		{
			ActivateSpoopMode();
			HideChar();

			rareEventActivated = true;
			eventActive = true;

			FNFMinigame();
		}
	}

	public void FNFMinigame()
	{
		IEnumerator WaitTime()
		{
			player.cc.enabled = false;
			player.enabled = false;

			audioDevice.PlayOneShot(baldiCountdown[0]);
			yield return new WaitForSeconds(0.4f);
			audioDevice.PlayOneShot(baldiCountdown[1]);
			yield return new WaitForSeconds(0.4f);
			audioDevice.PlayOneShot(baldiCountdown[2]);
			yield return new WaitForSeconds(0.4f);
			audioDevice.PlayOneShot(baldiCountdown[3]);
			yield return new WaitForSeconds(0.4f);

			hud.SetActive(false);
			robject = GameObject.Instantiate<GameObject>(rythmMinigame, new Vector3(0, 1000, 0), base.transform.rotation);

			yield return null;
			SongManager.Instance.onComplete.AddListener(OnRythmComplete);
		}

		StartCoroutine(WaitTime());
	}

	public void MrBeastDeltarune()
	{
		this.schoolMusic.Stop();
		IEnumerator WaitTime()
		{
			HideChar();
			canPause = false;
			rareEventActivated = true;
			eventActive = true;

			player.cc.enabled = false;
			player.enabled = false;
			cameraTransform.gameObject.SetActive(false);
			RenderSettings.skybox = blackSky;

			hud.SetActive(false);

			robject = GameObject.Instantiate<GameObject>(mrBeastDeltarune, new Vector3(0, 1000, 0), base.transform.rotation);

			yield return null;
		}

		StartCoroutine(WaitTime());
	}

	public void StartUndertale()
	{
		this.schoolMusic.Stop();
		IEnumerator WaitTime()
		{
			HideChar();
			canPause = false;
			rareEventActivated = true;
			eventActive = true;

			player.cc.enabled = false;
			player.enabled = false;
			cameraTransform.gameObject.SetActive(false);
			RenderSettings.skybox = blackSky;

			hud.SetActive(false);

			robject = GameObject.Instantiate<GameObject>(undertalefight, new Vector3(0, 1000, 0), base.transform.rotation);

			yield return null;
		}

		StartCoroutine(WaitTime());
	}

	public void StopEvent(string cevent)
	{
		switch(cevent)
		{
			case "dog":
				dogCall.SetActive(false);
				dogActive = false;
				break;
			case "uncanny":
				uncannyLmao.SetActive(false);
				uncannyCamera.gameObject.SetActive(false);
				reticle.SetActive(true);
				audioDevice.Stop();
				break;
			case "peter":
				foreach(PeterScript peter in FindObjectsOfType<PeterScript>())
				{
					GameObject.Destroy(peter.gameObject);
				}
				break;
			case "location":
				baldiPosFunny.SetActive(false);
				audioDevice.Stop();
				break;
			case "splatter":
				splatter.gameObject.SetActive(false);
				break;
			case "birds":
				birdEvent.gameObject.SetActive(false);
				break;
			case "noisecall":
				noiseCall.SetActive(false);
				break;
		}

		eventActive = false;
	}

	public void StopAllEvents()
	{
		dogCall.SetActive(false);
		dogActive = false;
		uncannyLmao.SetActive(false);
		reticle.SetActive(true);
		baldiPosFunny.SetActive(false);
		splatter.gameObject.SetActive(false);
		birdEvent.gameObject.SetActive(false);
		uncannyCamera.gameObject.SetActive(false);
		noiseCall.SetActive(false);
		audioDevice.Stop();

		foreach(PeterScript peter in FindObjectsOfType<PeterScript>())
		{
			GameObject.Destroy(peter.gameObject);
		}
	}

	public void RandomizedEvents()
	{
		int random = UnityEngine.Random.Range(0, 175);
		int rareRandom = UnityEngine.Random.Range(0, 225);

		if (!player.diarTimeStarted && !eventActive && time > 2f && eventDelay <= 0f && allowEvents)
		{
			// Your dog is calling event
			if (rareRandom == 5 && lastEvent != "dog")
			{
				bool did = false;
				foreach(int itemm in item)
				{
					if (itemm == 14 && !did)
					{
						did = true;
						lastEvent = "dog";
						eventDelay = 10f;
						eventActive = true;

						dogCall.SetActive(true);
						dogCall.GetComponent<AudioSource>().Play();
						dogActive = true;

						IEnumerator WaitTime()
						{
							yield return new WaitForSeconds(5);
							StopEvent(lastEvent);
						}

						StartCoroutine(WaitTime());
					}
				}
			}
			// Uncanny event
			else if (rareRandom == 6 && lastEvent != "uncanny")
			{
				lastEvent = "uncanny";
				eventDelay = 10f;
				eventActive = true;

				reticle.SetActive(false);
				uncannyLmao.SetActive(true);
				audioDevice.PlayOneShot(mus_Uncanny);

				uncannyCamera.gameObject.SetActive(true);

				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(mus_Uncanny.length);
					StopEvent(lastEvent);
				}

				StartCoroutine(WaitTime());
			}
			// Peter event
			else if (random == 4 && lastEvent != "peter")
			{
				lastEvent = "peter";
				eventDelay = 7f;

				IEnumerator Spawn()
				{
					for (int i = 0; i < 7; i++) 
					{
						GameObject.Instantiate<GameObject>(peterPrefab, new Vector3(playerTransform.position.x + UnityEngine.Random.Range(-20, 20), peterPrefab.transform.position.y, playerTransform.position.z + UnityEngine.Random.Range(-20, 20)), peterPrefab.transform.rotation);
						yield return new WaitForSeconds(0.2f);
					}
				}
				
				StartCoroutine(Spawn());
			}
			// Baldi location event
			else if (random == 3 && lastEvent != "location")
			{
				lastEvent = "location";
				eventDelay = 7f;
				eventActive = true;

				baldiPosFunny.SetActive(true);
				audioDevice.PlayOneShot(baldiHome);

				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(baldiHome.length);
					StopEvent(lastEvent);
				}

				StartCoroutine(WaitTime());
			}
			// Splatter event
			else if (random == 2 && lastEvent != "splatter")
			{
				lastEvent = "splatter";
				eventDelay = 7f;

				audioDevice.PlayOneShot(splatterSound);
				splatter.gameObject.SetActive(true);
				splatter.Play("SplatterSplat");
				
				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(4.5f);
					StopEvent(lastEvent);
				}

				StartCoroutine(WaitTime());
			}
			else if (random == 7 && lastEvent != "birds")
			{
				lastEvent = "birds";
				eventDelay = 7f;

				audioDevice.PlayOneShot(bird);
				birdEvent.gameObject.SetActive(true);

				eventActive = true;
				
				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(bird.length);
					StopEvent(lastEvent);
				}

				StartCoroutine(WaitTime());
			}
			else if (random == 8 && lastEvent != "noisecall")
			{
				lastEvent = "noisecall";
				eventDelay = 15f;
				eventActive = true;

				noiseCall.SetActive(true);
				audioDevice.PlayOneShot(noiseCalls);

				IEnumerator WaitTime()
				{
					yield return new WaitForSeconds(noiseCalls.length);
					StopEvent(lastEvent);
				}

				StartCoroutine(WaitTime());
			}
		}
	}

	public AudioClip noiseCalls;
	public AudioClip bird;
	public GameObject birdEvent;

	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
		foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
			{
				if (sources.gameObject.name != "Principal of the Thing" && sources.gameObject.name != "SchoolMusic")
				{
					sources.mute = false;
				}
            }
	}

	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor();
			this.reticle.SetActive(true);
		}
	}

	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor();
		this.reticle.SetActive(false);
	}

	public GameObject inventory;
	bool inventoryOpened = false;

	public void OpenInventory()
    {
        if (!this.learningActive)
        {
            this.UnlockMouse();
			inventoryOpened = true;
            this.inventory.SetActive(true);

			player.inNotebook = true;
        }
    }

	public void CloseInventory()
    {
        LockMouse();
		inventoryOpened = false;
        inventory.SetActive(false);
		player.inNotebook = false;
    }


    public void PauseGame()
    {
        if (!this.learningActive && canPause)
        {
            this.UnlockMouse();
            Time.timeScale = 0f;
            this.gamePaused = true;
            this.pauseMenu.SetActive(true);

			audioDevice.Pause();

			foreach(AudioSource source in FindObjectsOfType<AudioSource>())
			{
				if (source.gameObject.name != "Breakfast")
				source.Pause();
				else
					source.Play();
			}
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        this.gamePaused = false;
        this.pauseMenu.SetActive(false);
        this.LockMouse();

		audioDevice.UnPause();

		foreach(AudioSource source in FindObjectsOfType<AudioSource>())
		{
			if (source.gameObject.name != "Breakfast")
				source.UnPause();
			else
				source.Stop();
		}
    }

	public void OpenMap()
    {
        if (!this.learningActive && canPause)
        {
            this.UnlockMouse();
            Time.timeScale = 0f;
            mapOpen = true;
			player.inNotebook = true;
            this.map.SetActive(true);
			mapCamera.gameObject.SetActive(true);
			minimapCamera.gameObject.SetActive(false);

			audioDevice.Pause();
        }
    }

	public void CloseMap()
    {
        Time.timeScale = 1f;
        mapOpen = false;
		player.inNotebook = false;
        this.map.SetActive(false);
        this.LockMouse();
		mapCamera.gameObject.SetActive(false);
		minimapCamera.gameObject.SetActive(true);

		audioDevice.UnPause();
    }

    public void ActivateSpoopMode()
	{
		this.spoopMode = true;
		this.entrance_0.Lower();
		this.entrance_1.Lower();
		this.entrance_2.Lower();
		this.entrance_3.Lower();
		ShowChar();
		this.learnMusic.Stop();
		this.schoolMusic.Stop();
	}
	public void ShowPortals()
	{
		this.entrance_0.RaisePortal();
		this.entrance_1.RaisePortal();
		this.entrance_2.RaisePortal();
		this.entrance_3.RaisePortal();
	}
	public void HidePortals()
	{
		this.entrance_0.LowerPortal();
		this.entrance_1.LowerPortal();
		this.entrance_2.LowerPortal();
		this.entrance_3.LowerPortal();
	}

	public void ActivateMorbinTime()
	{
		IEnumerator Activate()
		{
			endgameTimeLeft = endgameTimeStart;
			endSong.loop = false;
			endSong.clip = morbinintro;
			//audioDevice.PlayOneShot(aud_morbbegin);
			endSong.Play();
			musicBeat.BPM = 169.444f;
			musicBeat.syncBPM();
			player.walkSpeed += 7f;
			player.runSpeed += 14f;
			player.DefaultVariables[0] += 7f;
			player.DefaultVariables[1] += 14f;

			morbintime.SetActive(true);
			timeLeftUI.SetActive(true);
			endgame = true;
			yield return new WaitForSeconds(3f);
			morbintime.SetActive(false);
			yield return new WaitForSeconds(morbinintro.length - 3.1f);
			monkeMorbin.loop = true;
			monkeMorbin.Play();
			endSong.loop = true;
			endSong.clip = morbinloop;
			endSong.Play();
		}	

		StartCoroutine(Activate());
	}
	
	public void ActivateLap2()
	{
		IEnumerator Lap2Setup()
		{
			chipfloke.angry = false;
			player.washedHands = true;
			endgametimelock = true;
			monkeMorbin.Stop();	
			endSong.Stop();
			player.cc.enabled = false;
			lap2cutscene.SetActive(true);
			portalaudiosource.PlayOneShot(portalenter);
			HideChar();
			yield return new WaitForSeconds(portalenter.length + 1.45f);
			notebooksToCollect = 29;
			UpdateNotebookCount();
			endgametimelock = false;
			exitsReached = 0;
			portalaudiosource.PlayOneShot(portalexit);
			ShowChar();
			player.gameObject.transform.position = new Vector3(5f, this.player.gameObject.transform.position.y, 10f);
			player.cc.enabled = true;
			HidePortals();
			foreach(NotebookScript books in FindObjectsOfType<NotebookScript>())
			{
				books.GoUp();
			}
			endSong.clip = morbinlap2;
			endSong.Play();
			monkeMorbin.clip = monkeLap2;
			monkeMorbin.Play();
			musicBeat.BPM = 180f;
			musicBeat.syncBPM();
			player.walkSpeed += 7f;
			player.runSpeed += 14f;
			player.DefaultVariables[0] += 7f;
			player.DefaultVariables[1] += 14f;
			yield return new WaitForSeconds(5f);
			lap2cutscene.SetActive(false);
		}
		StartCoroutine(Lap2Setup());
	}

	public void HideChar(string exep = "")
	{
		List<string> exeption = new List<string>(exep.Split(','));

		if (baldiTutor != null && !exeption.Contains("tutorraldi")) this.baldiTutor.SetActive(false);
		if (!exeption.Contains("raldi")) this.baldi.gameObject.SetActive(false);
		if (!exeption.Contains("chipfloke")) this.chipfloke.gameObject.SetActive(false);
		if (!exeption.Contains("crafters")) this.crafters.gameObject.SetActive(false);
		if (!exeption.Contains("vanman")) this.vanman.gameObject.SetActive(false);
		if (!exeption.Contains("gottaweep")) this.gottaWeep.gameObject.SetActive(false);
		if (!exeption.Contains("bloke")) this.bully.gameObject.SetActive(false);
		if (!exeption.Contains("firstprize")) this.firstPrize.gameObject.SetActive(false);
		if (!exeption.Contains("mrbeast")) mrBeast.gameObject.SetActive(false);
		if (!exeption.Contains("beans")) beans.gameObject.SetActive(false);
		if (!exeption.Contains("present")) present.gameObject.SetActive(false);
		if (!exeption.Contains("cow")) polishCow.gameObject.SetActive(false);
	}

	public void ShowChar()
	{
		this.crafters.gameObject.SetActive(true);
		if (!crafters.angry)
		{
			if (baldiTutor != null) this.baldiTutor.SetActive(false);
			this.baldi.gameObject.SetActive(true);
			this.chipfloke.gameObject.SetActive(true);
			if (!vanmaninprison)
				this.vanman.gameObject.SetActive(true);
			this.gottaWeep.gameObject.SetActive(true);
			this.bully.gameObject.SetActive(true);
			this.firstPrize.gameObject.SetActive(true);
			if (!killedmrBeast)
				mrBeast.gameObject.SetActive(true);
			beans.gameObject.SetActive(true);
			present.gameObject.SetActive(true);
			polishCow.gameObject.SetActive(true);
		}
		foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
			{
				if (sources.gameObject.name != "Principal of the Thing" && sources.gameObject.name != "SchoolMusic")
				{
					sources.mute = false;
				}
            }
	}

	public AudioClip crackhouseEscapeLoop;
	public Color endingFogColor;

	public void ActivateFinaleMode()
	{
		this.finaleMode = true;

		foreach(GameObject obj in ballposters)
		{
			obj.GetComponent<BoxCollider>().isTrigger = false;
		}

		if (!neilMode)
		{
			allowEvents = false;

			if (!mrBeast.beasting)
			{
				endSong.Play();
				audioDevice.PlayOneShot(allNotebooks);
			}

			IEnumerator WaitToPart2()
			{
				if (!mrBeast.beasting)
				{
					foreach(AudioSource ambiences in ambientMusics)
					{
						ambiences.volume = 0;
					}

					yield return new WaitForSeconds(endSong.clip.length - 1.5f);

					targetSchoolColor = dark;

					yield return new WaitForSeconds(1.5f);

					endSong.clip = crackhouseEscapeLoop;
					endSong.loop = true;
					endSong.Play();

					//player.walkSpeed = 25;
					//player.runSpeed = 30f;

					baldi.raldiSpeeed = true;
					baldi.speed = 30f;
					baldi.poses[0].SetActive(false);
					baldi.poses[3].SetActive(true);

					musicBeat.beatHit.AddListener(BeatHit);
					musicBeat.BPM = 170;
					musicBeat.syncBPM();

					UpdateNotebookCount();

					if (IteminInventory(14) || IteminInventory(18))
					{
						hotbarAnimator.SetTrigger("kaboom");
						audioDevice.PlayOneShot(robloxrocketsound);
						for (int i = 0; i < 5; i++)
							{
								if (item[i] == 14 || item[i] == 18 || item[i] == 19) {
									LoseItem(i);
									explosionAnimators[i].SetTrigger("kaboom");
								}
							}
					}
					StartCoroutine(ActivateExits());
				}
				else
				{
					StartCoroutine(ActivateExits());
				}
			}

			StartCoroutine(WaitToPart2());
		}
		
		if (neilMode)
		{
			StartCoroutine(ActivateExits(true));
		}
	}

	IEnumerator ActivateExits(bool delay = false)
	{
		if (delay)
			yield return new WaitForSeconds(0.2f);

		entrance_0.Raise();
		entrance_1.Raise();
		entrance_2.Raise();
		entrance_3.Raise();
	}
	
	public AudioClip machineRev;
	public AudioClip machineLoop;

	public void BeatHit()
	{
		if (exitsReached < 2)
			RenderSettings.ambientLight = Color.white;

		hudAnimator.SetBool("Direction", !hudAnimator.GetBool("Direction"));
		hudAnimator.SetTrigger("Hit");
	}

	public void GetAngry(float value)
	{
		if (!this.spoopMode)
		{
			this.ActivateSpoopMode();
		}
		this.baldi.GetAngry(value);
	}

	public GameObject questionGame;

	public void ActivateLearningGame()
	{
		StopAllEvents();
		audioDevice.Pause();
		this.learningActive = true;
		this.UnlockMouse();

		this.tutorBaldi.Stop();

		if (!this.spoopMode)
		{
			this.schoolMusic.Stop();
			this.learnMusic.Play();
		}
	}

	public void DeactivateLearningGame(GameObject subject)
	{
		UnityEngine.Object.Destroy(subject);
		audioDevice.UnPause();
		this.learningActive = false;
		LockMouse();
		
		if (!this.spoopMode)
		{
			this.schoolMusic.Play();
			this.learnMusic.Stop();
		}
		if (this.notebooks == 1 & !this.spoopMode)
		{
			this.quarter.SetActive(true);
		}
	}

	private void IncreaseItemSelection()
	{
		FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");
		this.itemSelected++;

		if (this.itemSelected > 4)
			this.itemSelected = 0;

		for (int i = 0; i < itemBGS.Length; i++) 
		{
			if (i == itemSelected)
				itemBGS[i].color = normalSelect;
			else
				itemBGS[i].color = selectSelect;
		}

		this.UpdateItemName();
	}

	private void DecreaseItemSelection()
	{
		FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");
		this.itemSelected--;
		if (this.itemSelected < 0)
			this.itemSelected = 4;

		for (int i = 0; i < itemBGS.Length; i++) 
		{
			if (i == itemSelected)
				itemBGS[i].color = normalSelect;
			else
				itemBGS[i].color = selectSelect;
		}
		
		this.UpdateItemName();
	}

	public Color normalSelect;
	public Color selectSelect;	

	private void UpdateItemSelection()
	{
		for (int i = 0; i < itemBGS.Length; i++) 
		{
			FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");

			if (i == itemSelected)
				itemBGS[i].color = normalSelect;
			else
				itemBGS[i].color = selectSelect;
		}

		this.UpdateItemName();
	}

	bool allowItemSwitch = true;

	public void CollectHammer() // For neil cutscene
	{
		itemSelected = 0;

		CollectItem(16);
		UpdateItemName();

		allowItemSwitch = false;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0002245C File Offset: 0x0002085C
	public void CollectItem(int item_ID, PickupScript pickup = null)
	{
		FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");
		int slot = 0;

		if (item_ID != 5 && item_ID != 13 && item_ID != 15)
		{	
			if (this.item[0] == 0)
			{
				this.item[0] = item_ID;
				this.itemSlot[0].sprite = itemSprites[item_ID];

				slot = 0;
			}
			else if (this.item[1] == 0)
			{
				this.item[1] = item_ID;
				this.itemSlot[1].sprite = itemSprites[item_ID];

				slot = 1;
			}
			else if (this.item[2] == 0)
			{
				this.item[2] = item_ID;
				this.itemSlot[2].sprite = itemSprites[item_ID];

				slot = 2;
			}
			else if (this.item[3] == 0)
			{
				this.item[3] = item_ID;
				this.itemSlot[3].sprite = itemSprites[item_ID];

				slot = 3;
			}
			else if (this.item[4] == 0)
			{
				this.item[4] = item_ID;
				this.itemSlot[4].sprite = itemSprites[item_ID];

				slot = 4;
			}
			else
			{
				if (pickup != null)
				{
					pickup.id = this.item[this.itemSelected];
					pickup.sprite.sprite = this.itemSprites[item_ID];
				}
					

				this.item[this.itemSelected] = item_ID;
				this.itemSlot[this.itemSelected].sprite = itemSprites[item_ID];

				slot = itemSelected;
			}
		}
		else if (item_ID == 5)
		{
			AddMoney(0.25f);
		}
		else if (item_ID == 15)
		{
			if (GasAmmo == 0)
				ammoGmodReference.GetComponent<Animator>().SetTrigger("firstselected");

			GasAmmo += 1;
			audioDevice.PlayOneShot(aud_ammoGrab);
		}
		else if (item_ID == 13)
		{
			GrabCard();
		}
			

		this.UpdateItemName();
	}

	public void AddMoney(float money)
	{
		this.money.money += money;

		if (!neilMode)
			audioDevice.PlayOneShot(cash);
		else
			audioDevice.PlayOneShot(neilCash);
	}

	bool inAnalMeter = false;


	public void GrabCard()
	{
			beastTip.SetActive(true);

			atm.infoTick.SetActive(true);

			if (!firstTimeStealer)
			{
				firstTimeStealer = true;

				if (!neilMode)
					beastlovania.PlayOneShot(mus_Theft);
			}

			FindObjectOfType<BeastCardArea>().ui_Card.SetActive(true);
	}

	public bool IteminInventory(int checkitem)
	{
		for (int i = 0; i < 5; i++)
		{
			if (item[i] == checkitem) {
				return true;
			}
		}
		return false;
	}

	public void MachineCheck()
	{
		Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit3;

		if (Physics.Raycast(ray3, out raycastHit3))
		{
			float bsodaPrice = 0.25f / sodaDiscount;
			float zestyPrice = 0.25f / zestyDiscount;
			float energyPrice = 0.50f / energyDiscount;
			float pearlPrice = 0.50f / pearlDiscount;

			if (raycastHit3.collider.name == "BSODAMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= bsodaPrice)
			{
				this.CollectItem(4);
				money.money -= bsodaPrice;
			}
			else if (raycastHit3.collider.name == "ZestyMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= zestyPrice)
			{
				money.money -= zestyPrice;
				this.CollectItem(1);
			}
			else if (raycastHit3.collider.name == "EnergyMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= energyPrice)
			{
				money.money -= energyPrice;
				this.CollectItem(17);
			}
			else if (raycastHit3.collider.name == "PEARLMachine" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= pearlPrice)
			{
				money.money -= pearlPrice;
				this.CollectItem(22);
			}
			else if (raycastHit3.collider.name == "PayPhone" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= 0.25f)
			{
				raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				money.money -= 0.25f;
			}
		}
	}

	private void UseItem()
	{
		Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit3;

		bool canUseOtherItem = true;

		// Other items
		if (canUseOtherItem)
		{
			if (this.item[this.itemSelected] != 0)
			{
				if (this.item[this.itemSelected] == 1) // Zesty Bar
				{
					audioDevice.PlayOneShot(aud_Nom);
					FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("Nom");
					this.player.stamina = this.player.maxStamina * 2f;

					if (!player.hasToPoop)
						player.poopCooldown -= 25;
					else
						player.poopMultiplier += 1f;

					this.ResetItem();
				}
				else if (this.item[this.itemSelected] == 2) // Lock
				{
					Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
					{
						raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().Lock(15f);
						this.ResetItem();
					}
				}
				else if (this.item[this.itemSelected] == 3) // Keys
				{
					Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit2;
					if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
					{
						DoorScript component = raycastHit2.collider.gameObject.GetComponent<DoorScript>();
						if (component.DoorLocked)
						{
							component.UnlockDoor();
							component.OpenDoor();
							this.ResetItem();
						}
					}
				}
				else if (this.item[this.itemSelected] == 4) // BSODA
				{
					player.playerAnimator.SetTrigger("BsodaShoot");

					UnityEngine.Object.Instantiate<GameObject>(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);

					ResetItem();
					player.ResetGuilt("drink", 1f);
					audioDevice.PlayOneShot(this.aud_Soda);
				}
				else if (this.item[this.itemSelected] == 6) // VHS
				{
					Ray ray4 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit4;
					if (Physics.Raycast(ray4, out raycastHit4) && (raycastHit4.collider.name == "TapePlayer" & Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
					{
						raycastHit4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.ResetItem();
					}
				}
				else if (this.item[this.itemSelected] == 7) // Clock
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem();
				}
				else if (this.item[this.itemSelected] == 8) // WD
				{
					Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit5;
					if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
					{
						raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
						this.ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Spray);
					}
				}
				else if (this.item[this.itemSelected] == 9) // G l o c k
				{
					Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit5;

					if (this.player.jumpRope)
					{
						audioDevice.PlayOneShot(beans.aud_spit);
						this.player.DeactivateJumpRope();
						this.vanman.Disappoint();
						this.ResetItem();
					}
					else if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.name == "MrBeast" && Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 50f && mrBeast.beasting))
					{
						MrBeastCutsceneStart();
						ResetItem();
						canPause = false;
						canUseOtherItem = false;
					}
				}
				else if (this.item[this.itemSelected] == 10) // Boots
				{
					this.player.ActivateBoots();
					base.StartCoroutine(this.BootAnimation());
					this.ResetItem();
				}
				else if (this.item[this.itemSelected] == 11 && !inAnalMeter) // Anal Meter
				{
					analMeter.SetActive(true);
					
					inAnalMeter = true;

					IEnumerator WaitTime()
					{
						yield return new WaitForSeconds(5f);
						inAnalMeter = false;
						analMeter.SetActive(false);
					}
					
					this.ResetItem();
					StartCoroutine(WaitTime());
				}
				else if (this.item[this.itemSelected] == 12) // Medkit
				{
					this.player.stamina = this.player.maxStamina * 4f;
					this.player.poopMultiplier = 1f;

					this.ResetItem();
				}
				else if (this.item[this.itemSelected] == 14) // I-phone
				{
					foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
					{
						if (sources.gameObject.name != "iPhone")
						{
							sources.Pause();
						}
					}
				
					phone.SetActive(true);
					Time.timeScale = 0f;
					canPause = false;
					gamePaused = true;
					UnlockMouse();
				}
				else if (this.item[this.itemSelected] == 15) // Acid Gas
				{
					UnityEngine.Object.Instantiate<GameObject>(acid, this.playerTransform.position, this.cameraTransform.rotation);

					this.ResetItem();
				}
				else if (item[itemSelected] == 16) // Hammer for Sticky Situations™
				{
					audioDevice.PlayOneShot(hammerHit);
		
					
					if (Physics.Raycast(ray3, out raycastHit3) && raycastHit3.collider.name == "1st Prize" && !firstPrize.gotHammered)
					{
						ResetItem();

						firstPrize.gotHammered = true;
						firstPrize.agent.acceleration -= 495;

						if (FindObjectOfType<PlayerStats>().data.objectasks[3] == 0)
						{
							FindObjectOfType<Objectasks>().CollectObjectask(3);
						}

						IEnumerator WaitToFix()
						{
							yield return new WaitForSeconds(10f);
							firstPrize.agent.acceleration += 495;
							firstPrize.gotHammered = false;
						}

						StartCoroutine(WaitToFix());
					}

					if (vanman.isKidnapped && !vanman.gotHammered)
					{
						ResetItem();

						vanman.gotHammered = true;
						vanman.kidnapTime = 0f;
					}

					// Break out of jail using hammer
					if (Physics.Raycast(ray3, out raycastHit3) && (raycastHit3.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f))
					{
						DoorScript component = raycastHit3.collider.gameObject.GetComponent<DoorScript>();
						if (component.DoorLocked)
						{
							audioDevice.PlayOneShot(hammerHit);

							component.UnlockDoor();
							component.OpenDoor();
							this.ResetItem();
						}
					}

					// Dent secret walls using hammer
					if (raycastHit3.transform.gameObject.name.ToLower().Contains("dentable"))
					{
						if (raycastHit3.collider.tag == "HammerableWindow" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							raycastHit3.transform.gameObject.GetComponent<MeshRenderer>().material = dentedSign;
							baldi.Hear(player.transform.position, 2f);

							ResetItem();
						}
					}
					// Hammer windows & vault
					else
					{
						if (raycastHit3.collider.tag == "Vault" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							for(int i = 0; i < 8; i++)
							{
								CollectItem(5);
							}

							raycastHit3.transform.gameObject.GetComponent<MeshRenderer>().material = brokenVault;
							moneySpray.SetActive(true);
							
							audioDevice.PlayOneShot(windowBreak);
							baldi.Hear(player.transform.position, 2f);
							ResetItem();
						}	
						if (raycastHit3.collider.tag == "BladderHammerable" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							if (!raycastHit3.transform.gameObject.GetComponent<BladderHammerable>().brokenAlready)
							{
								raycastHit3.transform.gameObject.GetComponent<BladderHammerable>().OnWindowBreak();
								raycastHit3.transform.gameObject.GetComponent<MeshRenderer>().material = brokenWindow;
								audioDevice.PlayOneShot(windowBreak);
								raycastHit3.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
								raycastHit3.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
								baldi.Hear(player.transform.position, 2f);
							}
						}
					}
				}
				else if (item[itemSelected] == 17) // 15 Second Energy
				{
					audioDevice.PlayOneShot(aud_Nom);
					FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("Nom");
					this.player.stamina = this.player.maxStamina * 2f;
					player.walkSpeed += 7f;
					player.runSpeed += 7f;

					IEnumerator WaitToDisable()
					{
						yield return new WaitForSeconds(15f);
						player.walkSpeed -= 7f;
						player.runSpeed -= 7f;
					}

					ResetItem();

					StartCoroutine(WaitToDisable());
				}
				else if (item[itemSelected] == 19) // Jail Card
				{
					Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit2;
					if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
					{
						DoorScript component = raycastHit2.collider.gameObject.GetComponent<DoorScript>();
						if (component.DoorLocked)
						{
							component.UnlockDoor();
							component.OpenDoor();
							component.lockTime = 0f;

							if (FindObjectOfType<PlayerStats>().data.objectasks[1] == 0)
							{
								FindObjectOfType<Objectasks>().CollectObjectask(1);
							}

							ResetItem();
						}
					}
				}
				else if (item[itemSelected] == 20) // M o n k e
				{
					if (!player.isMonke)
					{
						player.BecomeMonke();

						if (mode == "hard" && hardSecondPhase)
						{
							monkeMorbin.volume = 1f;
							monkeMorbin.time = endSong.time;
						}
							
						
						ResetItem();

						IEnumerator WaitTime()
						{
							yield return new WaitForSeconds(monketime);
							player.DisableMonke();

							if (mode == "hard" && hardSecondPhase)
								monkeMorbin.volume = 0f;
						}

						StartCoroutine(WaitTime());
					}
				}
				else if (item[itemSelected] == 22) // Ender Pearl
				{
					//player.playerAnimator.SetTrigger("BsodaShoot");
				} 
				else if (item[itemSelected] == 24)
				{
					if (!gameBoyOpen)
					{
						gameBoy.SetActive(true);
						canPause = false;
						canUseOtherItem = false;
						player.inNotebook = true;
						StopAllEvents();
						allowEvents = false;
						dogRoomAudio.volume = 0;
						gameBoyOpen = true;
					}
					else if (gameBoyOpen)
					{
						gameBoy.SetActive(false);
						canPause = true;
						canUseOtherItem = true;
						player.inNotebook = false;
						allowEvents = true;
						dogRoomAudio.volume = 1;
						gameBoyOpen = false;
					}
				}
				else if (this.item[this.itemSelected] == 25) // Polish
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.polishMarkerPrefab, this.playerTransform.position, this.cameraTransform.rotation);
					this.ResetItem();
				}
				else if (this.item[this.itemSelected] == 26) // Portal
				{
					if (Physics.Raycast(ray3, out raycastHit3) && raycastHit3.collider.name == "PortalOpenTrigger")
					{
						bool dumb = false;

						raycastHit3.transform.gameObject.GetComponent<BoxCollider>().isTrigger = true;
						
						foreach(Transform child in raycastHit3.transform)
						{
							if (!dumb)
							{
								child.gameObject.SetActive(true);
								dumb = true;
								audioDevice.PlayOneShot(bang);
							}
						}

						foreach(GameObject bposter in ballposters)
						{
							bposter.SetActive(false);
						}

						this.ResetItem();
					}
				}
			} else {
				if (neilMode && GasAmmo > 0)
				{
					UnityEngine.Object.Instantiate<GameObject>(acid, this.playerTransform.position, this.cameraTransform.rotation);
					audioDevice.PlayOneShot(aud_GasCanister);
					GasAmmo -= 1;
				}
			}
		}
	}

	public AudioClip bang;
	public AudioSource dogRoomAudio;

	public void ClearItems()
	{
		for (int i = 0; i < 5; i++)
		{
			item[i] = 0;
		}

		UpdateItemName();
	}

	public void RemoveItem(int id)
	{
		for (int i = 0; i < 5; i++)
		{
			if (item[i] == id)
			{
				this.item[i] = 0;
				this.itemSlot[i].sprite = itemSprites[0];
				UpdateItemName();
				break;
			}
		}
	}


	private IEnumerator BootAnimation()
	{
		float time = 15f;
		float height = 375f;
		Vector3 position = default(Vector3);
		this.boots.gameObject.SetActive(true);

		while (height > -375f)
		{
			height -= 375f * Time.deltaTime;
			time -= Time.deltaTime;
			position  = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}

		position = this.boots.localPosition;
		position.y = -375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);

		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}

		this.boots.gameObject.SetActive(true);
		while (height < 375f)
		{
			height += 375f * Time.deltaTime;
			position = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}

		position = this.boots.localPosition;
		position.y = 375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);
		yield break;
	}

	public void ResetItem()
	{
		if (!(debug && noreset)) {
			this.item[this.itemSelected] = 0;
			this.itemSlot[this.itemSelected].sprite = itemSprites[0];
			this.UpdateItemName();
		}
	}

	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].sprite = itemSprites[0];
		this.UpdateItemName();
	}

	private void UpdateItemName()
	{
		this.itemText.text = this.itemNames[this.item[this.itemSelected]];
		itemSprite.sprite = itemSprites[item[itemSelected]];
	}

	public IEnumerator ReEnablePauseDumbness()
	{
		yield return null;
		canPause = true;
	}
}*/
