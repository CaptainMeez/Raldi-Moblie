using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasAreaScript : MonoBehaviour
{
    public float duration;
    
    public void Update()
    {
        duration -= Time.deltaTime;

        if (duration < 0)
        {
            if (gameObject.GetComponent<BoxCollider>().bounds.Intersects(FindObjectOfType<PlayerScript>().GetComponent<CapsuleCollider>().bounds))
            {
                FindObjectOfType<PlayerScript>().inGas = false;
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}