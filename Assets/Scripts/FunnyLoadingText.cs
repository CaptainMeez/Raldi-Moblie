using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FunnyLoadingText : MonoBehaviour
{
    private string[] texts = {"Loading", "Loading.", "Loading..", "Loading..."};
    int curTex = 0;
    public TextMeshProUGUI guiText;

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        yield return new WaitForSeconds(0.5f);
        curTex++;

        if (curTex > texts.Length - 1)
            curTex = 0;

        guiText.text = texts[curTex];

        StartCoroutine(Loop());
    }
}
