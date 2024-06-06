using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastAttack01 : MonoBehaviour
{
    public Transform spawn;
    public GameObject beastPrefab;
    public Transform soul;


    public void Attack()
    {
        IEnumerator Attack()
        {
            for (int i = 0; i < 30; i++) 
            {
                GameObject proj = GameObject.Instantiate<GameObject>(beastPrefab, spawn.position, spawn.rotation, transform);
                proj.GetComponent<BeastAttack01Projectile>().soul = soul;
                proj.GetComponent<BeastAttack01Projectile>().targetsoul = true;
                yield return new WaitForSeconds(0.5f);
            }
        }

        StartCoroutine(Attack());
    }

    public void Reset()
    {
        foreach(Transform child in base.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
