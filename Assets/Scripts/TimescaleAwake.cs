using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleAwake : MonoBehaviour
{
    public int scale;

    private void Awake()
    {
        Time.timeScale = scale;
    }
}
