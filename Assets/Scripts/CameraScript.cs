using System;
using UnityEngine;

using Rewired;

public class CameraScript : MonoBehaviour
{
	public Cinemachine.CinemachineVirtualCamera cam;

	public Vector3 jumpHeightV3;
	private Vector3 basePosition;

	private int lookBehind;

	public float speedModifier = 1f;
	public float jumpHeight;
	private float initVelocity = 3;
	private float velocity;
	private float gravity = 5;

	public bool preventBackLooking = false;	
	private bool canJump = false;

	public Player input;

	private void Start()
	{
		cam.m_Lens.FarClipPlane = SettingsManager.RenderDistance;
		basePosition = base.transform.localPosition;

		input = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		canJump = (GameControllerScript.current.player.isMonke || GameControllerScript.current.player.jumpRope || GameControllerScript.current.player.dt_jumpRope);

		if (canJump)
		{
			velocity -= this.gravity * Time.deltaTime * (Convert.ToInt16(GameControllerScript.current.mode == "hard") + 1);
			jumpHeight += this.velocity * Time.deltaTime * (Convert.ToInt16(GameControllerScript.current.mode == "hard") + 1);	
			
			lookBehind = 0;
			base.transform.rotation = GameControllerScript.current.player.transform.rotation * Quaternion.Euler(0f, (float)this.lookBehind, 0f);

			if (this.jumpHeight <= 0f)
			{
				this.jumpHeight = 0f;
				speedModifier = 1f;
				lookBehind = 0;
				GameControllerScript.current.player.jumpModifier = 1f;

				if (input.GetButton("Jump"))
				{
					this.velocity = this.initVelocity;
					speedModifier = 1.3f * (Convert.ToInt16(GameControllerScript.current.mode == "hard") + 1);
				}
			}
			else
				GameControllerScript.current.player.jumpModifier = 1.5f;

			this.jumpHeightV3 = new Vector3(0f, this.jumpHeight, 0f);
		}
		else
		{
			GameControllerScript.current.player.jumpModifier = 1f;
			jumpHeight = 0;
			velocity = 0;
			transform.localPosition = basePosition;
		}

		if (input.GetButton("Jump") && !preventBackLooking && !canJump)
			lookBehind = 180;
		else
			lookBehind = 0;
	}

	private void LateUpdate()
	{
		if (GameControllerScript.current.player.gameOver)
		{
			Transform target = GameControllerScript.current.deathCameras[GameControllerScript.current.currentDeathCamera];

			base.transform.position = target.transform.position + target.transform.forward * 2f + new Vector3(0f, 5f, 0f);
			base.transform.LookAt(new Vector3(GameControllerScript.current.baldi.transform.position.x, GameControllerScript.current.baldi.transform.position.y + 5f, GameControllerScript.current.baldi.transform.position.z));
		}
		else
		{
			if (!canJump)
				base.transform.rotation = GameControllerScript.current.player.transform.rotation * Quaternion.Euler(0f, (float)this.lookBehind, 0f);
			else
				base.transform.localPosition = basePosition + this.jumpHeightV3;	
		}
	}
}
