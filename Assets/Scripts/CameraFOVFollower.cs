using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFOVFollower : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera tcamera;
    private Camera thisCamera;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
        tcamera = FindObjectOfType<GameControllerScript>().player.playerVCam;
    }

    void Update()
    {
        if (thisCamera.fieldOfView != tcamera.m_Lens.FieldOfView)            
            thisCamera.fieldOfView = tcamera.m_Lens.FieldOfView;
    }
}
