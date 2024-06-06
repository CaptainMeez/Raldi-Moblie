using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableNeilHallwayTriggerScript : MonoBehaviour
{
    private NeilBlockageScript parentwall;
    private NeilBlockageManagerScript manager;
    public bool enterorexit;
    // Start is called before the first frame update
    void Start()
    {
        parentwall = gameObject.transform.parent.gameObject.GetComponent<NeilBlockageScript>();
        manager = parentwall.gameObject.transform.parent.gameObject.GetComponent<NeilBlockageManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enterorexit)
        {
            gameObject.GetComponent<MeshCollider>().enabled = !parentwall.skipthiswall && !(manager.wallcooldown > 0);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        } else {   
            gameObject.GetComponent<MeshCollider>().enabled = !parentwall.skipthiswall;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            print(enterorexit);
            if (enterorexit)
            {
                print("in");
                parentwall.Interaction();
            } else {
                print("out");
                manager.InitializeWalls();
            }
        }
    }
}
