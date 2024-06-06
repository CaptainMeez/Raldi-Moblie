using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaltuhScript : MonoBehaviour
{
    public PlayerScript ps;
    public GameControllerScript gcs;
    public float MethTimeLeft = 0;
    public bool metfirstime = true;
    public float wait = 0;
    public bool intrigger = false;
    public float waittarget = 0;
    public int waittargetid = 0;
    public AudioClip aud_hello;
	public AudioClip aud_do_you_want;
    public AudioClip aud_here_you_go;
    public AudioClip aud_take_a_while;
    public AudioClip aud_not_ready;
    public AudioClip aud_ready;
    public AudioClip aud_sticking_around;
    public AudioClip aud_breakbadtheme;
    public GameObject item;
	public AudioSource audioDevice;
    public TextMeshPro[] text;
    private bool stuckaround = false;

    private float timeinarea = 0;
    // Start is called before the first frame update
    void Start()
    {
        //item.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (MethTimeLeft > 0) {
            MethTimeLeft -= Time.deltaTime;
        } else {
            MethTimeLeft = 0;
        }
        if (MethTimeLeft > gcs.methTimer)
            MethTimeLeft = gcs.methTimer;
        for (int i = 0; i < 2; i++)
        {
            text[i].text = Calculations.GetFormattedTime(MethTimeLeft);
        }
        if (intrigger) {
            this.wait += Time.deltaTime;
        } else {
            this.wait = -0.05f;
            this.waittarget = 0f;
            this.waittargetid = 0;
        }
        if (intrigger)
        {
            if (metfirstime) { // duct tapey and hardcodey as fuck but whatever
                if (this.wait > this.waittarget)
                {
                    if (waittargetid == 0)
                    {
                        this.waittarget += aud_hello.length;
                        audioDevice.PlayOneShot(aud_hello);
                    }
                    if (waittargetid == 1)
                    {
                        this.waittarget += aud_do_you_want.length;
                        audioDevice.PlayOneShot(aud_do_you_want);
                    }
                    if (waittargetid == 2)
                    {
                        this.waittarget += aud_here_you_go.length;
                        audioDevice.PlayOneShot(aud_here_you_go);
                        MethTimeLeft = gcs.methTimer;
                        metfirstime = false;
                        item.SetActive(true);
                    }
                    this.waittargetid += 1;
                }
            } else {
                if (this.wait > this.waittarget)
                {
                    if (waittargetid == 3)
                    {
                        audioDevice.PlayOneShot(aud_take_a_while);
                        this.waittargetid += 1;
                    }
                }
                if (MethTimeLeft > 0) {
                    if (waittargetid == 0)
                    {
                        this.waittargetid += 1;
                        audioDevice.PlayOneShot(aud_not_ready);
                    }
                } else if (MethTimeLeft == 0) {
                    if (waittargetid == 0)
                    {
                        this.waittargetid += 1;
                        if (!stuckaround)
                            audioDevice.PlayOneShot(aud_ready);
                        else
                            audioDevice.PlayOneShot(aud_sticking_around);
                        stuckaround = false;
                        item.SetActive(true);
                        MethTimeLeft = gcs.methTimer;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
	{
        if (other.transform.name == "Player") {
            intrigger = true;
        }
	}
    private void OnTriggerExit(Collider other) {
        if (other.transform.name == "Player") {
            intrigger = false;
            timeinarea = 0;
            stuckaround = false;
        }
    }
    private void OnTriggerStay(Collider other)
	{
        timeinarea += Time.deltaTime;
        if (other.transform.name == "Player" && !(MethTimeLeft > 0) && !metfirstime && !item.activeSelf) {
            if (timeinarea > 0.1f)
                stuckaround = true;
            print(stuckaround);
            waittargetid = 0;
        }
	}
}
