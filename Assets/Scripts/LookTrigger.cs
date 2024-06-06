using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTrigger : MonoBehaviour
{
    public bool allowTrigger = true;
    public UnityEngine.Events.UnityEvent onLook;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

		if (Physics.Raycast(ray, out raycastHit) && raycastHit.transform == base.transform && allowTrigger)
		{
            allowTrigger = false;
            onLook.Invoke();
        }
    }

    public void EnableTrigger()
    {
        allowTrigger = true;
    }
}
