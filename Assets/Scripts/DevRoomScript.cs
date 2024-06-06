using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevRoomScript : MonoBehaviour
{
    public GameObject neil;
    public AudioClip creditsMusic;
    public AudioSource music;
    public GameObject creditsObject;
    private bool fadeOut = false;

    public void Start()
    {
        if (PlayerPrefs.GetFloat("ShowNeilFinalCredits") == 0)
            neil.SetActive(true);
    
        if (fadeOut)
            music.volume -= Time.deltaTime / 5;
    }

    public void CreditsFunny()
    {
        IEnumerator Credits()
        {
            GameJolt.API.Trophies.TryUnlock(188673);
            
            PlayerPrefs.SetFloat("ShowNeilFinalCredits", 1);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            music.Stop();
            music.PlayOneShot(creditsMusic);
            creditsObject.SetActive(true);

            yield return new WaitForSeconds(120);

            fadeOut = true;

            yield return new WaitForSeconds(15);

            SceneManager.LoadScene("MainMenu");
        }
        
        StartCoroutine(Credits());
    }
}
