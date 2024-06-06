using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaldiInput
{
    public static bool GetInteractDown()
    {
        return (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E));
    }

    public static bool GetInteract()
    {
        return (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E));
    }

    public static bool GetInteractUp()
    {
        return (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E));
    }

    public static bool GetUseDown()
    {
        return (Input.GetMouseButtonDown(1));
    }
    public static bool GetUse()
    {
        return (Input.GetMouseButton(1));
    }
    public static bool GetUseUp()
    {
        return (Input.GetMouseButtonUp(1));
    }
}
