using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    public Transform soul;
    Vector3 start;

    void Start()
    {
        start = soul.localPosition;
    }

    public void Init()
    {
        soul.localPosition = start;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            soul.localPosition += new Vector3(Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
            soul.localPosition -= new Vector3(Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            soul.localPosition += new Vector3(0, Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.DownArrow))
            soul.localPosition -= new Vector3(0, Time.deltaTime, 0);

        if (soul.localPosition.x > 0.4418f)
            soul.localPosition = new Vector3(0.4418f, soul.localPosition.y, 0);
        else if (soul.localPosition.x < -0.4418f)
            soul.localPosition = new Vector3(-0.4418f, soul.localPosition.y, 0);

        if (soul.localPosition.y > 0.4411f)
            soul.localPosition = new Vector3(soul.localPosition.x, 0.4411f, 0);
        else if (soul.localPosition.y < -0.4411f)
            soul.localPosition = new Vector3(soul.localPosition.x, -0.4411f, 0);
    }
}