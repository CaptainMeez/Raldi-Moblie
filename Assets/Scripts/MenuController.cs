using System;
using UnityEngine;
using UnityEngine.UI;
// Token: 0x02000016 RID: 22
public class MenuController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && this.back != null)
        {
            this.back.SetActive(true);
            base.gameObject.SetActive(false);
        }
    }

    public GameObject back;
}