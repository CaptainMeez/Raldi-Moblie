    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CoulsonEngine.Game.Dialogue;

public class IshaanScript : MonoBehaviour
{
    public GameObject ishaan;
    public GameObject iCamera;
    public GameObject hamood;
    public AudioSource music;
    public AudioClip sans;
    public AudioClip papyrus;
    public AudioClip appear;
    public FileDialogueTrigger d_ishaan1;
    public AudioClip staticc;
    public LayerMask cutsceneMask;
    private LayerMask ogMask;
    public Camera mainCamera;
    private bool allowNormalShop = true;
    public GameObject ishaanShop;

    IEnumerator Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        yield return null;
        
        bool shouldEnable = false;

        if (FindObjectOfType<PlayerStats>().data.theEnd)
        {
            if (!FindObjectOfType<GameControllerScript>().neilMode && !FindObjectOfType<GameControllerScript>().ishaanMode && FindObjectOfType<GameControllerScript>().mode != "hard")
                shouldEnable = true;
        }

        if (shouldEnable) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }

    public void CheckForTalk()
    {
        if (FindObjectOfType<PlayerStats>().data.canTalkIshaan && !FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
        {
            IshaanCutscene();
            allowNormalShop = false;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < 15 && RaldiInputManager.current.GetInteractDown())
        {
            if (allowNormalShop && ishaan.activeInHierarchy)
            {
                ishaanShop.SetActive(true);   
            }
        }
    }

    public void IshaanCutscene() // Ishaan mode unlock
    {
        if (!FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
        {
            ogMask = mainCamera.cullingMask;
            mainCamera.cullingMask = cutsceneMask;

            GameControllerScript.current.player.playerVCam.gameObject.SetActive(false);
            GameControllerScript.current.player.inNotebook = true;
            GameControllerScript.current.allowEvents = false;
            GameControllerScript.current.StopAllEvents();
            GameControllerScript.current.HideChar();
            iCamera.SetActive(true);
            d_ishaan1.onComplete.AddListener(SpawnHamood);
            d_ishaan1.TriggerDialogue(1);
            music.clip = sans;
            music.Play();
        }
    }

    bool spawned = false;
    bool theFinal = false;

    public void SpawnHamood()
    {
        music.Stop();

        if (!theFinal)
        {
            if (!spawned)
            {
                IEnumerator Spawn()
                {
                    spawned = true;
                    yield return new WaitForSeconds(1f);
                    music.PlayOneShot(appear);
                    hamood.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    d_ishaan1.onComplete.AddListener(SpawnHamood);
                    d_ishaan1.dialogueFile = "IshaanPNMeet2";
                    d_ishaan1.TriggerDialogue();
                    music.clip = papyrus;
                    music.Play();
                }

                StartCoroutine(Spawn());
            }
            else
            {
                IEnumerator Spawn()
                {
                    theFinal = true;
                    yield return new WaitForSeconds(1f);
                    music.PlayOneShot(appear);
                    hamood.SetActive(false);
                    yield return new WaitForSeconds(1.5f);
                    d_ishaan1.onComplete.AddListener(SpawnHamood);
                    d_ishaan1.dialogueFile = "IshaanPNMeet3";
                    d_ishaan1.TriggerDialogue();
                }

                StartCoroutine(Spawn());
            }
        }
        else
        {
            IEnumerator Spawn()
            {
                GameControllerScript.current.canPause = false;
                ishaan.GetComponent<Animator>().Play("IshaanDie");

                yield return new WaitForSeconds(4);

                FindObjectOfType<ObjectasksScreen>().thatOneSpecificWallBehindThePhone.SetActive(false);
                FindObjectOfType<ObjectasksScreen>().mysteryRoom.SetActive(true);
                FindObjectOfType<ObjectasksScreen>().payPhone.SetActive(false);

                foreach(NotebookScript notebook in FindObjectsOfType<NotebookScript>())
                {
                    notebook.gameObject.SetActive(false);
                }

                foreach(PickupScript notebook in FindObjectsOfType<PickupScript>())
                {
                    notebook.gameObject.SetActive(false);
                }

                ishaan.SetActive(false);
                music.PlayOneShot(staticc); 

                RenderSettings.ambientLight = FindObjectOfType<ObjectasksScreen>().darkness;
                RenderSettings.skybox = GameControllerScript.current.blackSky;
                foreach (SpriteRenderer sprite in FindObjectsOfType<SpriteRenderer>())
                {
                    sprite.color = FindObjectOfType<ObjectasksScreen>().darkness;
                }
                FindObjectOfType<LockDoorIfNeil>().ForceLockDoor();
                GameControllerScript.current.HideChar();
                GameControllerScript.current.player.playerCanPoop = false;
                GameControllerScript.current.player.poopCooldown = 1000f;
                GameControllerScript.current.player.hasToPoop = false;
                GameControllerScript.current.allowEvents = false;

                yield return new WaitForSeconds(0.5f);
                GameControllerScript.current.player.playerVCam.gameObject.SetActive(true);
                GameControllerScript.current.player.inNotebook = false;
                mainCamera.cullingMask = ogMask;
                iCamera.SetActive(false);
                GameControllerScript.current.ishaanKeys = true;
                yield return new WaitForSeconds(2f);
                music.PlayOneShot(FindObjectOfType<ObjectasksScreen>().events);
                yield return new WaitForSeconds(2f);
                GameControllerScript.current.schoolMusic.clip = FindObjectOfType<ObjectasksScreen>().brokenSong;
                GameControllerScript.current.schoolMusic.Play();
                FindObjectOfType<ObjectasksScreen>().eventDisp.SetActive(true);
                yield return new WaitForSeconds(4f);
                FindObjectOfType<ObjectasksScreen>().eventDisp.SetActive(false);
            }

            StartCoroutine(Spawn());

           
        }
    }
}
