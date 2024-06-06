using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastAttack02 : MonoBehaviour
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
                GameObject proj = GameObject.Instantiate<GameObject>(beastPrefab, new Vector3(spawn.position.x,soul.position.y + UnityEngine.Random.Range(-10,10),spawn.position.z), spawn.rotation, transform);
                proj.GetComponent<BeastAttack01Projectile>().soul = soul;
                proj.GetComponent<Transform>().eulerAngles = new Vector3(0,-90,0);
                yield return new WaitForSeconds(0.25f);
            }
        }

        StartCoroutine(Attack());
    }

    public void Reset()
    {
        foreach(Transform child in base.transform)
        {
            if (child.name.ToLower() != "new sprite")
                GameObject.Destroy(child.gameObject);
        }
    }
}
