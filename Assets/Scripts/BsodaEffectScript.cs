using System;
using UnityEngine;
using UnityEngine.AI;

public class BsodaEffectScript : MonoBehaviour
{
	private NavMeshAgent agent;

	private float failSave;

	private Vector3 otherVelocity;

	private bool inBsoda;
	
	private void Start()
	{
		agent = base.GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (inBsoda)
			agent.velocity = otherVelocity;
		if (failSave > 0f)
			failSave -= Time.deltaTime;
		else
			inBsoda = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "BSODA")
		{
			inBsoda = true;
			otherVelocity = other.GetComponent<Rigidbody>().velocity;
			failSave = 1f;
		}
		else if (other.transform.name == "Gotta Sweep")
		{
			inBsoda = true;
			otherVelocity = base.transform.forward * agent.speed * 0.1f + other.GetComponent<NavMeshAgent>().velocity;
			failSave = 1f;
		}

		else if (other.name == "PintoBullet" & gameObject.name != "Beans")
		{
			if (other.transform.position.y <= 0f)
			{
				other.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
				return;
			}

			other.GetComponentInChildren<SpriteRenderer>().sprite = BeansScript.spriteNPCGum;
			other.GetComponent<BsodaSparyScript>().speed = 0f;
			UnityEngine.Object.FindObjectOfType<BeansScript>().SorryNPC();
			other.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
			otherVelocity = base.transform.forward * agent.speed * 0.05f;
			UnityEngine.Object.Destroy(other.gameObject, 10f);
			inBsoda = true;
			failSave = 10f;
		}
	}

	private void OnTriggerExit()
	{
		inBsoda = false;
	}
}
