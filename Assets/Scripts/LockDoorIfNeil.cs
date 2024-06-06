using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorIfNeil : MonoBehaviour
{
    public Material lockk;
    public GameObject raldi;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        if (GameControllerScript.current.neilMode || GameControllerScript.current.mode == "hard")
        {
            ForceLockDoor();  
        }
    }

    public void ForceLockDoor()
    {
        base.GetComponent<MeshRenderer>().material = lockk;
        base.GetComponent<BoxCollider>().isTrigger = false;
        raldi.SetActive(false);
    }
}
