using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWBoxColliderScript : MonoBehaviour
{
    public HammerableWindowScript hws;

    public void OnTriggerEnter(Collider other)
    {
        bool condition = 
        (other.gameObject.tag == "NPC" && hws.gc.neilMode) || 
        (other.gameObject.tag == "NeilProjectile" && hws.gc.neilMode && other.GetComponent<NeilProjectileScript>().shootForward) || 
        (other.gameObject.tag == "Player" && GameControllerScript.current.player.isMonke);
        
        if (condition)
            hws.Smash(true);
    }
}
