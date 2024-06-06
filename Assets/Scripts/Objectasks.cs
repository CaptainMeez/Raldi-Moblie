using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameJolt;

public class Objectasks : MonoBehaviour
{
    public ObjectasksScreen screen;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void CollectObjectask(int task)
    {
        if (FindObjectOfType<PlayerStats>().data.foundObjectasks)
        {
            FindObjectOfType<PlayerStats>().data.objectasks[task] = 1;
            FindObjectOfType<PlayerStats>().Save();
            source.PlayOneShot(screen.doTask);
            screen.UpdateTasks();

            bool completedAll = true;

            int index = 0;

            foreach(string pref in screen.unlockPrefs)
            {
                if (FindObjectOfType<PlayerStats>().data.objectasks[index] == 0)
                    completedAll = false;

                index++;
            }

            if (completedAll)
                GameJolt.API.Trophies.Unlock(182824);  
        }        
    }
}
