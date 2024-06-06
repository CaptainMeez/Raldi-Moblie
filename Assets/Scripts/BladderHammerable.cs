using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladderHammerable : MonoBehaviour
{
    public MeshRenderer posterWall;
    public Material brokenPoster;
    public bool brokenAlready;

    void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();

        brokenAlready = FindObjectOfType<PlayerStats>().data.windowIsBroken;

        if (brokenAlready)
        {
            posterWall.material = brokenPoster;
            base.GetComponent<MeshCollider>().enabled = false;
            base.GetComponent<MeshRenderer>().material = FindObjectOfType<GameControllerScript>().brokenWindow;
        }
    }

    public void OnWindowBreak()
    {
        FindObjectOfType<PlayerStats>().data.windowIsBroken = true;
        FindObjectOfType<PlayerStats>().Save();
    }
}
