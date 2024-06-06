using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOneShot : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject.Destroy(gameObject);
    }
}
