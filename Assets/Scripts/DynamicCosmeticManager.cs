using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCosmeticManager : MonoBehaviour
{
    public ColorPicker picker;
    public MeshRenderer[] shirt;
    public Shader defaultS;
    public Material playerShirt;

    public void Start()
    {
        playerShirt = new Material(defaultS);

        foreach(MeshRenderer renderer in shirt)
        {
            renderer.material = playerShirt;
        }
    }
}
