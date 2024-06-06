using System;
using UnityEngine;
using System.Collections;

public class EntranceScript : MonoBehaviour
{
	public Material map;
	public MeshRenderer wall;
	public GameObject portal;
	public GameObject portalboxcollider;
	private bool checkd = false;
	private bool allexits = false;

	public IEnumerator Start()
	{
		LowerPortal();

		yield return new WaitForSeconds(1);

		if (GameControllerScript.current.mode != "hard")
			portal.SetActive(false);
		else
			portal.SetActive(true);
	}

	public void Update()
	{
		if (portalboxcollider != null )
		{
			if (portalboxcollider.GetComponent<Lap2PortalColliderScript>().lap2initialized == true && !checkd)
			{
				Lower();
				checkd = true;
			}
		}

		if (GameControllerScript.current.exitsReached == 3 && !allexits & (GameControllerScript.current.mode == "hard") && FindObjectOfType<PlayerStats>().data.beatHardMode)
		{
			RaisePortal();
			allexits = true;
		}
	}

	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);

		if (GameControllerScript.current.finaleMode)
			this.wall.material = this.map;
	}

	public void Raise()
	{
		base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
	}

	public void LowerPortal()
	{
		if (portal != null)
			portal.transform.position = portal.transform.position - new Vector3(0f, 12f, 0f);
	}

	public void RaisePortal()
	{
		if (portal != null)
			portal.transform.position = portal.transform.position + new Vector3(0f, 12f, 0f);
	}
}
