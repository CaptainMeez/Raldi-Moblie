using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObjectasksScreen : MonoBehaviour
{
    public GameObject pcamera;
    public Image stageselectimage;
    public CoulsonEngine.Game.Dialogue.FileDialogueTrigger dialogue;
    public GameObject selectbox;
    public GameObject neilpfp;
    public AudioSource music;
    public AudioClip doTask;
    public AudioClip rssStart;
    public AudioSource beepsource;
    public TextMeshProUGUI text;
    public GameObject[] boxes = new GameObject[6];
    public Image funnyImage;
    public Sprite finishedAll;
    public bool inmenu = false;
    public bool neilmode = false;
    public bool allones = true;
    private bool initiated;
    private float wait;
    private float selectboxblinktime;
    private bool selectboxactivated = true;
    private bool wonTheGame = false;
    private Vector2 selected = new Vector2(0,0);
    public string[] descriptions = new string[9];
    public string[] unlockPrefs = {"vanman", "chipfloke", "mrbeast", "prize", "raldi", "crafters", "bloke", "pintobeans"};
    private string[] CompletedunlockPrefs = {"van man", "chipfloke", "mr.beast", "christian first prize", "neil", "raldi crack", "arts and crafters", "british bloke", "pintobeans"};    
    public bool[] unlocks = new bool[8];
    

    #if UNITY_EDITOR
    public KeyCode[] unlockDebug = 
    {
        KeyCode.Keypad7, 
        KeyCode.Keypad8, 
        KeyCode.Keypad9, 
        KeyCode.Keypad4, 
        KeyCode.Keypad6, 
        KeyCode.Keypad1, 
        KeyCode.Keypad2, 
        KeyCode.Keypad3, 
    };
    #endif

    bool opened = false;

    private void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        if (FindObjectOfType<PlayerStats>().data.theEnd)
        {
            wonTheGame = true;
        } 

        UpdateTasks();
    }

    public AudioClip sadge;

    public void UpdateTasks()
    {
        allones = true;

        for(int i = 0; i < unlockPrefs.Length; i++)
        {
            float value = FindObjectOfType<PlayerStats>().data.objectasks[i];

            unlocks[i] = value == 1;

            if (value != 1) {
                allones = false;
            }
        }

        if (wonTheGame) allones = true;

        if (allones && !FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked) {
            if (!wonTheGame)
            {
                neilmode = true;
                neilpfp.SetActive(true);
                grayopacitything.SetActive(true);
                music.pitch = 0.25f;
            } else {
                neilmode = false;
                grayopacitything.SetActive(true);
                
                funnyImage.sprite = finishedAll;
                music.volume = 0;
            }
        } else {
            neilmode = false;
            neilpfp.SetActive(false);
            grayopacitything.SetActive(false);
            music.pitch = 1f;
            if (FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
                funnyImage.sprite = finishedAll;
        }

        for(int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetActive(unlocks[i]);
        }
    }

    public void Open()
    {
        if (!opened && canOpenAgain)
        {
            print("piss");
            
            if (wonTheGame && !FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
            {
                print("piss 2");
                
                dialogue.TriggerDialogue(1);

                allowNormalExit = false;

                IEnumerator Dumbness() {yield return new WaitForSeconds(1f); music.clip = sadge; music.volume = 1f;}
                StartCoroutine(Dumbness());
            }  

            FindObjectOfType<PlayerStats>().data.foundObjectasks = true;
            FindObjectOfType<PlayerStats>().Save();

            text.text = "Escape to Quit, Arrows to move cursor,\nEnter to view information on how to beat.";
            wait = 0;
            initiated = false;
            inmenu = true;
            GameControllerScript.current.HideChar();
            GameControllerScript.current.hud.SetActive(false);
            music.PlayOneShot(rssStart);
            opened = true;
            GameControllerScript.current.UnlockMouse();
            GameControllerScript.current.canPause = false;
            GameControllerScript.current.player.playerVCam.gameObject.SetActive(false);
            pcamera.SetActive(true);

            GameControllerScript.current.allowEvents = false;
            GameControllerScript.current.StopAllEvents();  
            GameControllerScript.current.player.inNotebook = true;

            GameJolt.API.Trophies.TryUnlock(182825);   
        }
    }

    public AudioClip destroy;
    public AudioClip events;
    public AudioClip selectedboss;
    public AudioClip stagestarttheme;
    public AudioClip brokenSong;
    public GameObject eventDisp;
    public GameObject grayopacitything;
    public TextMeshProUGUI stageselectedtext;
    bool canOpenAgain = true;
    public Color darkness;
    public GameObject bladder;
    public GameObject dog;
    public GameObject thatOneSpecificWallBehindThePhone;
    public GameObject mysteryRoom;
    public GameObject payPhone;
    private bool allowNormalExit = true;
    public BoxCollider[] gameBreakingBalls;

    public void NeilClick()
    {
        IEnumerator StartNeil()
        {
            FindObjectOfType<LockDoorIfNeil>().ForceLockDoor();
            
            RenderSettings.skybox = GameControllerScript.current.blackSky;
            
            music.Stop();
            music.pitch = 1;
            canOpenAgain = false;
            base.GetComponent<Animator>().SetTrigger("Selected");
            grayopacitything.SetActive(false);
            music.PlayOneShot(selectedboss);
            yield return new WaitForSeconds(1f);
            music.PlayOneShot(stagestarttheme);
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < 8; i++)
            {
                stageselectedtext.text = (stageselectedtext.text + "?");
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(3.7f);
            base.GetComponent<Animator>().SetTrigger("Break");
            thatOneSpecificWallBehindThePhone.SetActive(false);
            mysteryRoom.SetActive(true);
            payPhone.SetActive(false);
            RenderSettings.ambientLight = darkness;
            foreach (SpriteRenderer sprite in FindObjectsOfType<SpriteRenderer>())
            {
                sprite.color = darkness;
            }
            foreach (BoxCollider poster in gameBreakingBalls)
            {
                poster.isTrigger = false;
            }
            GameControllerScript.current.HideChar();
            GameControllerScript.current.player.playerCanPoop = false;
            GameControllerScript.current.player.poopCooldown = 1000f;
            GameControllerScript.current.player.hasToPoop = false;
            GameControllerScript.current.allowEvents = false;
            music.PlayOneShot(destroy);
            bladder.SetActive(false);
            dog.SetActive(false);

            foreach(NotebookScript notebook in FindObjectsOfType<NotebookScript>())
            {
                notebook.gameObject.SetActive(false);
            }

            foreach(PickupScript notebook in FindObjectsOfType<PickupScript>())
            {
                notebook.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(2.1f);
            Close();
            yield return new WaitForSeconds(2f);
            music.PlayOneShot(events);
            yield return new WaitForSeconds(2f);
            GameControllerScript.current.schoolMusic.clip = brokenSong;
            GameControllerScript.current.schoolMusic.Play();
            eventDisp.SetActive(true);
            yield return new WaitForSeconds(4f);
            eventDisp.SetActive(false);
        }

        StartCoroutine(StartNeil());
    }

    public void Close()
    {
        text.text = "";
        inmenu = false;
        if (!neilmode)
            beepsource.Play();
        selected = new Vector2(0,0);

        if (canOpenAgain)
        {
            GameControllerScript.current.ShowChar();
            music.Stop();
            GameControllerScript.current.allowEvents = true;
        }
            
        GameControllerScript.current.hud.SetActive(true);
        opened = false;
        GameControllerScript.current.LockMouse();
        IEnumerator AllowPausing() {yield return null; GameControllerScript.current.canPause = true;}
        StartCoroutine(AllowPausing());
        GameControllerScript.current.player.playerVCam.gameObject.SetActive(true);
        pcamera.SetActive(false);

        GameControllerScript.current.player.inNotebook = false;
    }

    bool canActivateNeilClick = true;

    public void StaticMoment()
    {
        base.GetComponent<Animator>().SetTrigger("Break");
        FindObjectOfType<PlayerStats>().data.canTalkIshaan = true;
        FindObjectOfType<PlayerStats>().Save();
        GameControllerScript.current.ShowChar();
        canOpenAgain = false;
        Close();
    }

    private void Update()
    {
        if (inmenu) {
            if (Input.GetKeyDown(KeyCode.Return) && (!wonTheGame || FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)) {
                if (!neilmode)
                {
                    beepsource.Play();

                    if (!wonTheGame)
                        text.text = descriptions[Mathf.RoundToInt((selected.x + 1) + 3*(Mathf.Abs(2 - (selected.y + 1))))];
                    else
                        text.text = CompletedunlockPrefs[Mathf.RoundToInt((selected.x + 1) + 3*(Mathf.Abs(2 - (selected.y + 1))))].ToUpper() + " has been completed.";
                }
                else if (canActivateNeilClick && !wonTheGame)
                {
                    GameControllerScript.current.minimapstatic.SetActive(true);
                    canActivateNeilClick = false;
                    NeilClick();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (!neilmode)
                {
                    selected += new Vector2(-1,0);
                    selectboxactivated = true;
                    selectbox.SetActive(true);
                    selectboxblinktime = 0;
                    text.text = "Escape to Quit, Arrows to move cursor,\nEnter to view information on how to beat.";
                }
                beepsource.Play();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (!neilmode)
                {
                    selected += new Vector2(1,0);
                    selectboxactivated = true;
                    selectbox.SetActive(true);
                    selectboxblinktime = 0;
                    text.text = "Escape to Quit, Arrows to move cursor,\nEnter to view information on how to beat.";
                }
                beepsource.Play();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (!neilmode)
                {
                selected += new Vector2(0,1);
                selectboxactivated = true;
                selectbox.SetActive(true);
                selectboxblinktime = 0;
                text.text = "Escape to Quit, Arrows to move cursor,\nEnter to view information on how to beat.";
                }
                beepsource.Play();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (!neilmode)
                {
                selected += new Vector2(0,-1);
                selectboxactivated = true;
                selectbox.SetActive(true);
                selectboxblinktime = 0;
                text.text = "Escape to Quit, Arrows to move cursor,\nEnter to view information on how to beat.";
                }
                beepsource.Play();
            }

            if (selected.x < -1) {
                selected = new Vector2(1,selected.y);
            } else if (selected.x > 1) {
                selected = new Vector2(-1,selected.y);
            }
            if (selected.y < -1) {
                selected = new Vector2(selected.x,1);
            } else if (selected.y > 1) {
                selected = new Vector2(selected.x,-1);
            }
        }
        selectbox.transform.localPosition = new Vector3((selected.x*3.13f),(selected.y*2.5f) + 0.628f, 0);

        if (Input.GetKeyDown(KeyCode.Escape) && opened && !neilmode && allowNormalExit)
        {
            Close();
        }
        if (!neilmode) {
            if (wait >= (rssStart.length) && !initiated) {
                if (canActivateNeilClick)
                music.Play();
                initiated = true;
            } else if (inmenu) {
                wait += Time.deltaTime;
            }
        } else {
            if (wait >= (rssStart.length) && !initiated) {
                if (canActivateNeilClick)
                music.Play();
                initiated = true;
            } else if (inmenu) {
                wait += Time.deltaTime * 0.25f;
            }
        }
        selectboxblinktime += Time.deltaTime;
        if (selectboxblinktime > 0.2f) {
            selectboxactivated = !selectboxactivated;
            selectbox.SetActive(selectboxactivated);
            selectboxblinktime = 0;
        } 
    }
}
