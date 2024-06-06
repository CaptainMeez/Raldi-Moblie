using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameRaldiTutor : MonoBehaviour
{
    [SerializeField] private RaldiMinigameBus bus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!GameControllerScript.current.finaleMode)
                bus.Drop();
        }
    }
}
