using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using TMPro;

using Cinemachine;

public class PlayerMovementScript : MonoBehaviour
{
	public CinemachineVirtualCamera playerVCam;

	public Animator playerAnimator;

	public float defaultFOV;
	public float targetFOV;

	public bool disableMovement;
	public bool do3DMovement = false;
	public bool staminaByDefault = false;

	public float mouseSensitivity;
	public float walkSpeed;
	public float runSpeed;
	public float maxStamina;

	private Vector3 moveDirection;

	private float playerSpeed;

	public float stamina;

	public CharacterController cc;

	public float height;

    private Quaternion playerRotation;

	public void ToggleMovement(bool disable)
	{
		disableMovement = disable;
	}

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
		defaultFOV = PlayerPrefs.GetFloat("FOV");
		targetFOV = PlayerPrefs.GetFloat("FOV");
		

		height = base.transform.position.y;
		stamina = maxStamina;
		playerRotation = base.transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
	}


	private void Update()
	{
		playerVCam.m_Lens.FieldOfView = Mathf.Lerp(playerVCam.m_Lens.FieldOfView, targetFOV, 0.1f);
	
		base.transform.position = new Vector3(base.transform.position.x, height, base.transform.position.z);

		if (!disableMovement)
		{
			MouseMove();
			PlayerMove();
		}
    }

	private void MouseMove()
	{
		playerRotation.eulerAngles = playerRotation.eulerAngles + Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale;

		if (do3DMovement)
			playerRotation.eulerAngles = playerRotation.eulerAngles + Vector3.left * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale;
			
		base.transform.eulerAngles = playerRotation.eulerAngles;
	}

	private void PlayerMove()
	{
		float speedStuff = walkSpeed;

		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		
		vector = base.transform.forward * Input.GetAxis("Forward");
		vector2 = base.transform.right * Input.GetAxis("Strafe");
	
		if (stamina > 0f)
		{
			if ((Input.GetButton("Run")) || staminaByDefault)
			{
				if (SettingsManager.DynamicFOV == 2 && targetFOV != defaultFOV + 15)
					targetFOV = defaultFOV + 15;

				playerSpeed = runSpeed;
				speedStuff = runSpeed;
			}
			else
			{
				if (SettingsManager.DynamicFOV == 2)
					targetFOV = defaultFOV;

				playerSpeed = walkSpeed;
			}
		}
		else
		{
			playerSpeed = walkSpeed;
		}

		playerSpeed *= Time.deltaTime;
		moveDirection = (vector + vector2).normalized * playerSpeed;

		cc.Move(moveDirection);
		
		if (Input.GetAxis("Forward") != 0 || Input.GetAxis("Strafe") != 0)
		{
			if (playerAnimator.GetBool("Walking") == false)
			{
				playerAnimator.SetTrigger("Walk");
				playerAnimator.SetBool("Walking", true);
			}

			playerAnimator.speed = cc.velocity.magnitude / 16;
		}
		else
		{
			if (playerAnimator.GetBool("Walking") == true)
			{
				playerAnimator.SetTrigger("StopWalking");
				playerAnimator.SetBool("Walking", false);
			}
				
			playerAnimator.speed = 1f;
		}
	}
}
