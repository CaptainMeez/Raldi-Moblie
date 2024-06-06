using System;
using System.Collections.Generic;
using UnityEngine;

public class AILocationSelectorScript : MonoBehaviour
{
	private int id;
	private List<Transform> normPoints = new List<Transform>();
	private List<Transform> hallPoints = new List<Transform>();
	
	public Transform points;

	private void Start()
	{
		foreach (Transform child in points)
		{
			if (child.gameObject.name.ToLower().Contains("wanderpoint"))
			{
				normPoints.Add(child);

				if (!child.gameObject.name.ToLower().Contains("room"))
					hallPoints.Add(child);
			}
		}
	}

	public void GetNewTarget()
	{
		id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, normPoints.Count - 1));
		transform.position = normPoints[this.id].position;
		GameControllerScript.current.RandomizedEvents();
	}

	public void GetNewTargetHallway()
	{
		id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, hallPoints.Count - 1));
		transform.position = hallPoints[this.id].position;
		GameControllerScript.current.RandomizedEvents();
	}

	public void QuarterExclusive()
	{
		id = Mathf.RoundToInt(UnityEngine.Random.Range(1f, hallPoints.Count - 1));
		transform.position = hallPoints[this.id].position;
	}
}
