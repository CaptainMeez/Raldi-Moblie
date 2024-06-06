using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monoBoardScript : MonoBehaviour
{
    public bool isCheckpointSquare;
    public int checkpointid;
    public monoBoardManager mb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckpointSquare)
        {
            if (mb.checkpoints[checkpointid])
                gameObject.GetComponent<Renderer>().material.color = Color.gray;
            else
                gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isCheckpointSquare)
            {
                if (!mb.checkpoints[checkpointid])
                {
                    if (isCheckpointSquare)
                    {
                        mb.checkpoints[checkpointid] = true;
                        mb.gc.audioDevice.PlayOneShot(mb.gc.aud_Exit);
                    }
                }
            } 
            else
            {
                if (mb.Check())
                    mb.Passed();
            }
        }
    }

}
