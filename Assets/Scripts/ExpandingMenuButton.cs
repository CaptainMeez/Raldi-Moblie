using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingMenuButton : MonoBehaviour
{
    bool hover = false;
    private Vector3 ogScale;
    private Vector3 hoverScale;

    void Start()
    {
        ogScale = transform.localScale;
        hoverScale = ogScale + new Vector3(0.2f, 0.2f, 0.2f);
    }

    private int lerpval = 6;

    void Update()
    {
        if (hover)
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, hoverScale.x, Time.deltaTime * lerpval), Mathf.Lerp(transform.localScale.y, hoverScale.y, Time.deltaTime * lerpval), Mathf.Lerp(transform.localScale.z, hoverScale.z, Time.deltaTime * lerpval));
        else
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, ogScale.x, Time.deltaTime * lerpval), Mathf.Lerp(transform.localScale.y, ogScale.y, Time.deltaTime * lerpval), Mathf.Lerp(transform.localScale.z, ogScale.z, Time.deltaTime * lerpval));
    }

    public void Hover(bool hovered)
    {
        hover = hovered;
    }
}
