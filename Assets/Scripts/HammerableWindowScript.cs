using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerableWindowScript : MonoBehaviour
{
    public GameControllerScript gc;
    public BoxCollider smashhitbox;
    public GameObject window;
    public GameObject neilwindow;
    public GameObject brokenwindow;
    public AudioClip smashsound;
    private AudioSource audiosource;
    int ignorelayer;
    int notignorelayer;
    private float justtomakesurenoaccidentalgas = 0;
    private bool checkedgas = false;
    private bool smashed = false;
    private bool updateonce = false;

    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        gc = FindObjectOfType<GameControllerScript>();
        ignorelayer = LayerMask.NameToLayer("HammerWindow");
        notignorelayer = LayerMask.NameToLayer("Default");
    }

    void Update()
    {
        if (gc.neilMode)
        {
            gameObject.layer = ignorelayer;
            window.layer = ignorelayer;
            neilwindow.layer = ignorelayer;
            if (!updateonce)
            {
                window.SetActive(false);
                neilwindow.SetActive(true);
                updateonce = true;
            }
        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

		if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 10 && raycastHit.transform.gameObject.tag == "HammerableWindow" && !smashed)
		{
            if (gc.item[gc.itemSelected] == 16 && RaldiInputManager.current.GetUseDown())
            {
                gc.baldi.Hear(gc.player.transform.position, 4f);
                Smash(true);
                justtomakesurenoaccidentalgas = 0.025f;
            }
        }
        if (justtomakesurenoaccidentalgas > 0)
        {
            checkedgas = false;
            justtomakesurenoaccidentalgas -= Time.deltaTime;
        } else if (!checkedgas)
        {
            gc.ResetItem();
            checkedgas = true;
        }
    }

    public void Smash(bool playerCaused = false)
    {
        if (PlayerPrefs.GetFloat("destruction") == 1)
		    gc.player.ResetGuilt("hammer", 2f);
        smashed = true;
        window.SetActive(false);
        neilwindow.SetActive(false);
        brokenwindow.SetActive(true);
        audiosource.PlayOneShot(smashsound, 1f);

        bool canGetAchivement = true;
        if (playerCaused)
            gc.GiveScore(50);
        foreach(HammerableWindowScript window in FindObjectsOfType<HammerableWindowScript>())
        {
            if (!window.smashed)
                canGetAchivement = false;
        }

        if (canGetAchivement)
            GameJolt.API.Trophies.TryUnlock(189095);
    }
}
