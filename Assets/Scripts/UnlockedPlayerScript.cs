using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

using TMPro;

// Token: 0x020000D1 RID: 209
public class UnlockedPlayerScript : MonoBehaviour
{
	private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    public float stamina = 100f;
    public TextMeshProUGUI display;
    private float speedmultiplier;
    private float xRot;

    [SerializeField] private LayerMask floorMask;
    [SerializeField] private Transform feetTransform;
    public Transform PlayerCamera;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float Speed;
    private float Sensitivity;
    [SerializeField] private float Jumpforce;
    public Transform holdPos;
    public Transform noreverseholdPos;
    public Transform reverseholdPos;

    public Transform wallSpawn;

    private void Start()
    {
        display = GameControllerScript.current.phase2sprinttext;
        Sensitivity = PlayerPrefs.GetFloat("MouseSensitivity");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if ((Input.GetAxis("Strafe") != 0) && Input.GetAxis("Forward") != 0)
            PlayerMovementInput = new Vector3((Input.GetAxis("Strafe")*speedmultiplier)/Mathf.Sqrt(2), 0f, (Input.GetAxis("Forward")*speedmultiplier)/Mathf.Sqrt(2));
        else
            PlayerMovementInput = new Vector3((Input.GetAxis("Strafe")*speedmultiplier), 0f, (Input.GetAxis("Forward")*speedmultiplier));
            
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        if (Input.GetButton("Look Behind"))
            holdPos = reverseholdPos;
        else
            holdPos = noreverseholdPos;
        if (Input.GetButton("Run"))
        {
            if (stamina > 0) {
                speedmultiplier = 2f;
                stamina -= Time.deltaTime*20;
            } else {
                speedmultiplier = 1;
                stamina = 0;
            }
        } else {
            speedmultiplier = 1;
            if (Input.GetAxis("Strafe") != 0 || Input.GetAxis("Forward") != 0)
            {

            } else {
                if (stamina < 100)
                    stamina += Time.deltaTime*10;
                else
                    stamina = 100;
            }
        }
        display.text = Mathf.Floor(stamina) + "%";
        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(feetTransform.position, 0.1f, floorMask))
                PlayerBody.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
        }
    }

    private int lookBehind;

    private void MovePlayerCamera()
    {
        if (Input.GetButton("Look Behind"))
			this.lookBehind = 180;
		else
			this.lookBehind = 0;

        xRot -= PlayerMouseInput.y * Sensitivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f); 

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, PlayerCamera.transform.localRotation.y + lookBehind, 0);
    }
}