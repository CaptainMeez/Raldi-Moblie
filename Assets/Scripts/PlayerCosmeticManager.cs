using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCosmeticManager : MonoBehaviour
{
    public MeshRenderer[] shirt;
    public Shader defaultS;
    public Image healthbarcolor;

    public void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();
    }

    public void Update()
    {
        float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
        float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
        float b = FindObjectOfType<PlayerStats>().data.playercolor_b;

        Material playerShirt = new Material(defaultS);
        playerShirt.color = new Color(r / 255, g / 255, b / 255);

        if (healthbarcolor != null)
            healthbarcolor.color =new Color(r / 255, g / 255, b / 255, 0.5f);
            
        foreach(MeshRenderer renderer in shirt)
        {
            renderer.material = playerShirt;
        }
    }
}
