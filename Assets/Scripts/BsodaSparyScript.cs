using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class BsodaSparyScript : MonoBehaviour
{
	public GameControllerScript controller;

	public float speed;
	private float lifeSpan;

	private Rigidbody rb;

	public bool isbullet;

	private void Awake()
	{
		controller = GameControllerScript.current;
	}

	private void Start()
	{
		rb = base.GetComponent<Rigidbody>();
		rb.velocity = base.transform.forward * speed;
		lifeSpan = 30f;
	}

	private void Update()
	{
		rb.velocity = base.transform.forward * speed;
		lifeSpan -= Time.deltaTime;

		if (lifeSpan < 0f)
			UnityEngine.Object.Destroy(base.gameObject, 0f);

		if (isbullet && !(PlayerPrefs.GetFloat("sniper_beans") == 1)) // Snap to grid
		{
            var vec = transform.eulerAngles;
            vec.x = transform.eulerAngles.x;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = transform.eulerAngles.z;

            transform.eulerAngles = vec;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "BSODA" && isbullet)
		{
			if (FindObjectOfType<PlayerStats>().data.objectasks[7] == 0)
				FindObjectOfType<Objectasks>().CollectObjectask(7);

			GameJolt.API.Trophies.TryUnlock(182826);

			UnityEngine.Object.Destroy(base.gameObject, 0f);
		}
	}
}
