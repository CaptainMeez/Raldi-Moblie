using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallySimpleSwingDoorScript : MonoBehaviour
{
    public AudioClip open;
    public Material openmat;

    public MeshRenderer[] sides;

    bool opened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !opened)
        {
            opened = true;

            GetComponent<AudioSource>().PlayOneShot(open);

            foreach(MeshRenderer side in sides)
            {
                side.material = openmat;
            }
        }
    }
}
