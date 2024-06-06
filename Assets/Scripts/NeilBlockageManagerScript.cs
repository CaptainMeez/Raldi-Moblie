using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilBlockageManagerScript : MonoBehaviour
{
    private NeilBlockageScript[] walls = new NeilBlockageScript[3];
    public int disabledWall;
    public float wallcooldown;
    // Start is called before the first frame update
    void Start()
    {
        walls[0] = gameObject.transform.GetChild(0).GetComponent<NeilBlockageScript>();
        walls[1] = gameObject.transform.GetChild(1).GetComponent<NeilBlockageScript>();
        walls[2] = gameObject.transform.GetChild(2).GetComponent<NeilBlockageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wallcooldown > 0)
            wallcooldown -= Time.deltaTime;
        else
            wallcooldown = -1;
    }
    public void InitializeWalls()
    {
        wallcooldown = 5f;
        foreach(NeilBlockageScript wall in walls)
        {
            wall.Reset();
        }  
    }
    public void GenerateWalls()
    {
        int walltoblock = 0;
        foreach(NeilBlockageScript wall in walls)
        {
            if (wall.skipthiswall)
            {
                disabledWall = wall.wallid;
            }
        }
        walltoblock = disabledWall;
        while (walltoblock == disabledWall)
        {
            walltoblock = Random.Range(0,3);
            if (walltoblock > 2)
            {
                walltoblock = 2;
            }
        }
        print("wall id to block: " + walltoblock);
        print(walltoblock);
        foreach(NeilBlockageScript wall in walls)
        {
            if (wall.wallid == walltoblock)
            {
                wall.BuildWall();
            }
        }
    }
}
