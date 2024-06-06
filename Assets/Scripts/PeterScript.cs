using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterScript : MonoBehaviour
{
    public AudioClip[] lines;
    public Sprite lois;
    public AudioClip loisLine;
    public SpriteRenderer image;

    void Awake()
    {
        AudioClip clip = lines[UnityEngine.Random.Range(0, lines.Length)];

        if (Mathf.RoundToInt(UnityEngine.Random.Range(0, 1000)) == 0)
        {
            image.sprite = lois;
            clip = loisLine;
        }

        base.GetComponent<AudioSource>().PlayOneShot(clip);

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(clip.length);
            GameObject.Destroy(this.gameObject);
        }

        StartCoroutine(WaitTime());
    }
}
