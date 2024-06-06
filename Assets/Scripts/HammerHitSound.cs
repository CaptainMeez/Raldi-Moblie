using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitSound : MonoBehaviour
{
    public AudioClip hit;

    private IEnumerator Start()
    {
        GetComponent<AudioSource>().PlayOneShot(hit);
        yield return new WaitForSeconds(hit.length);
        GameObject.Destroy(this.gameObject);
    }
}
