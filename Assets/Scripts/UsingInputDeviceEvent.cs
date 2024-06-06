using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Rewired;

public class UsingInputDeviceEvent : MonoBehaviour
{
    private Player input;
    private bool controller = false;

    public UnityEvent onKeyboardInput;
    public UnityEvent onControllerInput;

    void Awake()
    {
        input = ReInput.players.GetPlayer(0);

        if (input.controllers.GetLastActiveController().type == ControllerType.Joystick)
        {
            controller = true;
            onControllerInput.Invoke();
        } 
        else if (input.controllers.GetLastActiveController().type == ControllerType.Keyboard)
        {
            controller = false;
            onKeyboardInput.Invoke();
        }
    }

    void Update()
    {
        if (!controller)
        {
            if (input.controllers.GetLastActiveController().type == ControllerType.Joystick)
            {
                controller = true;
                onControllerInput.Invoke();
            }
        }
        else if (controller)
        {
            if (input.controllers.GetLastActiveController().type == ControllerType.Keyboard)
            {
                controller = false;
                onKeyboardInput.Invoke();
            }
        }
    }
}
