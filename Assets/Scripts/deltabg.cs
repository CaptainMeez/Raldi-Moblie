using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deltabg : MonoBehaviour
{
    public Transform the;
    public float movementx;
    public float movementy;
    public float ogx;
    public float ogy;
    private float distancetoogx = 0;
    private float distancetoogy = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        the.localPosition += new Vector3(Time.deltaTime*movementx,Time.deltaTime*movementy,0);
        distancetoogx = Mathf.Abs(Mathf.Abs(the.localPosition.x) - Mathf.Abs(ogx));
        distancetoogy = Mathf.Abs(Mathf.Abs(the.localPosition.y) - Mathf.Abs(ogy));
        //print(distancetoogy);
        if (distancetoogx > 10) {
            the.localPosition = new Vector3(ogx,the.localPosition.y,the.localPosition.z);
        }
        if (distancetoogy > 10) {
            the.localPosition = new Vector3(the.localPosition.x,ogy,the.localPosition.z);
        }
    }
}
