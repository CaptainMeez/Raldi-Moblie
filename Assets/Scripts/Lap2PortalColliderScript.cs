using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lap2PortalColliderScript : MonoBehaviour
{
    public EntranceScript root;

    public bool lap2initialized = false;

    void OnTriggerEnter(Collider other)
    {
        IEnumerator Collide()
        {
            lap2initialized = true;
            GameControllerScript.current.GiveScore(1250);
            GameControllerScript.current.ActivateLap2();
            root.Raise(); // really stupid fix but ok
            yield return new WaitForSeconds(GameControllerScript.current.portalenter.length + 0.05f);
            root.Lower();
        }
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Collide());
        }
    }
}
