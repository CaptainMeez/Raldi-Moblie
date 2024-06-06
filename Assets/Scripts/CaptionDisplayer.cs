using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CaptionDisplayer : MonoBehaviour
{
    private Transform followPoint;
    public TextMeshPro caption;
    private string text;
    public Color textColor;

    public CaptionDisplayer(string text, Transform followPoint)
    {
        if (followPoint != null)
            this.followPoint = followPoint;

        caption.text = text;
    }

    void Update()
    {
        if (followPoint != null)
            base.transform.position = followPoint.position;
    }

    public void ChangeFollowpoint(Transform newf)
    {
        followPoint = newf;
    }

    public void UpdateText(string newt)
    {
        text = newt;
        caption.text = text;
        caption.color = textColor;
    }
}
