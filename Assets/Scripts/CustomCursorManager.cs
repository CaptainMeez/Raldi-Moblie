using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CustomCursorManager : MonoBehaviour
{
    public static CustomCursorManager current;

    public Image cursor;
    private Sprite normal;
    public Sprite pressed;

    private void Start()
    {
        current = this;
        normal = cursor.sprite;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (cursor != null)
        {
            if (Input.GetMouseButton(0))
                cursor.sprite = pressed;
            else
                cursor.sprite = normal;

            cursor.transform.position = Input.mousePosition;
        }
    }
}
