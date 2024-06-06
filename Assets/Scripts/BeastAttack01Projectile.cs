using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastAttack01Projectile : MonoBehaviour
{
    private Rigidbody rigidbody;
    public Transform soul;
    private float ogzpos;
    public bool targetsoul;

    void Start()
    {
        ogzpos = base.transform.position.z;
        rigidbody = base.GetComponent<Rigidbody>();
        if (targetsoul)
            base.transform.LookAt(soul.position);
    }

    void Update()
    {
        rigidbody.velocity = base.transform.forward * 40;
        rigidbody.position = new Vector3(rigidbody.position.x,rigidbody.position.y,ogzpos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Soul")
        {
            FindObjectOfType<BattleControllerScript>().TakeDamage(FindObjectOfType<BattleControllerScript>().bossDmg, gameObject);
        }
    }
}
