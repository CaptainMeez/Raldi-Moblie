using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableSlot : MonoBehaviour
{
    public int id;
    private Vector3 startPosition;
    public Canvas myCanvas;

    public int indexInArray;

    private void Start()
    {
        startPosition = transform.position;
    }

    bool dropped = false;
    bool mouseOver = false;

    bool bugFixer = false;

    public void ToggleMouseOver(bool mouseOver)
    {
        this.mouseOver = mouseOver;
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0) && (mouseOver || bugFixer))
        {
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out movePos);

            if (!FindObjectOfType<InventoryScript>().secondDamnBugFixer)
                FindObjectOfType<InventoryScript>().itemBeingThrown = indexInArray;

            transform.position = myCanvas.transform.TransformPoint(movePos);

            bugFixer = true;
            dropped = true;
            FindObjectOfType<InventoryScript>().secondDamnBugFixer = true;
        }
        else if (!mouseOver || !bugFixer || FindObjectOfType<InventoryScript>().itemBeingThrown != indexInArray)
        {
            transform.position = startPosition;

            if (FindObjectOfType<InventoryScript>().throwOut && dropped)
            {
                FindObjectOfType<InventoryScript>().throwOut = false;

                FindObjectOfType<PlayerScript>().DropItem(id);

                FindObjectOfType<InventoryScript>().secondDamnBugFixer = false;
            }

            bugFixer = false;
            dropped = false;
        }
    }
}