using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class NeilBook : MonoBehaviour
{
    public GameControllerScript gc;
    public TextMeshProUGUI text;

    public Image neilImage;

    public Sprite ishaan;

    public string[] texts = {"CAN'T THIS GAME BE NORMAL FOR ONCE", "THAT'S ALL IM TRYING TO DO.", "STOP GIVING INTO HIM", "WHY WONT THIS GAME STOP BEING AN **AMONG US**", "GOOD LUCK, " + System.Environment.UserName.ToUpper() + "."};

    public float walk;
    public float run;

    public void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        walk = gc.player.walkSpeed;
        run = gc.player.runSpeed;

        gc.player.walkSpeed = 0f;
        gc.player.runSpeed = 0f;

        gc.baldi.speed = 0f;
        gc.canPause = false;
        if (!gc.ishaanMode)
        {
            text.text = texts[UnityEngine.Random.Range(0, texts.Length)];
        }
        else
        {
            neilImage.sprite = ishaan;
            text.text = "hello it is me ishaan do you want to buy some potatoes";
        }
        if (!gc.neilModeExits)
        {
            gc.entrance_0.Lower();
            gc.entrance_1.Lower();
            gc.entrance_2.Lower();
            gc.entrance_3.Lower();

            gc.neilModeExits = true;
        }

        if (gc.notebooks == 2)
        {
            gc.baldi.gameObject.SetActive(true);
            gc.baldi.speed = 100;
            gc.baldi.agent.enabled = false;
            gc.baldi.transform.position = gc.neilSpawns[UnityEngine.Random.Range(0, gc.neilSpawns.Length)].position;
            gc.baldi.agent.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Exit();
    }
    
    public void Exit()
    {
        IEnumerator ExitBook()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gc.AddMoney(0.125f);
            gc.player.walkSpeed = walk;
            gc.player.runSpeed = run;
            gc.baldi.GetAngry(1.3f);

            if (gc.notebooks == 1) gc.neilLetter.SetActive(false);
            if (gc.notebooks != 1) gc.baldi.speed = 100f;
            if (gc.notebooks == gc.notebooksToCollect) gc.ActivateFinaleMode();

            yield return null;

            gc.canPause = true;
            gc.DeactivateLearningGame(base.gameObject);
        }

        StartCoroutine(ExitBook());
    }
}
