using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class RaldiInputManager : MonoBehaviour
{
    public static RaldiInputManager current;
    private Player input;
    
    private void Start()
    {
        current = this;
        input = ReInput.players.GetPlayer(0);
    }

    public bool GetInteractDown() { return input.GetButtonDown("Interact"); }
    public bool GetInteract() { return input.GetButton("Interact"); }
    public bool GetInteractUp() { return input.GetButtonUp("Interact"); }

    public bool GetUseDown() { return input.GetButtonDown("Use");}
    public bool GetUse() { return input.GetButton("Use");}
    public bool GetUseUp() { return input.GetButtonUp("Use"); }

    public bool SlotsLeft() { return input.GetButtonDown("SlotsLeft"); }
    public bool SlotsRight() { return input.GetButtonDown("SlotsRight"); }
}
