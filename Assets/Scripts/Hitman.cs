using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitman : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent nav;

    public float killTime = 15f;

    void Awake()
    {
        if (PlayerPrefs.GetFloat("true_hitman") == 1)
            killTime = 30f;
    }
    void Update()
    {
        nav.SetDestination(GameControllerScript.current.baldi.transform.position);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "Baldi")
        {
            UnityEngine.Object.Destroy(this.gameObject);
            GameControllerScript.current.baldi.Kill(killTime);
        }
    }
}
