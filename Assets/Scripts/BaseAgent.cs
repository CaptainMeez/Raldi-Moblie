using System;
using UnityEngine;
using UnityEngine.AI;

public class BaseAgent : MonoBehaviour
{
	private NavMeshAgent agent;
	private AILocationSelectorScript wanderer;

	public bool db;
	public float coolDown;

	private void Awake()
	{
		wanderer = FindObjectOfType<AILocationSelectorScript>();
		agent = GetComponent<NavMeshAgent>();

		Wander();
	}

	private void Update()
	{
		if (coolDown > 0f)
			coolDown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
		RaycastHit raycastHit;

		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player")
		{
			db = true;
			TargetPlayer();
		}
		else
		{
			db = false;

			if (agent.velocity.magnitude <= 1f & coolDown <= 0f)
				Wander();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderer.transform.position);
		coolDown = 1f;
	}

	private void TargetPlayer()
	{
		agent.SetDestination(GameControllerScript.current.player.transform.position);
		coolDown = 1f;
	}
}
