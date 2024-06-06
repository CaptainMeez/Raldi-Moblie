using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveFirstRoomTrigger : MonoBehaviour
{
    public GameObject raldiStyle;
    public GameObject raldiTutor;
    public bool autoactivateEasterEgg = false;
    public AudioClip secret;
    public GameObject enviornment;
    public GameObject funnyObjectsAround;
    public GameObject animationVCam;
    public Material pinkSky;
    public GameObject reticles;
    public GameObject minigame;
    public Material defaultSkybox;
    public AudioClip liscensedGangam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameControllerScript.current.baldiStyle && !GameControllerScript.current.mode2016 && GameControllerScript.current.notebooks == 1 && (GameControllerScript.current.mode.Contains("story") || GameControllerScript.current.mode.Contains("endless")) && GameControllerScript.current.mode != "story_cow" && !GameControllerScript.current.neilMode)
        {
            GameControllerScript.current.baldiStyle = true; 
            raldiStyle.SetActive(true);
            raldiTutor.SetActive(false);

            if (PlayerPrefs.GetFloat("StreamerMode") == 2)
            {
                raldiStyle.GetComponent<AudioSource>().clip = liscensedGangam;
                raldiStyle.GetComponent<AudioSource>().Play();
            }

            if (autoactivateEasterEgg)
                raldiStyle.GetComponent<AudioSource>().clip = secret;

            raldiStyle.GetComponent<AudioSource>().Play();
            GameControllerScript.current.schoolMusic.Stop();

            if (autoactivateEasterEgg)
            {
                GameControllerScript.current.musicBeat.BPM = 130;
                GameControllerScript.current.musicBeat.syncBPM();
                GameControllerScript.current.musicBeat.beatHit.AddListener(GameControllerScript.current.BeatHit);
                GameControllerScript.current.quarter.SetActive(false);
                raldiStyle.GetComponent<AudioSource>().spatialBlend = 0;

                foreach(DoorScript door in FindObjectsOfType<DoorScript>())
                {
                    if (!door.gameObject.name.ToLower().Contains("jail"))
                        door.LockDoor(9999);
                }

                IEnumerator FunnyStuff()
                {
                    yield return new WaitForSeconds(14.77f);
                    RenderSettings.skybox = pinkSky;
                    enviornment.SetActive(false);
                    reticles.SetActive(false);
                    funnyObjectsAround.SetActive(true);
                    GameControllerScript.current.hud.SetActive(false);
                    GameControllerScript.current.player.playerVCam.gameObject.SetActive(false);
                    GameControllerScript.current.player.inNotebook = true;
                    animationVCam.SetActive(true);
                    yield return new WaitForSeconds(29.545f);
                    
                    GameObject miniobj = GameObject.Instantiate<GameObject>(minigame, new Vector3(0, 1000, 0), minigame.transform.rotation);
                    RenderSettings.fog = true;
                    RenderSettings.fogColor = Color.black;
                    RenderSettings.ambientLight = GameControllerScript.current.neilColor;

                    yield return new WaitForSeconds(36.9240f);
                    GameObject.Destroy(miniobj);
                    RenderSettings.fog = false;
                    RenderSettings.ambientLight = Color.white;
                    RenderSettings.skybox = defaultSkybox;
                    enviornment.SetActive(true);
                    reticles.SetActive(true);
                    funnyObjectsAround.SetActive(false);
                    GameControllerScript.current.hud.SetActive(true);
                    GameControllerScript.current.player.playerVCam.gameObject.SetActive(true);
                    GameControllerScript.current.player.inNotebook = false;
                    animationVCam.SetActive(false);
                    raldiStyle.GetComponent<AudioSource>().loop = false;
                    GameControllerScript.current.schoolMusic.Play();
                    GameControllerScript.current.musicBeat.beatHit.RemoveAllListeners();
                    
                    foreach(DoorScript door in FindObjectsOfType<DoorScript>())
                    {
                        door.UnlockDoor();
                    }
                }

                StartCoroutine(FunnyStuff());
            }
        }
    }
}
