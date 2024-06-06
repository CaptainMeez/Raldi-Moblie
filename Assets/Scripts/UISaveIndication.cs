using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UISaveIndication : MonoBehaviour
{
    private IEnumerator Start()
    {
        Object.DontDestroyOnLoad(gameObject);
        yield return new WaitForSecondsRealtime(1.5f);
        GameObject.Destroy(gameObject);
    }
}
