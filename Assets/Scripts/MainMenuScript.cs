using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public static MainMenuScript current;
    public Animator introAnim;
    public AudioSource source;
    public Image logo;
    public Sprite peterLogo;
    public Sprite normLogo;
    public AudioSource music;
    public AudioSource peterMusic;
    public GameObject peterFlash;
    public Image[] menuChars;
    private Sprite[] originalChars;
    public Sprite peterr;
    public AudioClip vineBoom;
    public AudioClip ishaanMenu;
    public MenuPeter peter;
    public bool neilmode = false;
    public Sprite neilLogo;
    public GameObject neil;
    public Image cuzsielogo;
    public Image cuzsielogobg;
    public GameObject[] shitThatShouldDeactivate;
    public AudioClip neilTitleScreen;
    public AudioClip repriseMenu;
    public Camera maincamera;
    public Slider FOVSlider;
    public bool fadeOut = false;
    public GameObject ishaanMode;
    public GameObject hardRecordText;
    public GameObject pussyOut;
    private bool creditsOpen = false;

    public Button[] hardModeButton;
    public TMPro.TextMeshProUGUI[] hardModeDesc;

    public void ToggleCredits(bool on) { creditsOpen = on; }

    public void ActivatePeter()
    {
        logo.sprite = peterLogo;
        music.volume = 0;
        peterMusic.volume = 1;
        peterMusic.time = music.time;
        peterMusic.PlayOneShot(vineBoom);
        FindObjectOfType<GameJoltAvatar>().peterify(peterr);
        peterFlash.SetActive(true);

        originalChars = new Sprite[menuChars.Length];

        int index = 0;

        foreach(Image charr in menuChars)
        {
            originalChars[index] = charr.sprite;
            charr.sprite = peterr;

            index++;
        }
    }

    public void EnableMusicFade()
    {
        fadeOut = true;
    }

    public void DeactivatePeter()
    {
        logo.sprite = normLogo;
        music.volume = 1;
        peterMusic.volume = 0;

        peterFlash.SetActive(false);

        int index = 0;

        foreach(Image charr in menuChars)
        {
            charr.sprite = originalChars[index];
            index++;
        }

        peter.clickTimes = 0;
        peter.canClick = true;
    }

    public Sprite ishaan;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        PlayerPrefs.SetFloat("NeilScarySave", 0);
        PlayerPrefs.SetFloat("PreventNewFiles", 0);

        FindObjectOfType<PlayerStats>().TryLoad();

        if (PlayerPrefs.GetFloat("FOV") == 0) PlayerPrefs.SetFloat("FOV", 60);
        else FOVSlider.value = PlayerPrefs.GetFloat("FOV");

        if (!FindObjectOfType<PlayerStats>().data.storyModeWon)
        {
            foreach (Button button in hardModeButton) // Multiple button support because yes
            {
                button.interactable = false;
                button.GetComponent<Image>().color = Color.black;
            }
            foreach (TMPro.TextMeshProUGUI text in hardModeDesc)
            {
                text.text = "Unlocked by beating Story Mode!";
            }
           
            hardRecordText.SetActive(false);
        }
        
        introAnim.SetTrigger("PlayIntro");

        if (FindObjectOfType<PlayerStats>().data.ishaanMenu)
        {
            music.clip = ishaanMenu;
            music.Play();

            maincamera.clearFlags = CameraClearFlags.SolidColor;
            RenderSettings.ambientLight = Color.gray;

            foreach (GameObject item in shitThatShouldDeactivate)
            {
              item.SetActive(false);  
            }
        }

        if (FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
            ishaanMode.SetActive(true); 
        
        neil.SetActive(false);
        neilmode = FindObjectOfType<PlayerStats>().data.interactedWithNeil;

        if (neilmode) 
        {
            maincamera.clearFlags = CameraClearFlags.SolidColor;
            cuzsielogo.sprite = neilLogo;
            cuzsielogobg.color = new Color(0f,0f,0f,1);
            foreach (GameObject item in shitThatShouldDeactivate)
            {
              item.SetActive(false);  
            }
            neil.SetActive(true);
            music.clip = neilTitleScreen;
            music.Play();

            RenderSettings.ambientLight = Color.gray;

            pussyOut.SetActive(true);

            foreach(Modifier bad in ModifierMenu.modifiers)
            {
                PlayerPrefs.SetFloat(ModifierMenu.ConvertToInternal(bad.name), 0);
            }
        }

        if (PlayerPrefs.GetFloat("RepriseMenu") == 1)
        {
            PlayerPrefs.SetFloat("RepriseMenu", 0);
            music.clip = repriseMenu;
            music.Play();
        }
    }

    public AudioSource creditsTheme;

    void Update()
    {
        PlayerPrefs.SetFloat("FOV", FOVSlider.value); // PART OF FIX III
        if (Input.GetKeyDown(KeyCode.Escape) && introAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToLower() == "introanimation")
        {
            source.time = introAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.5f;

            introAnim.Play("IntroFinish");
        }

        if (fadeOut && music.volume > 0)
            music.volume -= Time.deltaTime * 2;
        else if (!fadeOut)
        {
            if (!creditsOpen)
            {
                if (music.volume < 1f)
                    music.volume += Time.deltaTime / 2;
                if (creditsTheme.volume > 0f)
                    creditsTheme.volume -= Time.deltaTime / 2;
            }
            else
            {
                if (creditsTheme.volume < 1f)
                    creditsTheme.volume += Time.deltaTime / 2;
                if (music.volume > 0f)
                    music.volume -= Time.deltaTime / 2;
            }
        }
        
    }

    public void AchievementCheck()
    {
        if (neilmode)
            GameJolt.API.Trophies.TryUnlock(184394);
    }
}
