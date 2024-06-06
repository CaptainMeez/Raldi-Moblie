using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionManager
{
    public static void ShowCaption(Caption[] captions)
    {
        IEnumerator ShowCaptions()
        {
            foreach(Caption caption in captions)
            {
                CaptionDisplayer captiond = GameObject.Instantiate<GameObject>(GameObject.FindObjectOfType<CaptionController>().captionPrefab).GetComponent<CaptionDisplayer>();
                captiond.textColor = caption.textColor;
                captiond.ChangeFollowpoint(caption.followPoint);
                captiond.UpdateText(caption.text);
                yield return new WaitForSeconds(caption.time);
                GameObject.Destroy(captiond.gameObject);
            }
        }

        GameObject.FindObjectOfType<CaptionController>().StartCoroutine(ShowCaptions()); // monobehavior moment
    }
}

public class Caption
{
    public string text;
    public float time;
    public Transform followPoint;
    public Color textColor;

    public Caption(string text, float time, Color textColor, Transform followPoint = null)
    {
        this.text = text;
        this.time = time;
        this.textColor = textColor;

        if (followPoint != null)
            this.followPoint = followPoint;
    }
}
