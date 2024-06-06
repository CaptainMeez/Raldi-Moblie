using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadUtil
{
    public static bool IsControllerConnected()
    {
        return Gamepad.current != null;
    }
}
