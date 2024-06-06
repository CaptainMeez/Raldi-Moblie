using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FamilyFriendlyPoster : MonoBehaviour
{
    public Material familyMaterial;

    void Start()
    {
        if (PlayerPrefs.GetFloat("FamilyFriendly") == 2)
        {
            // Material arrays are weird
            Material[] newArray = new Material[2];
            newArray[0] = base.GetComponent<MeshRenderer>().materials[0];
            newArray[1] = familyMaterial;

            base.GetComponent<MeshRenderer>().materials = newArray;
        } 
    }
}
