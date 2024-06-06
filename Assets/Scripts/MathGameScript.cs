using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Video;

using TMPro;

using Rewired;

public class MathGameScript : MonoBehaviour
{   
    public AudioSource vocals;

    private Player input;
    private bool noteLock = false;
    private void Start()
    {
        input = ReInput.players.GetPlayer(0);

        Time.timeScale = 0f;
        psc.inNotebook = true;

        if (gc.notebooks == 1)
        {
            Time.timeScale = 1f;

            video.SetActive(false);

            vocals.Play();

            IEnumerator Meme()
            {
                StartCoroutine(WaitTime());

                gc.ActivateLearningGame();
                gc.schoolMusic.Stop();
                gc.learnMusic.Play();
                
                answerText.text = "Wenomechainsama";
                yield return new WaitForSeconds(3.5f);

                answerText.text = "Tumajarbisaun";
                yield return new WaitForSeconds(4f);

                answerText.text = "Wefenlooof";
                yield return new WaitForSeconds(3.5f);

                answerText.text = "Eselifterbraun";
            }

            IEnumerator WaitTime() 
            {
                yield return new WaitForSeconds(funnySong.length);

                ExitBook();
            }

            StartCoroutine(Meme());
        }
        else
        {
            gc.ActivateLearningGame();
            gc.learnMusic.Stop();

            if (UnityEngine.Random.Range(0, 100) == 50)
                realVid.clip = secret;
            else
                realVid.clip = videos[UnityEngine.Random.Range(0, videos.Length)];
            
            gc.schoolMusic.Stop();

            realVid.loopPointReached += OnVideoEnd;
            Time.timeScale = 0f;
        }
    }

    private void Update()
    {
        if (input.GetButton("Skip") || input.GetButton("Pause") /*|| Gamepad.current.bButton.wasPressedThisFrame*/ & !noteLock)
        {
            noteLock = true;
            ExitBook();
        }
    }

    public void OnVideoEnd(VideoPlayer v)
    {
        ExitBook();
    }

    public void ExitBook()
    {
        if (GameControllerScript.current.notebooks == GameControllerScript.current.notebooksToCollect & (GameControllerScript.current.mode.Contains("story") || GameControllerScript.current.mode == "hard"))
			GameControllerScript.current.ActivateFinaleMode();

        if (GameControllerScript.current.trueReset)
        {
            GameControllerScript.current.trueReset = false;
            RenderSettings.fog = false;
            GameControllerScript.current.ShowChar();
        }

        if (questionText.text == "/tp @s 0 0 0")
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync("MCEEMenu");
        }
        else
        {
            Time.timeScale = 1f;

            IEnumerator Exit()
            {
                float modifier = 1 + PlayerPrefs.GetFloat("hot_headed");

                psc.inNotebook = false;
                problemsWrong++;

                if (!baldiScript.endless)
                    baldiScript.GetAngry(1f * modifier);
                else 
                    baldiScript.baldiAnger -= 0.25f;

                if (gc.spoopMode) gc.ShowChar();
                if (gc.notebooks == 2) gc.ActivateSpoopMode();

                yield return null;

                gc.eventDelay = UnityEngine.Random.Range(3,10);

                this.gc.DeactivateLearningGame(base.gameObject);
            }
                
            StartCoroutine(Exit());
        }
    }

    public GameControllerScript gc;

    public GameObject blackScreen;
    public AudioClip NEIL_Game;
    public BaldiScript baldiScript;
    public PlayerScript psc;
    public VideoClip[] videos;
    public VideoClip secret;
    public AudioClip funnySong;
    public GameObject video;
    public VideoPlayer realVid;

    public TMP_InputField questionText;
    public TextMeshProUGUI answerText;

    private int problemsWrong;
 }
