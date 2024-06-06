using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilSecondaryHitbox : MonoBehaviour
{
    public NeilScript neil;
    public bool isShield = false;

    /*private void OnTriggerEnter(Collider other)
    {
        if (isShield)
            neil.cameFromShield = true;

        neil.OnTriggerEnter(other);
    }*/
}
