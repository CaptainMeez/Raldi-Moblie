using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopArea : MonoBehaviour
{
    void Start()
    {
        IEnumerator WaitToDisable()
        {
            yield return new WaitForSeconds(25);
            GameObject.Destroy(base.gameObject);
        }

        StartCoroutine(WaitToDisable());
    }
}
