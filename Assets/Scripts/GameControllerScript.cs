using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Rendering.PostProcessing;

using TMPro;

using Rewired;

public class GameControllerScript : MonoBehaviour
{
	public static GameControllerScript current;

	[Header("Debug")]
	public bool debug = true;
	public float monketime = 10;
	 	
	[Header("Hard Mode")]
	public AudioSource monkeMorbin;
	public AudioSource portalaudiosource;
	public AudioClip portalenter;
	public AudioClip portalexit;
	public Text phonehitmantext;
	public TextMeshProUGUI addscoretext;
	public Animator addscoretextanimator;
	public GameObject scoredisplay;
	public int score;
	public int rank;
	public int[] scoretargets = new int[5];
	private float removescoretimer = 1;
	public float endgameTimeLeft = 180;
	public float endgameTimeStart = 180;
	public bool endgame = false;
	public GameObject timeLeftUI;
	public GameObject lap2cutscene;
	public Slider timeLeftSlider;
	public TextMeshProUGUI timeLeftText;
	private float deathanimtime = -2f;
	private bool healthup = false;
	private bool MetalMode = false;
	private Transform thing;	

	[Header("Script References")]
	public CursorControllerScript cursorController;
	public MoneyManager money;
	public PostProcessVolume ppVolume;
	public LoadingScreen loading;
	public LoadingScreen realLoading;
	public DoorScript jailDoor;
	public ATMScript atm;
	public PlayerScript player;
	public Billboard mrBeastBillboard;
	public BaldiCharacterManager bcm;
	public SolitareCreditsOriginal solitareCredits;
	public CoulsonEngine.Sound.Music.MusicBeat musicBeat;
	private Player input;
	[HideInInspector] public NotebookScript hard10THBook;

	// Post Processing
	[HideInInspector] public ChromaticAberration chromaticEffect;
	[HideInInspector] public Bloom bloomEffect;
	[HideInInspector] public AutoExposure exposure;

	[Header("Characters")]
	public BaldiScript baldi;
	public PrincipalScript chipfloke;
	public PlaytimeScript vanman;
	public BullyScript bully;
	public CraftersScript crafters;
	public SweepScript gottaWeep;
	public FirstPrizeScript firstPrize;
	public MrBeastScript mrBeast;
	public BeansScript beans;
	public FunnyMan present;
	public PolishCow polishCow;
	public WaltuhScript walter;
	public NeilScript neil;

	[Header("Exits")]
	public EntranceScript entrance_0;
	public EntranceScript entrance_1;
	public EntranceScript entrance_2;
	public EntranceScript entrance_3;

	[Header("Cameras")]
	public Camera pcamera;
	public Camera uncannyCamera;
	public Camera mapCamera;
	public Camera minimapCamera;
	
	[Header("Objects")]
	public GameObject fadeDownCamera;
	public GameObject minimapstatic;
	public Transform playerTransform;
	public Transform cameraTransform;

	public GameObject debugModeText;
	public GameObject raldiStyle;
	public GameObject mrBeastCutscene;
	public GameObject map;
	public GameObject neilOnlyItems;
	public GameObject ammoGmodReference;
	public GameObject neilHealthbar;
	public GameObject analMeter;
	public GameObject uncannyLmao;
	public GameObject moneySpray;
	public GameObject pauseMenu;
	public GameObject hitmanNotif;
	public GameObject highScoreText;
	public GameObject acid;
	public GameObject warning;
	public GameObject baldiTutor;
	public GameObject quarter;
	public GameObject dogCall;
	public GameObject noiseCall;
	public GameObject rythmMinigame;
	public GameObject baldiPosFunny;
	public GameObject allIKnow;
	public GameObject fakeBaldi;
	public GameObject cuzsie;
	public GameObject phone;
	public GameObject hud;
	public GameObject sansblackscreen;
	public GameObject beastTip;
	public GameObject threewayblocks;
	public GameObject secondaryCamera;
	private GameObject robject;

	public BoxCollider secondaryPP;

	public TextMeshPro NOTtext;
	public TextMeshProUGUI nbLabel;

	public GameObject[] ballposters;
	public GameObject[] bathroomHighlights;
	public Transform[] neilSpawns;
	public Transform[] deathCameras;

	[Header("Prefabs")]
	public GameObject gameBoy;
	public GameObject phase2Prefab;
	public GameObject mrBeastDeltarune;
	public GameObject undertalefight;
	public GameObject bsodaSpray;
	public GameObject enderPearl;
	public GameObject alarmClock;
	public GameObject polishMarkerPrefab;
	public GameObject peterPrefab;
	public GameObject neilBook;
	public GameObject neilProjectile;
	public GameObject basicSlots;
	public GameObject slots;
	public GameObject[] posters;

	[Header("Animation")]
	public Animator splatter;
	public Animator hudAnimator;
	public Animator hotbarAnimator;
	public Animator basicHotbarAnimator;
	public Animator[] explosionAnimators = new Animator[5];
	public Animator[] basicExplosionAnimators = new Animator[5];
	public Animator wiigolfslideranim;
	
	[Header("Music")]
	public AudioClip dankSchool;
	public AudioClip getFuckinTrolled;
	public AudioClip mus_Uncanny;
	public AudioClip baldiHome;
	public AudioClip morbinintro;
	public AudioClip morbinloop;
	public AudioClip morbinlap2;
	public AudioClip monkeLap2;
	public AudioClip doubleTroubleSchool;
	public AudioSource[] ambientMusics;

	[Header("Sounds")]
	public AudioClip pickup;
	public AudioClip cash;
	public AudioClip neilCash;
	public AudioClip tf2nope;
	public AudioClip robloxrocketsound;
	public AudioClip splatterSound;
	public AudioClip aud_PearlThrow;
	public AudioClip aud_PearlLand;
	public AudioClip aud_Soda;
	public AudioClip aud_Spray;
	public AudioClip aud_Exit;
	public AudioClip aud_GasCanister;
	public AudioClip aud_Nom;
	public AudioClip aud_ammoGrab;
	public AudioClip aud_buzz;
	public AudioClip mus_Theft;
	public AudioClip aud_NeilHPBeep;
	public AudioClip aud_MerryChristmas;
	public AudioClip hammerHit;
	public AudioClip dialing;
	public AudioClip pickUpPhone;
	public AudioClip talkingOnPhone;
	public AudioClip windowBreak;
	public AudioClip neilDeath;
	public AudioClip ishaanDeath;
	public AudioClip allNotebooks;
	public AudioClip PlayerDamageSound;
	public AudioClip portalOpen;

	public AudioClip[] baldiCountdown;

	[Header("Material")]
	public Material confusingPoster;
	public Material troll;
	public Material blackSky;
	public Material galaxySky;
	public Material defaultSky;
	public Material dentedSign;
	public Material brokenVault;
	public Material brokenWindow;

	[Header("Images")]
	public Sprite trollSprite;

	public Sprite[] itemSprites = new Sprite[10];
	public int[] rewarditems = new int[10];

	[Header("AudioSource")]
	public AudioSource audioDevice;
	public AudioSource schoolMusic;
	public AudioSource beastlovania;
	public AudioSource learnMusic;
	public AudioSource tutorBaldi;
	public AudioSource ambience;
	public AudioSource neilambience;
	public AudioSource endSong;

	[Header("Colors")]	
	public Color allIColor;
	public Color neilColor;
	public Color dark;
	[HideInInspector] public Color targetSchoolColor;

	[Header("Videos")]
	public VideoPlayer allIKnowVideo;

	[Header("UI")]
	public TextMeshProUGUI ammotext;
	public TextMeshProUGUI phase2sprinttext;
	public TMP_Text itemText;
	public TMP_Text notebookCount;
	public RectTransform boots;
	public RectTransform NeilHealthbar;
	public RectTransform NeilPlayerHealthbar;
	public GameObject reticle;
	public GameObject throwProjectileText;
	public GameObject morbintime;
	public Slider wiigolfslider;

	public Image[] itemSlot = new Image[5];
	public RawImage[] itemBGS = new RawImage[3];

	public Image[] basicitemSlot = new Image[3];
	public RawImage[] basicitemBGS = new RawImage[3];

	[Header("Sprites")]
	public SpriteRenderer itemSprite;

	[Header("Machine Discounts")]
	public int sodaDiscount = 1;
	public int zestyDiscount = 1;
	public int energyDiscount = 1;
	public int pearlDiscount = 1;

	[Header("Varriables")]

	public float methTimer = 300f;
	public float methPrice = 0.25f;
	public bool isHoldingNeilObject = false;
	private bool gameBoyOpen = false;
	public string mode;
	private string lastEvent = "";

	public float iframes;
	public float timepearlhelddown = 0;
	public float neilhp = 0;
	public float hptarget = 28;
	public float neilPlayerHP = 28f;
	public float eventDelay = 10f;
	public float time;
	public float playTime;
	private float neilhptimer = 0;
	public float gameOverDelay;
	
	public int neilwallspawnchancemax = 5;
	public int exitsReached;
	public int itemSelected;
	public int notebooks;
	public int beastnotebooks = 0;
	public int notebooksToCollect;
	public int neilKeysCollected = 0;
	public int GasAmmo;
	public int currentDeathCamera = 0;
	
	public bool mode2016 = false;
	public bool trueReset = false;
	public bool inBathroom;
	public bool forceNeilMode = false;
	public bool startedNeilAnimation = false;
	public bool allowEvents = true;
	public bool destroyMeshes = false;
	public bool canPause = true;
	public bool neilMode = false;
	public bool trollMode = false;
	public bool neilModeExits = false;
	public bool dogActive = false;
	public bool rareEventActivated = false;
	public bool hpgenerating = false;
	public bool creditGameOver = false;
	public bool gmBugFixer = false;
	public bool eventActive = false;
	public bool beastCardCollected = false;
	public bool firstTimeStealer = false;
	private bool BallPostersEnabled = true;
	public bool spoopMode;
	public bool finaleMode;
	public bool mouseLocked;
	public bool gamePaused;
	public bool learningActive;
	public bool inventoryfull;
	public bool noreset;
	public bool hardSecondPhase = false;
	public bool secondPhase10thCollected = false;
	public bool baldiStyle = false;
	public bool killedmrBeast;
	public bool startinMrBeastDeltarune;
	public bool startinUndertale;
	public bool doubleTrouble = false;
	public bool vanmaninprison = false;
	public bool ishaanKeys = false;
	public bool throwingpearl;

	public int[] bannedneilitems = new int[2]{14, 10};
	public int[] neilitemstogas = new int[4]{13, 11, 9, 3};
	

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
	[Header("Don't forget to change these in the delta prefab!")]

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

	private bool canUseOtherItem = true;

	public int[] item = new int[3];

	[Header("Neil Mode")]
	public GameObject neilLetter;

	public void ExitGame() {realLoading.gameObject.SetActive(true); StartCoroutine(realLoading.LoadingLoadScene("MainMenu"));}
	public void Reset() {loading.gameObject.SetActive(true); StartCoroutine(loading.LoadingLoadScene("School"));}
	public void Destroy(GameObject target) => GameObject.Destroy(target);

	public AudioClip bus;

	public bool ishaanMode = false;

	ChromaticAberration pp2chromatic;
	LensDistortion pp2distortion;
	Vignette pp2Vig;

	public GameObject playerGlasses;

	private void Start()
	{
		print(Calculations.ToRomanNumeral(14));
		FindObjectOfType<PlayerStats>().TryLoad();

		scoredisplay.SetActive(false);
		current = this;

		mode = PlayerPrefs.GetString("CurrentMode");
		audioDevice = base.GetComponent<AudioSource>();
		notebooksToCollect = FindObjectsOfType<NotebookScript>().Length;
		posters = GameObject.FindGameObjectsWithTag("PosterWall");
		secondaryCamera.GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("FOV");
		input = ReInput.players.GetPlayer(0);
		
		itemSelected = 0;
		gameOverDelay = 0.5f;

		// Get post processing profiles
		ppVolume.profile.TryGetSettings(out chromaticEffect);
		ppVolume.profile.TryGetSettings(out bloomEffect);
		ppVolume.profile.TryGetSettings(out exposure);
		volume2.profile.TryGetSettings(out pp2chromatic);
		volume2.profile.TryGetSettings(out pp2distortion);
		volume2.profile.TryGetSettings(out pp2Vig);
		

		PlayerPrefs.SetInt("record endless", 0);
		PlayerPrefs.SetInt("cursessionendless", 0);

		if (FindObjectOfType<PlayerStats>().data.recordTime == 0)
		{
			FindObjectOfType<PlayerStats>().data.recordTime = 9999;
			FindObjectOfType<PlayerStats>().Save();
		}

		if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
		{
			itemSlot = basicitemSlot;
			itemBGS = basicitemBGS;

			slots.SetActive(false);
			basicSlots.SetActive(true);
		}

		StartGame();
	}

	public AudioClip crackhouseMusicBox;
	public Color nightdark;
	public GameObject playerFlashlight;
	public Material nightSky;
	public Shader standard;

	public void StartGame()
	{
		ammoGmodReference.SetActive(neilMode);
		if (PlayerPrefs.GetFloat("wrong_mode") == 1)
		{
			TransformtoWrongModeItems();
			ammoGmodReference.SetActive(true);
		}
		if (FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked && PlayerPrefs.GetFloat("StartInIshaan") == 1)
		{
			FindObjectOfType<PlayerStats>().data.ishaanMenu = false;
			FindObjectOfType<PlayerStats>().Save();
			neilMode = true;
			ishaanMode = true;
		}

		if (PlayerPrefs.GetFloat("crack_delivery") == 1)
		{
			foreach(NavMeshAgent agent in FindObjectsOfType<NavMeshAgent>())
			{
				agent.speed = agent.speed * 2;
			}

			baldi.speed = baldi.speed * 2;
		}

		if (PlayerPrefs.GetFloat("after_dark") == 1)
		{
			schoolMusic.clip = crackhouseMusicBox;
			RenderSettings.ambientLight = nightdark;
			RenderSettings.skybox = nightSky;
			playerFlashlight.SetActive(true);

			foreach(SpriteRenderer sprites in FindObjectsOfType<SpriteRenderer>())
			{
				sprites.color = nightdark;
			}
		}

		if (PlayerPrefs.GetFloat("bad_sleep_schedule") == 1)
		{
			foreach(NavMeshAgent agent in FindObjectsOfType<NavMeshAgent>())
			{
				agent.speed = agent.speed * 0.5f;
			}

			baldi.speed = baldi.speed * 0.5f;
		}

		if (!forceNeilMode && !ishaanMode) neilMode = FindObjectOfType<PlayerStats>().data.interactedWithNeil;
		else neilMode = true;
		
		neilOnlyItems.SetActive(neilMode);

		targetSchoolColor = Color.white;

		if (PlayerPrefs.GetString("Troll") == "troll")
		{
			trollMode = true;
			PlayerPrefs.SetString("Troll", "NoTroll");
		}

		if (!neilMode && !trollMode)
			schoolMusic.Play();
		
		if (neilMode)
		{
			RenderSettings.ambientLight = neilColor;
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.black;
			RenderSettings.fogDensity = 0.005f;
			RenderSettings.skybox = blackSky;

			GameObject.Destroy(tutorBaldi.gameObject);

			baldi.gameObject.SetActive(true);
			ammoGmodReference.SetActive(true);
			neilOnlyItems.SetActive(true);
			minimapstatic.SetActive(true);

			BallPostersEnabled = false;
			spoopMode = true;
			allowEvents = false;

			baldi.GetAngry(1f);
			baldi.BecomeNeil();
			baldi.speed = 0;

			player.playerCanPoop = false;

			schoolMusic.Stop();
			
			HideChar();
			TransformtoNeilItems();
		}
		else if (mode2016)
		{
			schoolMusic.clip = dankSchool;
			musicBeat.BPM = 162;
			musicBeat.beatHit.AddListener(BeatHit);
			musicBeat.syncBPM();

			foreach(GameObject obj in ballposters)
			{
				obj.GetComponent<BoxCollider>().isTrigger = false;
			}

			allowEvents = false;

			foreach(AudioSource ambiences in ambientMusics)
			{
				ambiences.volume = 0;
			}

			spoopMode = true;
				
			baldi.raldiSpeeed = true;
			baldi.speed = 10f;
			baldi.poses[0].SetActive(false);
			baldi.poses[3].SetActive(true);

			player.walkSpeed += 5;
			player.runSpeed += 5;

			nbLabel.text = "DANKBOOKS";

			playerGlasses.SetActive(true);

			ShowChar();

			GameObject.Destroy(tutorBaldi.gameObject);

			player.cc.enabled = false;
			player.transform.position = solitareCredits.playerAfterSpawn.position;
			player.cc.enabled = true;

			ColorGrading color;
			
			ppVolume.GetComponent<BoxCollider>().enabled = true;
			bloomEffect.active = false;
			chromaticEffect.active = false;
			ppVolume.profile.TryGetSettings(out color);
			color.active = true;
		}
		else if (mode == "hard")
		{
			scoredisplay.SetActive(FindObjectOfType<PlayerStats>().data.beatHardMode);
			phonehitmantext.text = "Cash Drop";
			player.allowMockRunning = true;
			allowEvents = false;

			spoopMode = true;
				
			baldi.raldiSpeeed = true;
			baldi.speed = 10f;
			baldi.poses[0].SetActive(false);
			baldi.poses[3].SetActive(true);

			musicBeat.beatHit.AddListener(BeatHit);
			musicBeat.BPM = 169.444f;
			musicBeat.syncBPM();

			StartCoroutine(ActivateExits());

			endSong.loop = true;
			endSong.clip = limboReference;
			endSong.Play();

			schoolMusic.Stop();

			entrance_0.Lower();
			entrance_1.Lower();
			entrance_2.Lower();
			entrance_3.Lower();
			entrance_0.Lower();
			entrance_1.Lower();
			entrance_2.Lower();
			entrance_3.Lower();

			ShowChar();

			GameObject.Destroy(tutorBaldi.gameObject);

			player.cc.enabled = false;
			player.transform.position = solitareCredits.playerAfterSpawn.position;
			player.cc.enabled = true;

			BallPostersEnabled = false;
			ToggleAmbiences(false);
			UpdateNotebookCount();
		}

		if (mode == "endless")
		{
			baldi.endless = true;
			PlayerPrefs.SetInt("cursessionendless", 1);
		}

		if (trollMode)
		{
			GameObject.Destroy(tutorBaldi.gameObject);
			notebooks = 2;
			spoopMode = true;
			baldi.gameObject.SetActive(true);
			baldi.GetAngry(15f);
			baldi.GetComponent<AudioSource>().volume = 0f;
			baldi.baldiAnimator.enabled = false;
			schoolMusic.clip = getFuckinTrolled;
			schoolMusic.Play();

			foreach(MeshRenderer render in FindObjectsOfType<MeshRenderer>())
			{
				render.material = troll;
			}

			foreach(SpriteRenderer render in FindObjectsOfType<SpriteRenderer>())
			{
				render.sprite = trollSprite;
			}
		}

		if (mode == "story_double")
		{
			schoolMusic.clip = doubleTroubleSchool;
			schoolMusic.Play();
		}

		if (NOTtext != null) NOTtext.enabled = !BallPostersEnabled;

		if (startinMrBeastDeltarune) MrBeastDeltarune();
		if (startinUndertale) StartUndertale();

		audioDevice.PlayOneShot(bus);
		
		IEnumerator CameraFunnies()
		{
			if (mode != "hard" && !FindObjectOfType<PlayerStats>().data.ishaanTimeWahoo)
			{
				fadeDownCamera.SetActive(true);
				player.playerVCam.gameObject.SetActive(false);
				yield return null;
				fadeDownCamera.SetActive(false);
				player.playerVCam.gameObject.SetActive(true);
			}
		}

		ToggleBallsPosters(BallPostersEnabled);
		LockMouse();
		UpdateNotebookCount();
		StartCoroutine(CameraFunnies());
	}

	public void ToggleBallsPosters(bool toggle)
	{
		foreach(GameObject obj in ballposters)
		{
			obj.GetComponent<BoxCollider>().isTrigger = toggle;
		}
	}

	public void ToggleAmbiences(bool toggle)
	{
		foreach(AudioSource ambiences in ambientMusics)
		{
			if (!toggle)
				ambiences.volume = 0;
			else
				ambience.volume = 1;
		}
	}

	public Material wall;

	public AudioClip limboReference;
	public bool holdingpearl;
	private bool endgamelock = false;

	private bool endgametimelock = false;
	public AudioClip aud_timeout;
	public AudioClip aud_megamanexplode;

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

		if (tickUpChromatic)
			pp2chromatic.intensity.value += Time.deltaTime / 5;
		if (tickUpDistortion)
			pp2distortion.intensity.value -= Time.deltaTime * 14;
		if (tickUpVig)
			pp2Vig.intensity.value += Time.deltaTime / 15;

		if (finaleMode)
			allowEvents = false;

		if (PlayerPrefs.GetFloat("after_dark") == 1)
		{
			foreach(SpriteRenderer sprites in FindObjectsOfType<SpriteRenderer>())
			{
				sprites.color = nightdark;
			}

			if (RenderSettings.skybox != nightSky)
				RenderSettings.skybox = nightSky;
		}

		if (endgame)
		{
			if (!endgametimelock)
				removescoretimer -= Time.deltaTime;
			if (removescoretimer <= 0)
			{
				removescoretimer = 1;
				RemoveScore(5);
			}
			if (endgameTimeLeft > 0)
			{
				if (!endgametimelock)
					endgameTimeLeft -= Time.deltaTime;
				timeLeftSlider.value = 1-(endgameTimeLeft/endgameTimeStart);
			} else if (!endgamelock) {
				endgameTimeLeft = 0;
				audioDevice.PlayOneShot(aud_timeout, 3f);
				baldi.TargetPlayer();
				baldi.baldicator.SetTrigger("Hear");
				baldi.baldicator.SetTrigger("Glow");
				baldi.speed *= 2;
				endgamelock = true;
			} else {
				//baldi.TargetPlayer();
			}
			timeLeftText.text = Calculations.GetFormattedTime(endgameTimeLeft);
		}
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
						//player.ResetGuilt("drink", 1f);
				audioDevice.PlayOneShot(aud_PearlThrow);
			} else {
				throwingpearl = false;
				timepearlhelddown = 0;
			}
		} else {
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

		if (!neilMode) 
		{
			neilambience.Stop();
		}

		if (player.gameOver && neilMode)
		{
			if (!ishaanMode)
			{
				deathanimtime += Time.deltaTime;
				if (deathanimtime > 0) {
					foreach(GameObject objectmoverthing in FindObjectsOfType<GameObject>())
					{
						if  (!objectmoverthing.name.ToLower().Contains("baldi") && !objectmoverthing.name.ToLower().Contains("neil") && !objectmoverthing.name.ToLower().Contains("neilsprite") && !objectmoverthing.name.ToLower().Contains("area light") && !objectmoverthing.name.ToLower().Contains("main camera") && !objectmoverthing.gameObject.name.ToLower().Contains("player"))
						{
							objectmoverthing.transform.position = objectmoverthing.transform.position + new Vector3(UnityEngine.Random.Range(-deathanimtime,deathanimtime),UnityEngine.Random.Range(-deathanimtime,deathanimtime),UnityEngine.Random.Range(-deathanimtime,deathanimtime)); 
							//objectmoverthing.transform.localScale = objectmoverthing.transform.localScale + new Vector3(UnityEngine.Random.Range(-deathanimtime/5,deathanimtime/5),UnityEngine.Random.Range(-deathanimtime/5,deathanimtime/5),UnityEngine.Random.Range(-deathanimtime/5,deathanimtime/5)); 
						}
					}
				}
			}
		}
		if (iframes > 0)
			iframes -= Time.deltaTime;
		else
			iframes = 0;
		if ((finaleMode && !neilMode || mode == "hard" || mode2016) && !(PlayerPrefs.GetFloat("after_dark") == 1))
			RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetSchoolColor, 0.01f);

		PassTime();
		UpdateHUD();
		InputCheck();
		GameOverCheck();
		InventoryCheck();
		
		if (neilMode) NeilHPCheck();
		int quickcheck = 7 - (7 - (Mathf.RoundToInt(neilhp/2)));
		if (FindObjectOfType<NeilScript>() != null)
			threewayblocks.SetActive(neilhp < 15 && FindObjectOfType<NeilScript>().startedBoss && healthup);
		if (quickcheck >= 0)
			neilwallspawnchancemax = quickcheck;
		else
			neilwallspawnchancemax = 1;
	}

	public AudioClip surreal2;
	public TextMeshProUGUI fakeEventText;
	public GameObject postIshaanEnable;
	public GameObject[] portalOpenTriggers;

	public void IshaanDeath()
	{
		player.ResetSpeed();
		hud.SetActive(true);
        player.infiniteStamina = false;
        player.staminaByDefault = false;

		neilHealthbar.SetActive(false);
		neilhp = 999;
		RenderSettings.skybox = blackSky;
		audioDevice.PlayOneShot(aud_megamanexplode, 2f);
		Destroy(neil.gameObject);

		IEnumerator Surreality()
		{
			foreach(GameObject poster in posters)
			{
				poster.GetComponent<MeshRenderer>().material = confusingPoster;
			}
			foreach(GameObject obj in portalOpenTriggers)
			{
				obj.SetActive(true);
			}
			
			postIshaanEnable.SetActive(true);
			yield return new WaitForSeconds(2);
			yield return new WaitForSeconds(2f);
            audioDevice.PlayOneShot(FindObjectOfType<ObjectasksScreen>().events);
            yield return new WaitForSeconds(2f);
            GameControllerScript.current.schoolMusic.clip = surreal2;
            GameControllerScript.current.schoolMusic.Play();
			fakeEventText.text = "Party at the principals office! Come get your present!";
            FindObjectOfType<ObjectasksScreen>().eventDisp.SetActive(true);
            yield return new WaitForSeconds(4f);
            FindObjectOfType<ObjectasksScreen>().eventDisp.SetActive(false);
		}

		StartCoroutine(Surreality());
	}

	public void GiveScore(int give)
	{
		addscoretext.text = "+" + give;
		addscoretext.color = Color.green;
		addscoretextanimator.SetTrigger("score");
		score += give;
		CheckRank();
	}

	public void RemoveScore(int remove)
	{
		score -= remove;
		addscoretext.text = "-" + remove;
		addscoretext.color = Color.red;
		addscoretextanimator.SetTrigger("score");
		CheckRank();
	}
	public void TransformtoNeilItems()
	{
		foreach(PickupScript pickup in FindObjectsOfType<PickupScript>())
		{
			if (neilitemstogas.Contains(pickup.id))
			{
				pickup.id = 15;
				pickup.storeid = 15;
				pickup.itemType = ItemType.GasCanister;

				int index = 0;
				foreach(Transform children in pickup.transform)
				{
					if (index == 0)
					children.GetComponent<SpriteRenderer>().sprite = itemSprites[15];

					index++;
				}
				
			} 
			else if (bannedneilitems.Contains(pickup.id)) 
			{
				pickup.id = 0;
				pickup.droppedItem = true;
			}
		}
	}
	public void TransformtoWrongModeItems()
	{
		foreach(PickupScript pickup in FindObjectsOfType<PickupScript>())
		{
			if (neilitemstogas.Contains(pickup.id) && pickup.itemType != ItemType.CreditCard)
			{
				pickup.id = 15;
				pickup.itemType = ItemType.GasCanister;

				int index = 0;
				foreach(Transform children in pickup.transform)
				{
					if (index == 0)
					children.GetComponent<SpriteRenderer>().sprite = itemSprites[15];

					index++;
				}
				
			} 
			else if (bannedneilitems.Contains(pickup.id)) 
			{
				pickup.id = 0;
				pickup.droppedItem = true;
			}
		}
	}


	public void CheckRank()
	{
		int carry = 0;
		for (int i = 0; i < 5; i++)
		{
			if (score >= scoretargets[i])
			{
				carry += 1;
			}
		}
		rank = carry;
	}
	public Sprite ishaanhealthbar;
	public void GenerateHP()
	{
		if (ishaanMode)
			neilHealthbar.GetComponent<Image>().sprite = ishaanhealthbar;
		neilHealthbar.SetActive(true);
		IEnumerator WaitForDropDown()
		{
    		yield return new WaitForSeconds(0.8f);
			hpgenerating = true;
		}
		StartCoroutine(WaitForDropDown());
	}

	float startFixedDelta;
	
	public void SlowMo()
	{
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = startFixedDelta * 0.1f;
		canPause = false;
		sloMo = true;

		print("sloow mooo");
	}

	public void UnSlowMo()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = startFixedDelta;

		canPause = true;
		sloMo = false;

		print("normal speed");
	}

	public void PlayerDamage(float damage)
	{
		if (!(iframes > 0))
		{
			neilPlayerHP -= damage;
			iframes = 0.5f;
		}
		audioDevice.PlayOneShot(PlayerDamageSound);
	}

	public void MrBeastCutsceneStart()
	{
		IEnumerator Cutscene()
		{
			beastlovania.Stop();
			
			player.inNotebook = true;
			HideChar();
			mrBeast.gameObject.SetActive(true);
			mrBeast.music.Stop();
			mrBeast.agent.speed = 0;
			StopAllEvents();
			allowEvents = false;
			mrBeastBillboard.enabled = false;
			audioDevice.PlayOneShot(beans.aud_spit);
			yield return new WaitForSeconds(2);
			mrBeastCutscene.SetActive(true);
			yield return new WaitForSeconds(float.Parse(mrBeastCutscene.GetComponent<VideoPlayer>().clip.length.ToString()));
			MrBeastDeltarune();
			mrBeastCutscene.SetActive(false);
		}

		StartCoroutine(Cutscene());
	}

	private void GameOverCheck()
	{
		bool modifiersOn = false;

		foreach(Modifier modifier in ModifierMenu.modifiers)
		{
			if (PlayerPrefs.GetFloat(ModifierMenu.ConvertToInternal(modifier.name)) == 1)
				modifiersOn = true;
		}

		if (player.gameOver)
		{
			if (!neilMode)
			{
				if (mode == "endless")
				{
					if (!modifiersOn)
					{
						if (notebooks > FindObjectOfType<PlayerStats>().data.highBooks && !highScoreText.activeSelf)
						{
							highScoreText.SetActive(true);
							PlayerPrefs.SetInt("record endless", 1);
							FindObjectOfType<PlayerStats>().data.highBooks = notebooks;
							GameJolt.API.Scores.Add(notebooks, notebooks + " Notebooks", 804800);
						}
					}
					else
					{
						if (notebooks > FindObjectOfType<PlayerStats>().data.modifhighBooks && !highScoreText.activeSelf)
						{
							highScoreText.SetActive(true);
							PlayerPrefs.SetInt("record endless", 1);
							FindObjectOfType<PlayerStats>().data.modifhighBooks = notebooks;
						}
					}
				}

				if (!chipfloke.isSolitareConfined)
					Time.timeScale = 0f;

				gameOverDelay -= Time.unscaledDeltaTime * 0.5f;

				if (!chipfloke.isSolitareConfined || chipfloke.isSolitareConfined && !creditGameOver)
					pcamera.farClipPlane = gameOverDelay * 400f;

				audioDevice.PlayOneShot(aud_buzz);

				if (gameOverDelay <= 0f)
				{
					if (!chipfloke.isSolitareConfined)
					{	
						Time.timeScale = 1f;

						if (!gmBugFixer)
						{
							FindObjectOfType<PlayerStats>().Save();
							SceneManager.LoadSceneAsync("GameOver");
							gmBugFixer = true;
						}
					}
					else if (!creditGameOver)
					{
						creditGameOver = true;
						Time.timeScale = 1f;
						
						solitareCredits.blackScreen.SetActive(true);

						HideChar();
					}
					
				} else {
					PlayerPrefs.SetInt("recentNormalModeNotebooks", this.notebooks);
					PlayerPrefs.SetInt("recentNormalModeExits", this.exitsReached);
				}
				FindObjectOfType<PlayerStats>().Save();
			}
			else if (neilMode && !startedNeilAnimation)
			{
				canPause = false;
				
				startedNeilAnimation = true;
				baldi.sayshitcooldown = 9999f;
				baldi.timeToMove = 9999f;
				IEnumerator WaitTime()
				{
					if (FindObjectOfType<NeilScript>() != null)
						if (FindObjectOfType<NeilScript>().startedBoss)
							FindObjectOfType<NeilScript>().midi.Tempo = 0.001f;

					player.enabled = false;
					destroyMeshes = true;
					if (!ishaanMode)
					{
						audioDevice.PlayOneShot(neilDeath);
						yield return new WaitForSeconds(neilDeath.length);
						Application.Quit();
						print("closed");
					}
					else
					{
						audioDevice.PlayOneShot(ishaanDeath);
						yield return new WaitForSeconds(ishaanDeath.length - 0.1f);
						SceneManager.LoadScene("MainMenu");
					}
					if (FindObjectOfType<NeilScript>() != null)
					{
						if (FindObjectOfType<NeilScript>().phase2)
							SceneManager.LoadScene("NeilTip");
					}
				}
				
				StartCoroutine(WaitTime());
			}
		}
	}

	private void InventoryCheck()
	{
		inventoryfull = true;
		if ((PlayerPrefs.GetFloat("back_to_the_basics") != 1))
		{
			for (int i = 0; i < 5; i++)
			{
				if (item[i] == 0)
					inventoryfull = false;
			}
		} else {
			for (int i = 0; i < 3; i++)
			{
				if (item[i] == 0)
					inventoryfull = false;
			}
		}
	}

	public bool doPlayerHP;

	private void NeilHPCheck()
	{
		if (hpgenerating) 
		{
			neilhptimer += Time.deltaTime;

			if (neilhptimer > 0.05) 
			{
				if (neilhp < hptarget) 
				{
					neilhptimer = 0;
					neilhp += 1;
					audioDevice.PlayOneShot(aud_NeilHPBeep);
				} 
				else
					hpgenerating = false;
					if (!healthup)
						healthup = true;
			}	
		}

		NeilHealthbar.localScale = new Vector3(1,(1-(neilhp/28)),1);
		
		NeilPlayerHealthbar.localScale = new Vector3(1,(1-(neilPlayerHP/28)),1);
	}
	
	private void PassTime()
	{
		if (!gamePaused)
		{
			playTime += Time.deltaTime;

			if (!learningActive && spoopMode)
			{
				time += Time.deltaTime;

				if (eventDelay > 0 && !eventActive)
					eventDelay -= Time.deltaTime;
			}
		}
	}

	bool sloMo = false;

	private void UpdateHUD()
	{
		if (player.stamina < 0f & !warning.activeSelf)
			warning.SetActive(true);
		else if (player.stamina > 0f & warning.activeSelf)
			warning.SetActive(false);
		ammotext.text = GasAmmo + "";
	}

	bool mapOpen = false;

	private void InputCheck()
	{
		if (!learningActive)
		{
			if (input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
					PauseGame();
				else
					UnpauseGame();
			}

			if (!gamePaused && !FindObjectOfType<CoulsonEngine.Game.Dialogue.DialogueManager>().InDialogue)
			{
				if (input.GetButtonDown("Map"))
				{
					if (!mapOpen)
						OpenMap();
					else
						CloseMap();
				}

				if (mapOpen && input.GetButtonDown("Pause"))
				{
					CloseMap();
				}

				if (Input.GetKey(KeyCode.Q))
					player.DropItem(itemSelected);
			}

			if (!gamePaused & Time.timeScale != 1f)
				Time.timeScale = 1f;
			if (RaldiInputManager.current.GetUseDown() && Time.timeScale != 0f)
				UseItem();
			if (RaldiInputManager.current.GetInteractDown() && Time.timeScale != 0f)
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
				else if (Input.GetKeyDown(KeyCode.Alpha4) && !(PlayerPrefs.GetFloat("back_to_the_basics") == 1))
				{
					this.itemSelected = 3;
					UpdateItemSelection();
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5) && !(PlayerPrefs.GetFloat("back_to_the_basics") == 1))
				{
					this.itemSelected = 4;
					UpdateItemSelection();
				}
			}

			// THE SACRED DEBUG
			if (debug)
			{
				if (Input.GetKeyDown(KeyCode.H))
				{
					iPhoneFunction("hitman");
				}
				
				if (Input.GetMouseButtonDown(0) && MetalMode) // Enable / disable metal mode
				{
					if (FindObjectOfType<UnlockedPlayerScript>() != null)
						thing = GameObject.Instantiate<GameObject>(GameControllerScript.current.neilProjectile, FindObjectOfType<UnlockedPlayerScript>().transform.position, spawnPos.rotation).transform;
					else
						thing = GameObject.Instantiate<GameObject>(GameControllerScript.current.neilProjectile, player.transform.position, spawnPos.rotation).transform;
					thing.GetComponentInChildren<NeilProjectileScript>().isMetalBlade = true;
				}
				if (Input.GetKeyDown(KeyCode.I))
				{
					MetalMode = !MetalMode;
				}

				if (Input.GetKeyDown(KeyCode.L)) // Collect all notebooks
				{
					if (notebooks != 10)
						notebooks = 9;
					else
						notebooks = 18;
					if (neilMode)
						FindObjectOfType<NotebookScript>().Collect();
					UpdateNotebookCount();
					/*foreach(EntranceScript e in FindObjectsOfType<EntranceScript>())
					{
						e.Lower();
					}*/
				}
				else if (Input.GetKeyDown(KeyCode.Keypad0)) // Enable neil mode
				{
					FindObjectOfType<PlayerStats>().data.interactedWithNeil = !FindObjectOfType<PlayerStats>().data.interactedWithNeil;
					FindObjectOfType<PlayerStats>().Save();
				}
				else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)) // Infinite Money
				{
					money.money = 9999;
				}
				else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B)) // Mrbeast deltarune
				{
						MrBeastDeltarune();
						ResetItem();
						canPause = false;
				}
				else if (Input.GetKeyDown(KeyCode.G)) // E pearl and crackpipe
				{
					if (!neilMode)
					{
						CollectItem(18);
						CollectItem(25);
					} else {
						GasAmmo = 9999;
					}
				}
				else if (Input.GetKeyDown(KeyCode.U)) // no item reset 
				{
					noreset = !noreset;
				}
				else if (Input.GetKeyDown(KeyCode.T)) // no item reset 
				{
					if (monketime == 10)
						monketime = 60;
					else
						monketime = 10;
				}
				else if (Input.GetKeyDown(KeyCode.P)) // tp to polish
				{
					player.cc.enabled = false;
					polishCow.coolDown = 0f;
					playerTransform.position = new Vector3(polishCow.gameObject.transform.position.x,playerTransform.position.y,polishCow.gameObject.transform.position.z);
					player.cc.enabled = true;
				}
				else if (Input.GetKeyDown(KeyCode.Y)) // Makes mrbeast check his room
				{
					beastCardCollected = true;
					audioDevice.Stop();
				}	
				else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)) // Resets data
				{
					FindObjectOfType<PlayerStats>().ResetToDefaults();

					FindObjectOfType<ObjectasksScreen>().UpdateTasks();
				}
			}
		}
	}
	
	public void GiveRewardItem()
	{
		bool isFull = true;

		foreach(int i in item)
		{
			if (i == 0)
				isFull = false;
		}

		if (!isFull)
        	CollectItem(UnityEngine.Random.Range(1, rewarditems.Length));
	}
	
	public void iPhoneFunction(string function)
	{
		switch (function)
		{
			case "hitman":
				if (mode != "hard")
					UseIphone();
				else
					AddMoney(1);
				break;
			case "ping":
				PingNotebooks();
				break;
			case "exit":
				foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
				{
					if (sources.gameObject.name != "iPhone")
					{
						sources.UnPause();
					}
				}

				phone.SetActive(false);
				Time.timeScale = 1f;
				canPause = true;
				gamePaused = false;
				LockMouse();
				break;
		}

		if (function != "exit")
		{
			ResetItem();
			
			foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
			{
				if (sources.gameObject.name != "iPhone")
				{
					sources.UnPause();
				}
			}

			phone.SetActive(false);
			Time.timeScale = 1f;
			canPause = true;
			gamePaused = false;
			LockMouse();
		}
	}	

	private bool portalFailsafe = false;
	public PostProcessVolume volume2;

	public void OpenPortal()
	{
		IEnumerator OpenPortal()
		{
			if (!portalFailsafe)
			{
				portalFailsafe = true;
				secondaryPP.enabled = true;

				player.walkSpeed = 1;
				player.autoWalk = true;
				player.inNotebook = true;

				audioDevice.PlayOneShot(portalOpen);

				yield return new WaitForSeconds(1);
				tickUpChromatic = true;
				yield return new WaitForSeconds(1);
				tickUpDistortion = true;
				yield return new WaitForSeconds(5);
				tickUpVig = true;
				yield return new WaitForSeconds(4);

				SceneManager.LoadScene("Development");
			}
		}

		StartCoroutine(OpenPortal());
	}
	
	public void PingNotebooks()
	{
		foreach(NotebookScript nb in FindObjectsOfType<NotebookScript>())
		{
			nb.Ping();
		}
	}

	public bool inHitmanCutscene = false;

	public void UseIphone()
	{
		IEnumerator WaitTime()
		{
			audioDevice.PlayOneShot(dialing);
			yield return new WaitForSeconds(dialing.length);
			audioDevice.PlayOneShot(pickUpPhone);
			
			GameObject.Instantiate<GameObject>(cuzsie, spawnPos.position, spawnPos.rotation);
		}

		StartCoroutine(WaitTime());
	}

	public Transform spawnPos;

	public void UpdateNotebookCount()
	{
		if (PlayerPrefs.GetFloat("blind") == 1)
        {
            notebookCount.gameObject.SetActive(false);
        }

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
		if (FindObjectOfType<PlayerStats>().data.objectasks[2] == 0)
			FindObjectOfType<Objectasks>().CollectObjectask(2);
		killedmrBeast = genocide;
		beastCardCollected = false;
		eventActive = false;
		player.cc.enabled = true;
		canPause = true;
		canUseOtherItem = true;
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
				foreach(int itemm in item)
				{
					if (itemm == 14)
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
			this.mouseLocked = true;
			this.reticle.SetActive(true);
		}
	}

	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor();
		this.mouseLocked = false;
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

		FindObjectOfType<BaldiCharacterManager>().OnSpoopMode();
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
			finaleMode = false;
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
		if (!exeption.Contains("walter")) walter.gameObject.SetActive(false);

		FindObjectOfType<BaldiCharacterManager>().RunCharacterSpawnCheck(false);
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
			walter.gameObject.SetActive(true);

		}
		foreach(AudioSource sources in FindObjectsOfType<AudioSource>())
			{
				if (sources.gameObject.name != "Principal of the Thing" && sources.gameObject.name != "SchoolMusic")
				{
					sources.mute = false;
				}
            }

		FindObjectOfType<BaldiCharacterManager>().RunCharacterSpawnCheck(true);
	}

	public AudioClip crackhouseEscapeLoop;
	public Color endingFogColor;

	public void ActivateFinaleMode()
	{
		print("activated");
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
						Animator theAnimator = hotbarAnimator;

                    	if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                        	theAnimator = basicHotbarAnimator;

						theAnimator.SetTrigger("kaboom");
						audioDevice.PlayOneShot(robloxrocketsound);
						for (int i = 0; i < 5; i++)
							{
								if (item[i] == 14 || item[i] == 18 || item[i] == 19) {
									LoseItem(i);
									
									Animator[] theAnimators = explosionAnimators;

									if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
										theAnimators = basicExplosionAnimators;
						
									theAnimators[i].SetTrigger("kaboom");
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
		if (exitsReached < 2 && !(PlayerPrefs.GetFloat("after_dark") == 1))
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
		int maxItems = 4;
		if (PlayerPrefs.GetFloat("back_to_the_basics") == 1) maxItems = 2;

		FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");
		this.itemSelected++;

		if (this.itemSelected > maxItems)
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
		int maxItems = 4;
		if (PlayerPrefs.GetFloat("back_to_the_basics") == 1) maxItems = 2;

		FindObjectOfType<PlayerScript>().playerAnimator.SetTrigger("SelectItem");
		this.itemSelected--;
		if (this.itemSelected < 0)
			this.itemSelected = maxItems;

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
			else if (this.item[3] == 0 && !(PlayerPrefs.GetFloat("back_to_the_basics") == 1))
			{
				this.item[3] = item_ID;
				this.itemSlot[3].sprite = itemSprites[item_ID];

				slot = 3;
			}
			else if (this.item[4] == 0 && !(PlayerPrefs.GetFloat("back_to_the_basics") == 1))
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
			else if (raycastHit3.collider.name == "MethUpgrade" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f && money.money >= methPrice)
			{
				print(methPrice);
				raycastHit3.collider.gameObject.GetComponent<walterMethStationScript>().Purchase();
				methTimer /= 2;
				money.money -= methPrice;
				methPrice += 0.5f;
			}
		}
	}

	public GameObject hammerSound;

	private void UseItem()
	{
		Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit3;

		// Other items
		if (canUseOtherItem || item[itemSelected] == 24)
		{
			if (this.item[this.itemSelected] != 0)
			{
				// Laxitive modifier
				if (PlayerPrefs.GetFloat("laxitive") == 1)
				{
					if (!player.hasToPoop)
						player.poopCooldown -= 25;
					else
						player.poopMultiplier += 1f;
				}

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
					print(PlayerPrefs.GetFloat("destruction") == 1);
					// Hammer sound
					bool madeOne = false;

					Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
					RaycastHit raycastHit2;

					if (Physics.Raycast(ray2, out raycastHit2) && Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f && !madeOne)
					{
						madeOne = true;	
						GameObject.Instantiate<GameObject>(hammerSound, raycastHit2.transform.position, raycastHit2.transform.rotation);
					}
					
					// 1st Prize
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

					// Vanman
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
							if (PlayerPrefs.GetFloat("destruction") == 1)
								player.ResetGuilt("hammer", 2f);
							print(player.guilt);
							print(player.guiltType);
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
							if (PlayerPrefs.GetFloat("destruction") == 1)
								player.ResetGuilt("hammer", 2f);
							AddMoney(2f);
							raycastHit3.transform.gameObject.GetComponent<MeshRenderer>().material = brokenVault;
							moneySpray.SetActive(true);
							
							audioDevice.PlayOneShot(windowBreak);
							baldi.Hear(player.transform.position, 2f);
							ResetItem();
						}	
						if (raycastHit3.collider.tag == "BladderHammerable" & Vector3.Distance(this.playerTransform.position, raycastHit3.transform.position) <= 10f)
						{
							if (PlayerPrefs.GetFloat("destruction") == 1)
								player.ResetGuilt("hammer", 2f);
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
							float modifier = 1;
							
							if (PlayerPrefs.GetFloat("make_this_quick") == 1) modifier *= 2;

							if (PlayerPrefs.GetFloat("my_roots") == 1) modifier *= 0.5f;
							yield return new WaitForSeconds(monketime / modifier);
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
				if ((neilMode || PlayerPrefs.GetFloat("wrong_mode") == 1) && GasAmmo > 0)
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
		if (PlayerPrefs.GetFloat("back_to_the_basics") == 1 && id > 2)
			return;
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
}
	