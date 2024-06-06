using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolitareCreditsOriginal : MonoBehaviour
{
    public GameObject credits;
    public GameObject anotherLife;
    public GameObject blackScreen;
    public GameObject igetitbutton;
    public AudioClip anotherLifeTheme;
    public Transform playerAfterSpawn;
    private bool skipcutscene;

    public List<GameObject> sources = new List<GameObject>();
    public void Skip()
    {
        print("skip");
        skipcutscene = true;
        igetitbutton.SetActive(false);
    }
    public void Init()
    {
        bool hasCard = false;
        
        int index = 0;

        GameControllerScript.current.canPause = false;

        foreach(int item in GameControllerScript.current.item)
        {
            print("checking card");
            if (item == 19)
                hasCard = true;

            GameControllerScript.current.LoseItem(index);

            index++;
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(5);
            if (FindObjectOfType<PlayerStats>().data.dealtWithSolitary)
            {
                igetitbutton.SetActive(true);
                GameControllerScript.current.UnlockMouse();
            }
            for( float timer = 62 ; timer >= 0 ; timer -= Time.deltaTime )
            {
                print(timer);
                if(skipcutscene)
                {
                    timer = 0;
                }
                if (timer >= 0)
                    yield return null;
            }
            credits.SetActive(false);
            GameControllerScript.current.schoolMusic.Stop();
            FindObjectOfType<PlayerStats>().data.dealtWithSolitary = true;
			FindObjectOfType<PlayerStats>().Save();
            if (!hasCard && !GameControllerScript.current.finaleMode)
            {
                print("die");
                PlayerPrefs.SetInt("recentNormalModeNotebooks", GameControllerScript.current.notebooks);
				PlayerPrefs.SetInt("recentNormalModeExits", GameControllerScript.current.exitsReached);
                SceneManager.LoadSceneAsync("GameOver");
            }
            else
            {
                 GameControllerScript.current.mrBeast.beasting = false;
                GameControllerScript.current.mrBeast.gettingMad = false;
                GameControllerScript.current.mrBeast.killingPlayer = false;
                GameControllerScript.current.beastCardCollected = false;
                GameControllerScript.current.LockMouse();
                anotherLife.SetActive(true);
                blackScreen.SetActive(false);
                GameControllerScript.current.hud.SetActive(true);

                StartCoroutine(DisableTrueAnimation());

                GameControllerScript.current.player.inNotebook = true;
                GameControllerScript.current.schoolMusic.clip = anotherLifeTheme;
                GameControllerScript.current.schoolMusic.loop = false;
                GameControllerScript.current.schoolMusic.Play();

                Camera.main.farClipPlane = SettingsManager.RenderDistance;

                GameControllerScript.current.HideChar();
                GameControllerScript.current.player.gameOver = false;
                GameControllerScript.current.gameOverDelay = 0.5f;
                GameControllerScript.current.chipfloke.isSolitareConfined = false;

                yield return new WaitForSeconds(30.3f);
                GameControllerScript.current.money.money = 0;
                GameControllerScript.current.player.cc.enabled = false;
                GameControllerScript.current.player.transform.position = playerAfterSpawn.position;
                GameControllerScript.current.player.transform.eulerAngles = new Vector3(0, 0, 0);
                GameControllerScript.current.player.cc.enabled = true;
                GameControllerScript.current.canPause = true;

                GameControllerScript.current.beastTip.SetActive(false);

			    GameControllerScript.current.atm.infoTick.SetActive(false);

			    FindObjectOfType<BeastCardArea>().ui_Card.SetActive(false);
                
                yield return null;

                GameControllerScript.current.player.inNotebook = false;

                GameControllerScript.current.trueReset = true;

                RenderSettings.fog = true;
                RenderSettings.fogColor = Color.white;
            }
        }

        StartCoroutine(Wait());
    }

    IEnumerator DisableTrueAnimation()
    {
        yield return new WaitForSeconds(40f);
        anotherLife.SetActive(false);
    }
}
