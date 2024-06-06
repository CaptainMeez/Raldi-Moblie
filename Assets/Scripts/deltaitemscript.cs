using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;

public class deltaitemscript : MonoBehaviour
{
    public SelectHud parent;
    public int selectedButton;
    public bool canInteract = false;
    public TextMeshProUGUI[] buttons;
    public int[] BannedItems;
    public int[] noConsume;
    	public string[] itemNames = new string[]
	{
		"",
		"Zesty Bar",
		"Door Lock",
		"Keys",
		"BSODA",
		"Quarter",
		"Tape",
		"Clock",
		"NoSquee",
		"Glock",
		"Boots",
		"Poop Meter",
		"Medkit",
		"Credit Card",
		"iPhone",
		"Acid Gas",
		"Hammer",
		"15 Energy",
		"Crackpipe",
		"Jail Card",
		"Monkenator",
		"Potato",
		"Ender Pearl"
	};
    public int[] inventory = new int[5];
    public GameObject attackUI;
    public GameControllerScript gc;
    public int inventorytype = 0;
    private void Start()
    {
        gc = GameControllerScript.current;
        for (int i = 0; i < 5; i++)
        {
            if (inventorytype == 0)
                inventory[i] = gc.item[i];
            else if (inventorytype == 1)
                inventory[i] = UnityEngine.Random.Range(0, itemNames.Length - 1);
            else if (inventorytype == 2)
            {
                inventory[0] = 2;
                inventory[1] = 22;
                inventory[2] = 12;
                inventory[3] = 14;
                inventory[4] = 4;
            }
            buttons[i].text = "* " + itemNames[inventory[i]];
        }
    }
    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Select(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Select(1);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
                EnterSubMenu();
        }
    }

    private void Select(int change)
    {
        parent.auSource.PlayOneShot(parent.impact);

        selectedButton += change;

        if (selectedButton > buttons.Length - 1)
            selectedButton = 0;
        else if (selectedButton < 0)
            selectedButton = buttons.Length - 1;

        for(int i = 0; i < buttons.Length; i++)
        {
            if (selectedButton == i)
            {
                buttons[i].color = Color.white;
            }
            else
            {
                buttons[i].color = Color.gray;
            }
        }
    }
    
    public void EnterSubMenu()
    {
        if (!BannedItems.Contains(inventory[selectedButton]))
        {
            canInteract = false;
            parent.auSource.PlayOneShot(parent.enter);
            
            FindObjectOfType<BattleControllerScript>().ItemSelect(inventory[selectedButton], gameObject);
            if (!noConsume.Contains(inventory[selectedButton]))
            {
                inventory[selectedButton] = 0;
                buttons[selectedButton].text = "* " + itemNames[inventory[selectedButton]];
                for (int i = 0; i < 5; i++)
                {
                    gc.item[i] = inventory[i];
                    print(inventory[i]);
                    gc.itemSlot[i].sprite = gc.itemSprites[inventory[i]];
                }
            }
        } else {
            parent.auSource.PlayOneShot(parent.refuse);
        }
    }

    public void Return()
    {
        attackUI.SetActive(false);
        attackUI.GetComponent<ThrowAttackScript>().canInteract = false;
        canInteract = true;
        FindObjectOfType<SelectHud>().ExitSubMenu();
    }
}
