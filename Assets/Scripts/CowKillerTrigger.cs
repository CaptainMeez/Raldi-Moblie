using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowKillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Application.Quit();
            print("QUIT");
        }
    }
}
