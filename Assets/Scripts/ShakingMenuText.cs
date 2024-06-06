using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingMenuText : MonoBehaviour
{
    private Vector3 originalPositon;
    private bool canDo = false;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        originalPositon = transform.localPosition;
        canDo = true;
    }

    void Update()
    {
        if (canDo)
            transform.localPosition = new Vector3(originalPositon.x + UnityEngine.Random.Range(-2, 2), originalPositon.y + UnityEngine.Random.Range(-2, 2), originalPositon.z);
    }
}
