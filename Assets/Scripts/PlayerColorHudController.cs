using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorHudController : MonoBehaviour
{
    public Sprite mono;
    public Image slots;
    public Image basicSlots;
    public Image[] subImages;
    public Sprite slotsMono;
    public Sprite basicSlotMono;

    void Start()
    {
        if (PlayerPrefs.GetFloat("HUDPlayerColor") == 1)
        {
            FindObjectOfType<PlayerStats>().TryLoad();

            float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
            float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
            float b = FindObjectOfType<PlayerStats>().data.playercolor_b;

            GetComponent<Image>().color = new Color(r / 255, g / 255, b / 255, 1);
            GetComponent<Image>().sprite = mono;

            if (slots != null)
            {
                slots.sprite = slotsMono;
                slots.color = new Color(r / 255, g / 255, b / 255, 1);

                if (PlayerPrefs.GetFloat("back_to_the_basics") == 1)
                {
                    basicSlots.sprite = basicSlotMono;
                    basicSlots.color = new Color(r / 255, g / 255, b / 255, 1);
                }
            }

            foreach(Image img in subImages)
            {
                img.color = new Color(r / 255, g / 255, b / 255, 1);
                img.sprite = mono;
            }
        }
    }
}
