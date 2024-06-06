using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelScript : MonoBehaviour
{
    public GameObject playerCam;
    float smoothHandLerp = 20.9f;

    // Update is called once per frame
    void Update()
    {
        base.transform.eulerAngles = new Vector3(Mathf.LerpAngle(base.transform.eulerAngles.x, base.transform.eulerAngles.x, smoothHandLerp * Time.deltaTime), Mathf.LerpAngle(base.transform.eulerAngles.y, playerCam.transform.eulerAngles.y, smoothHandLerp * Time.deltaTime), Mathf.LerpAngle(base.transform.eulerAngles.z, playerCam.transform.eulerAngles.z, smoothHandLerp * Time.deltaTime));
        base.transform.position = playerCam.transform.position;
    }
}
