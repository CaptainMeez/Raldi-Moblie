using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GustavBedScript : MonoBehaviour
{
    public Animator gustav;
    private bool died = false;
    public AudioClip die;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameControllerScript.current.player.isMonke && !died)
        {
            died = true;
            gustav.SetTrigger("Kill");
            base.GetComponent<AudioSource>().PlayOneShot(die);

            GameControllerScript.current.GiveScore(100);
        }
    }
}