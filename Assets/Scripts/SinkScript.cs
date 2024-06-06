using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using TMPro;

using Rewired;

public class SinkScript : MonoBehaviour
{
    private AudioSource audioSource;

    public Slider timeSlider;
    public TextMeshProUGUI timeLeftText;

    private bool countDown = false;
    private float timeLeft;

    private Player input;

    void Start()
    {
        audioSource = base.GetComponent<AudioSource>();

        input = ReInput.players.GetPlayer(0);
    }

    void Update()
    {        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < GameControllerScript.current.player.interactDistance && (Input.GetKeyDown(KeyCode.Mouse0)) && !GameControllerScript.current.player.washedHands && !countDown)
        {
            GameControllerScript.current.player.inNotebook = true;
            countDown = true;

            timeSlider.gameObject.SetActive(true);
            timeSlider.minValue = 0;
            timeSlider.maxValue = 5;

            audioSource.Play();
            audioSource.time = 0;

            FindObjectOfType<CameraScript>().preventBackLooking = true;
        }

        if (countDown && timeLeft < 5f)
        {
            timeLeft += Time.deltaTime;
            timeSlider.value = timeLeft;
            timeLeftText.text = "Washing Hands... " + Mathf.RoundToInt(timeLeft).ToString() + "/5";
            if (input.GetButtonDown("Jump")) {
                timeLeft += 0.1f;

                audioSource.time += 0.1f;
            }
        }
        else if (countDown && timeLeft > 5f)
        {
            timeSlider.gameObject.SetActive(false);
            audioSource.Stop();
            countDown = false;
            timeLeft = 0f;
            GameControllerScript.current.player.inNotebook = false;
            GameControllerScript.current.player.justPooped = false;
            GameControllerScript.current.player.washedHands = true;
            FindObjectOfType<CameraScript>().preventBackLooking = false;
        }
    }
}
