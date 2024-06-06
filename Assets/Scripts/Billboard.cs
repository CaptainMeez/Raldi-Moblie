using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private void LateUpdate()
	{
		if (Time.timeScale != 0)
			base.transform.LookAt(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
	}
}