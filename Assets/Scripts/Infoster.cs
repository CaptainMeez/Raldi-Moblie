using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Infoster : MonoBehaviour
{
    public Material dtMat;

    void Start()
    {
        if (PlayerPrefs.GetString("CurrentMode") == "story_double")
        {
            Material[] newArray = new Material[2];
            newArray[0] = base.GetComponent<MeshRenderer>().materials[0];
            newArray[1] = dtMat;

            base.GetComponent<MeshRenderer>().materials = newArray;
        } 
    }
}
