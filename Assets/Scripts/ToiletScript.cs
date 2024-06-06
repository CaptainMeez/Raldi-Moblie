using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

using TMPro;

using Rewired;

public class ToiletScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public Transform player;
    public float interactDistance;
    AudioSource audioSource;
    public AudioClip poopSound;
    public AudioClip victory;

    public UnityEvent onFlush;

    private bool broken = false;

    public bool goldenToilet = false;

    public GameObject poopingVCam;


    // GOLDEN TOILET STUFF
    private Slider timeSlider;
    private TextMeshProUGUI timeLeftText;
    private bool countDown = false;
    private float timeLeft;

    private Player input;

    void Start()
    {
        input = ReInput.players.GetPlayer(0);

        audioSource = base.GetComponent<AudioSource>();

        timeSlider = FindObjectOfType<SinkScript>().timeSlider;
        timeLeftText = FindObjectOfType<SinkScript>().timeLeftText;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (!goldenToilet)
        {
            if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(player.position, base.transform.position) < interactDistance && RaldiInputManager.current.GetInteractDown() && (playerScript.hasToPoop || playerScript.poopCooldown < 30f))
            {
                Shit();
            }
        }
        else
        {
            if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < GameControllerScript.current.player.interactDistance && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && (playerScript.hasToPoop || playerScript.poopCooldown < 30f) && !countDown)
            {
                GameControllerScript.current.player.inNotebook = true;
                countDown = true;

                timeSlider.gameObject.SetActive(true);
                timeSlider.minValue = 0;
                timeSlider.maxValue = 5;

                FindObjectOfType<CameraScript>().preventBackLooking = true;
            }

            if (countDown && timeLeft < 5f)
            {
                timeLeft += Time.deltaTime;
                timeSlider.value = timeLeft;
                timeLeftText.text = "Taking a Poop... " + Mathf.RoundToInt(timeLeft).ToString() + "/5";

                if (input.GetButtonDown("Jump")) 
                {
                    if (!(PlayerPrefs.GetFloat("DisableFarts") == 2))
                        audioSource.PlayOneShot(playerScript.farts[UnityEngine.Random.Range(0, playerScript.farts.Length)]);
                        
                    timeLeft += 0.1f;
                }
            }
            else if (countDown && timeLeft > 5f)
            {
                timeSlider.gameObject.SetActive(false);
                countDown = false;
                timeLeft = 0f;
                //GameControllerScript.current.player.inNotebook = false;
                poopingVCam.SetActive(false);
                playerScript.playerVCam.gameObject.SetActive(true);
                Shit();
                FindObjectOfType<CameraScript>().preventBackLooking = false;
            }
        }
    }

    public void Shit()
    {
        float cooldown = 150f;
        if (goldenToilet) cooldown = 200f;

        playerScript.hasToPoop = false;
        playerScript.poopCooldown = cooldown;
        playerScript.poopMultiplier = 1f;
        playerScript.justPooped = true;
        playerScript.washedHands = false;

        if (!(PlayerPrefs.GetFloat("DisableFarts") == 2))
            audioSource.PlayOneShot(poopSound);
            
        audioSource.PlayOneShot(victory);
        
        if (!(PlayerPrefs.GetFloat("blind") == 1))
        {
            foreach(GameObject highlights in GameControllerScript.current.bathroomHighlights)
            {
                highlights.SetActive(false);
            }
        }

        PlayerPrefs.SetInt("PoopTimes", PlayerPrefs.GetInt("PoopTimes") + 1);

        if (PlayerPrefs.GetInt("PoopTimes") == 25)
            GameJolt.API.Trophies.TryUnlock(184395);

        onFlush.Invoke();
    }
}
