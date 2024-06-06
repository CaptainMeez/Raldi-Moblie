using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingDogMusic : MonoBehaviour
{
    public AudioSource music;
    public bool indaroom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !indaroom)
        {
            indaroom = true;

            music.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && indaroom)
        {
            indaroom = false;
            music.Stop();
            
            for (int i = 0; i < 5; i++)
			{
				if (GameControllerScript.current.item[i] == 24) {
                    Animator[] theAnimators = GameControllerScript.current.explosionAnimators;

                    if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                        theAnimators = GameControllerScript.current.basicExplosionAnimators;

                    Animator theAnimator = GameControllerScript.current.hotbarAnimator;

                    if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                        theAnimator = GameControllerScript.current.basicHotbarAnimator;

                    theAnimator.SetTrigger("kaboom");
					GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.robloxrocketsound);
					GameControllerScript.current.LoseItem(i);
				    theAnimators[i].SetTrigger("kaboom");
				}
			}
        }
    }
}
