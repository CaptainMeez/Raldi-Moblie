using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkinCharacter : MonoBehaviour
{
    private Animator canimation;

    private void Start()
    {
        canimation = GetComponent<Animator>();
    }

    public void Sing(SingDirection dir)
    {
        switch (dir)
        {
            case SingDirection.Left: canimation.SetTrigger("Left"); break;
            case SingDirection.Down: canimation.SetTrigger("Down"); break;
            case SingDirection.Up: canimation.SetTrigger("Up"); break;
            case SingDirection.Right: canimation.SetTrigger("Right"); break;
        }
    }
}

public enum SingDirection {Left, Down, Up, Right}
