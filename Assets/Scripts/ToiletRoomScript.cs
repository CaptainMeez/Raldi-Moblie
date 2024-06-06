using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletRoomScript : MonoBehaviour
{
    public Billboard toiletSprite;
    public GameObject brokenSound;
    public Transform player;
    public Animator camAnim;
    public GameObject hell;
    public CharacterController cc;
    public CameraScript playerCam;
    public GameObject world1;
    public GameObject world2;
    public Transform standPoint;

    void Start()
    {
        toiletSprite.enabled = false;
    }

    public void Flush()
    {
        IEnumerator WaitTime()
        {
            cc.enabled = false;
            player.LookAt(toiletSprite.transform.position);
            player.gameObject.transform.position = standPoint.position;

            yield return new WaitForSeconds(4f);

            toiletSprite.enabled = true;
            brokenSound.SetActive(true);
            
            yield return new WaitForSeconds(4f);

            hell.SetActive(true);
            playerCam.enabled = false;
            camAnim.enabled = true;

            yield return new WaitForSeconds(4f);
            world1.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            yield return new WaitForSeconds(2f);
            world2.SetActive(true);
        }

        StartCoroutine(WaitTime());
    }
}
