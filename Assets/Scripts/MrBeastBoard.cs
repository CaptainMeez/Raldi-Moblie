using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeastBoard : MonoBehaviour
{
    private void Start()
	{
		this.m_Camera = GameObject.FindGameObjectWithTag("SoulCamera").transform;
	}

	private void LateUpdate()
	{
		base.transform.LookAt(m_Camera.position);
		base.transform.eulerAngles += modifier;
	}

	private Vector3 defaultVector;

	private Transform m_Camera;
	public Vector3 modifier;
}
