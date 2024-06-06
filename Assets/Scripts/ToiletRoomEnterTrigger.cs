using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToiletRoomEnterTrigger : MonoBehaviour
{
    public GameObject hudAnimation;
    public CharacterController cc;
    public AudioSource source;
    public AudioClip transition;
    
    public void Trigger()
    {
        if (!GameControllerScript.current.neilMode)
        {
            cc.enabled = false;
            hudAnimation.SetActive(true);

            source.PlayOneShot(transition);
            
            IEnumerator WaitTime()
            {
                yield return new WaitForSeconds(6.5f);

                SceneManager.LoadSceneAsync("ToiletRoom");
            }

            StartCoroutine(WaitTime());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            Trigger();
        }
    }
}
