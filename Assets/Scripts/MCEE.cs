using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MCEE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var windowPtr = FindWindow(null, "Raldi's Crackhouse");
        SetWindowText(windowPtr, "Minecraft 1.19");

        Screen.SetResolution(854, 480, FullScreenMode.Windowed);
    }


    //Import the following.
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);
}
