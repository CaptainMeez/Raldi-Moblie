using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWPrizeColliderTrigger : MonoBehaviour
{
   public HammerableWindowScript hws;

    public void OnTriggerEnter(Collider other)
    {
        bool condition = other.gameObject.name == "1st Prize";

        if (condition)
            hws.Smash();
    }
}
