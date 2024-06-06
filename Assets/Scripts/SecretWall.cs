using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public Material hitMat;
    public AudioClip sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
	{
		if (other.tag == "BSODA")
		{
            other.transform.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().material = hitMat;
            GameControllerScript.current.audioDevice.PlayOneShot(sfx);
        }
    }
}
