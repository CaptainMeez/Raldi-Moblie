using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealBillboard : MonoBehaviour
{
	private void Start()
	{
		this.m_Camera = Camera.main;

		defaultVector = base.transform.eulerAngles;
	}

	private void LateUpdate()
	{
		base.transform.LookAt(m_Camera.transform.position);
		base.transform.eulerAngles += modifier;
		base.transform.eulerAngles = new Vector3(defaultVector.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
	}

	private Vector3 defaultVector;

	private Camera m_Camera;
	public Vector3 modifier;
}
