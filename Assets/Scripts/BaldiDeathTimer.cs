using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class BaldiDeathTimer : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject container;

    private void Update()
    {
        if (GameControllerScript.current.baldi.isDead)
        {
            text.text = Mathf.Ceil(GameControllerScript.current.baldi.deadTime).ToString();

            if (!container.activeInHierarchy)
                container.SetActive(true);
        }   
        else
        {
            text.text = "";

            if (container.activeInHierarchy)
                container.SetActive(false);
        }    
    }
}
