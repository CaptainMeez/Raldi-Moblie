using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class InventoryScript : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public Image generalImage;
    public Image hoverImage;

    public Image[] images;

    public int itemBeingThrown;

    public bool throwOut;

    public bool secondDamnBugFixer = false;

    public void ToggleThrowOut(bool throwOut)
    {
        this.throwOut = throwOut;
    }

    public void Update()
    {
        int index = 0;
        
        foreach(Image image in images)
        {
            image.sprite = GameControllerScript.current.itemSprites[GameControllerScript.current.item[index]];

            index++;
        }

        generalImage.sprite = GameControllerScript.current.itemSprites[GameControllerScript.current.item[GameControllerScript.current.itemSelected]];
    }
    
    public void HoverOnOption(int itemID)
    {
        nameText.text = GameControllerScript.current.itemNames[GameControllerScript.current.item[itemID]];
        descriptionText.text = GameControllerScript.current.itemDescriptions[GameControllerScript.current.item[itemID]];

        hoverImage.sprite = GameControllerScript.current.itemSprites[GameControllerScript.current.item[itemID]];
    }
}
