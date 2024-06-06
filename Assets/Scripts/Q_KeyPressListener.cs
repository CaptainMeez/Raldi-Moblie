using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Q_KeyPressListener : MonoBehaviour
{
    public UnityEvent onKeyPress;
    public KeyCode key;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            onKeyPress.Invoke();
        }
    }
}
