using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScrollScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Zoom(0.5f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Zoom(-0.5f);
        }
    }

    public void Zoom(float change)
    {
        base.transform.localScale += new Vector3(change, change, change);

        if (base.transform.localScale.x > 4)
            base.transform.localScale = new Vector3(4, 4, 4);
        if (base.transform.localScale.x < 1)
            base.transform.localScale = new Vector3(1, 1, 1);
    }
}
