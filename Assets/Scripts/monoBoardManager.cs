using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monoBoardManager : MonoBehaviour
{
    public bool[] checkpoints = new bool[4];
    public GameControllerScript gc;
    public GameObject objects;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("passive_income") == 1)
            objects.SetActive(true);
        else
            objects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Passed()
    {
        gc.AddMoney(1);
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i] = false;
        }
    }
    public bool Check()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (!checkpoints[i])
                return false;
        }
        return true;
    }
}
