using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldiCharacterManager : MonoBehaviour
{
    public GameObject tutorBaldi;
    public DTBaldiScript baldi;
    public FirstPrizeScript prize;
    public PrincipalScript principal;
    public SweepScript gottaSweep;
    public BullyScript bully;
    public DTCraftersScript crafters;
    public DTPlaytimeScript playtime;

    public void OnTutorSpawn()
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
        {
            tutorBaldi.SetActive(true);
        }
    }

    public void OnSpoopMode()
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
        {
            tutorBaldi.SetActive(false);
        }
    }

    public void GetAngry(float anger)
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
            baldi.GetAngry(anger);
    }
    
    public void Hear(Vector3 soundLocation, float priority)
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
            baldi.Hear(soundLocation, priority);
    }

    public void RunCharacterSpawnCheck(bool show = false)
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
        {
            switch(show)
            {
                case true:
                    baldi.gameObject.SetActive(true);
                    prize.gameObject.SetActive(true);
                    principal.gameObject.SetActive(true);
                    gottaSweep.gameObject.SetActive(true);
                    bully.gameObject.SetActive(true);
                    crafters.gameObject.SetActive(true);
                    playtime.gameObject.SetActive(true);
                    break;
                case false:
                    baldi.gameObject.SetActive(false);
                    prize.gameObject.SetActive(false);
                    principal.gameObject.SetActive(false);
                    gottaSweep.gameObject.SetActive(false);
                    bully.gameObject.SetActive(false);
                    crafters.gameObject.SetActive(false);
                    playtime.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void DespawnCrafters()
	{
		this.crafters.gameObject.SetActive(false);
	}
}