using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJInfoTick : MonoBehaviour
{
    public GameObject objectt;

    void Start()
    {
        if (PlayerPrefs.GetFloat("GJTick") == 0)
            objectt.SetActive(true);
    }

    public void OnClick()
    {
        PlayerPrefs.SetFloat("GJTick", 1);
    }
}