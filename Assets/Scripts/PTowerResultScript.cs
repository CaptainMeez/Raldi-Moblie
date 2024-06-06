using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PTowerResultScript : MonoBehaviour
{
    private bool modifiersOn = false;
    public int rank;
    public int score;
    public float time;
    public AudioClip[] ranksounds = new AudioClip[6];
    public AudioClip modifrank;
    public Sprite[] rankimages = new Sprite[6];
    public Image rankimage;
    public AudioSource ad;
    public Animator playeranim;
    public bool indoom;
    public GameObject doomscreen;
    public GameObject[] stats = new GameObject[3];
    public TextMeshProUGUI[] stattexts=  new TextMeshProUGUI[3];
    public GameObject modifierText; 
    public int doomstate = 0;
    public AudioSource doomad;
    private float playsoundtimer = 0;
    private bool counting = false;
    public AudioClip pistol;
    public AudioClip doomexplosion;
    private bool donecounting = false;
    private string[] ranks = new string[6]{"d","c","b","a","s","p"};
    private bool newrecord = false;
    public GameObject Pscene;
    public Color darkness;
    public Material galaxysky;
    public GameObject flash;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Modifier modifier in ModifierMenu.modifiers)
		{
			if (PlayerPrefs.GetFloat(ModifierMenu.ConvertToInternal(modifier.name)) == 1)
				modifiersOn = true;
		}
        FindObjectOfType<PlayerStats>().TryLoad();
        if (!modifiersOn)
        {
            FindObjectOfType<PlayerStats>().data.beatHardMode = true;
            UnlockAchievement();
        }
        modifierText.SetActive(false);
        Pscene.SetActive(false);
        if (!modifiersOn)
        {
            if (PlayerPrefs.GetInt("CurSessionScore") > FindObjectOfType<PlayerStats>().data.recordScore)
            {
                FindObjectOfType<PlayerStats>().data.recordScore = PlayerPrefs.GetInt("CurSessionScore");
                newrecord = true;
            }
        } 

        FindObjectOfType<PlayerStats>().Save();

        doomscreen.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            stats[i].SetActive(false);
        }
        score = PlayerPrefs.GetInt("CurSessionScore");
        rank = PlayerPrefs.GetInt("CurSessionRank");
        time = PlayerPrefs.GetFloat("CurSessionPlaytime");
        rankimage.enabled = false;
        if (!modifiersOn)
        {
        ad.PlayOneShot(ranksounds[rank]);
        playeranim.SetTrigger(ranks[rank]);
        } else {
            ad.PlayOneShot(modifrank);
            playeranim.SetTrigger("m");
        }
        IEnumerator ShowImage()
        {
            yield return new WaitForSeconds(3.69f);
            if (modifiersOn)
                print("lol");
            if (rank < 5 && !modifiersOn)
            {
                rankimage.sprite = rankimages[rank];
                rankimage.enabled = true;
            }
            else if (!modifiersOn)
            {
                flash.SetActive(false);
                yield return new WaitForSeconds(0.65f);
                flash.SetActive(true);
                Pscene.SetActive(true);
                RenderSettings.skybox = galaxysky;
                RenderSettings.ambientLight = darkness;
            } 
            else
            {
                rankimage.sprite = rankimages[rank];
                rankimage.enabled = true;
                modifierText.SetActive(true);
            }
            yield return new WaitForSeconds(12 - 3.69f);
            doomscreen.SetActive(true);
            yield return new WaitForSeconds(1);
            doomstate = 1;
            indoom = true;
        }

        StartCoroutine(ShowImage());
    }

    public void UnlockAchievement()
    {
        GameJolt.API.Trophies.TryUnlock(188677);
    }

    // Update is called once per frame
    float carry = 0;
    private int curdoomstate;
    
    public GameObject recordtext;
    public AudioClip victory;
    void FixedUpdate()
    {
        IEnumerator WaitForNext()
        {
            doomstate = 0;
            counting = false;
            carry = 0;
            yield return new WaitForSeconds(1f);
            doomstate += curdoomstate += 1;
        }
        IEnumerator WaitForMenu()
        {
            doomstate = 0;
            counting = false;
            carry = 0;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("MainMenu");
        }
        if (indoom)
        {
            if (doomstate == 1)
            {
                stats[0].SetActive(true);
                stattexts[0].text = "" + Mathf.RoundToInt(carry);
                counting = true;
                if (carry < score)
                {
                    carry += Mathf.CeilToInt(score / 50);
                } else if (carry > score) {
                    carry = score;
                } else {
                    counting = false;
                    curdoomstate = doomstate;
                    StartCoroutine(WaitForNext());
                }
            } else if (doomstate == 2) {
                stats[1].SetActive(true);
                stattexts[1].text = "" + ranks[Mathf.RoundToInt(carry)].ToUpper();
                counting = true;
                if (carry < rank)
                {
                    carry += 1;
                } else {
                    counting = false;
                    StartCoroutine(WaitForNext());
                }
            } else if (doomstate == 3) {
                stats[2].SetActive(true);
                stattexts[2].text = "" + Calculations.GetFormattedTime(carry);
                counting = true;
                if (carry < time)
                {
                    carry += 7;
                } else if (carry > time) {
                    carry = time;
                } else {
                    counting = false;
                    StartCoroutine(WaitForNext());
                }
            } else if (doomstate == 4) {
                if (newrecord)
                {
                    recordtext.SetActive(true);
                    doomad.PlayOneShot(victory);
                    doomstate = 5;
                }
                StartCoroutine(WaitForMenu());
            }
            if (counting)
            {
                donecounting = false;
                if (playsoundtimer >= 0)
                {
                    playsoundtimer -= Time.deltaTime;
                } else {
                    doomad.clip = pistol;
                    doomad.Play();
                    playsoundtimer = 0.08f;
                }
            } else {
                if (!donecounting)
                {
                    doomad.clip = doomexplosion;
                    doomad.Play();
                    donecounting = true;
                }
                playsoundtimer = 0;
            }
        }
    }
}
