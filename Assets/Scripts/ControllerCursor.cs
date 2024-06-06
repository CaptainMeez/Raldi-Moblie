using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerCursor : MonoBehaviour
{
    /*public bool allowControllerMovement = false;

    void Update()
    {
        if ((Gamepad.current.leftStick.ReadValue().x != 0 || Gamepad.current.leftStick.ReadValue().y != 0) && allowControllerMovement && Application.isFocused)
		{
            Win32.POINT pos = Win32.GetCursorPosition();

            Win32.SetCursorPos(pos.x + Mathf.RoundToInt(Gamepad.current.leftStick.ReadValue().x) * 15, pos.y + Mathf.RoundToInt(-Gamepad.current.leftStick.ReadValue().y) * 15);

            if (Gamepad.current.rightTrigger.wasPressedThisFrame)
            {    
                StartCoroutine(SignalClick());
            }   
		}
    }

    IEnumerator SignalClick()
    {
        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
        yield return null;
        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp); 
    }*/
}
