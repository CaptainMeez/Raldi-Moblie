using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilCreditsTrigger : MonoBehaviour
{
    public GameObject neil;
    public AudioSource source;
    public AudioSource music;
    public PlayerMovementScript player;
    public AudioClip neilEnd;
    public AudioClip creditsMusic;
    bool fadeOut = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (fadeOut)
        {
            music.volume -= Time.deltaTime / 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.disableMovement = true;

            StartCoroutine(StartCredits());
        }
    }

    public GameObject creditsObject;

    IEnumerator StartCredits()
    {
        FindObjectOfType<PlayerStats>().data.theEnd = true;
        FindObjectOfType<PlayerStats>().data.interactedWithNeil = false;
        FindObjectOfType<PlayerStats>().Save();

        PlayerPrefs.SetFloat("RepriseMenu", 1);
        
        music.Stop();
        source.PlayOneShot(neilEnd);
        neil.SetActive(true);
        
        yield return new WaitForSeconds(neilEnd.length + 0.2f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        music.PlayOneShot(creditsMusic);
        creditsObject.SetActive(true);

        yield return new WaitForSeconds(120);

        fadeOut = true;

        yield return new WaitForSeconds(15);
        
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }
}
