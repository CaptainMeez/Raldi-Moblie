using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilHouse : MonoBehaviour
{
    public AudioSource music;
    public GameObject neil;
    public GameObject objects;

    void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        if (FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
        {
            music.volume = 0;
            neil.SetActive(false);
            objects.SetActive(false);
        }
    }
}