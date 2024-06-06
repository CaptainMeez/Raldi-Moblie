using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Rewired;

public class QuestionGame : MonoBehaviour
{
    private Player input;

    private float timer;
    private bool doTimer = true;

    public int question = 0;
    public AudioSource player;
    public AudioClip ding;
    public AudioClip vineThud;
    public AudioClip youStupid;
    public AudioClip screech;
    public GameObject loseScreen;
    public AudioClip winSound;
    bool canAnswer = true;

    public TMPro.TextMeshProUGUI timerText;
    public Animator timerAnim;

    public GameObject[] buttons;
    public Text[] texts;

    public AudioSource music;

    public TMPro.TextMeshProUGUI questionText;

    public void Start()
    {
        input = ReInput.players.GetPlayer(0);

        GameControllerScript.current.HideChar();
        GameControllerScript.current.schoolMusic.Stop();

        music.Play();

        player.PlayOneShot(startSounds[0]);

        GameControllerScript.current.player.inNotebook = true;
        GameControllerScript.current.canPause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        timer = music.clip.length;
        doTimer = true;
    }

    public void Answer(int buttonID)
    {
        if (canAnswer)
        {
            bool correctAnswer = false;

            if (question == 0)
            {
                if (buttonID == 0)
                    correctAnswer = true;
            }
            else if (question == 1)
            {
                correctAnswer = false;
            }

            if (correctAnswer)
            {
                player.PlayOneShot(ding);
            }
            else
            {
                Die();
            }

            question++;

            if (question == 1 && correctAnswer)
            {
                timer = music.clip.length;

                breakAnswers = true;

                foreach(GameObject objectt in buttons)
                {
                    if (!objectt.activeInHierarchy)
                        objectt.SetActive(true);
                }

                music.Play();

                player.PlayOneShot(startSounds[1]);

                questionText.text = "What's Obama's last name?";
            }
        }
    }

    public void Die()
    {
        canAnswer = false;
        doTimer = false;

        foreach(MouseoverScript mouses in FindObjectsOfType<MouseoverScript>())
        {
            if (mouses.name.ToLower().Contains("button"))
            {
                mouses.enabled = false;
            }
        }

        music.Stop();

        loseScreen.SetActive(true);

        IEnumerator WaitToDisable()
        {
            player.PlayOneShot(vineThud);

            yield return new WaitForSeconds(0.5f);

            player.PlayOneShot(youStupid);

            yield return new WaitForSeconds(2f);
            
            GameControllerScript.current.ActivateSpoopMode();
            GameControllerScript.current.player.inNotebook = false;
            GameControllerScript.current.ShowChar();
            GameControllerScript.current.canPause = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameControllerScript.current.DeactivateLearningGame(gameObject);
        }

        StartCoroutine(WaitToDisable());
    }

    public void HoverOver(int button)
    {
        if (question == 0)
        {
            player.PlayOneShot(questionSounds[button]);
        }
        else if (question == 1)
        {
            player.PlayOneShot(screech);
        }
    }

    public AudioClip[] questionSounds;
    public AudioClip[] barackquestionSounds;
    public AudioClip[] startSounds;

    bool breakAnswers;

    private void Update()
    {
        if (breakAnswers)
        {
            texts[0].text = CrackhouseUtil.GetRandomString(7);
            texts[1].text = CrackhouseUtil.GetRandomString(7);
            texts[2].text = CrackhouseUtil.GetRandomString(7);
            texts[3].text = CrackhouseUtil.GetRandomString(7);
        }

        if (doTimer && timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Calculations.GetFormattedTime(timer);
            timerAnim.speed = 1f + (music.clip.length - timer);
        }
        else
        {
            timerAnim.speed = 1f;
        }

        if (doTimer && timer <= 0)
        {
            doTimer = false;
            music.Stop();
            Die();
        }

        if (input.GetButton("MenuLeft"))
            Answer(0);
        else if (input.GetButton("MenuRight"))
            Answer(1);
        
        if (question != 0)
        {
            if (input.GetButton("MenuUp"))
                Answer(2);
            else if (input.GetButton("MenuDown"))
                Answer(3);
        }
    }

}
