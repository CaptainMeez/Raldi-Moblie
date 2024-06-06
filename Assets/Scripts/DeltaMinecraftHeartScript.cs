using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeltaMinecraftHeartScript : MonoBehaviour
{
    public BattleControllerScript bcs;
    public Image heart;
    public int heartid = 0;
    public Sprite empty;
    public Sprite half;
    public Sprite full;
    public float calchearthp = 0;
    public float defaulty = 0;
    public float shakecounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        defaulty = heart.transform.localPosition.y;
    } 

    // Update is called once per frame
    void Update()
    {
        calchearthp = Mathf.Ceil((bcs.playerhp / bcs.playermaxhp)*20)-1;
        if (calchearthp < heartid * 2) {
            heart.sprite = empty;
        }
        else if (calchearthp == heartid * 2) {
            heart.sprite = half;
        }
        else {
            heart.sprite = full;
        }
        if (calchearthp < 4) {
            if (shakecounter <= 0) {
                heart.transform.localPosition = new Vector3(heart.transform.localPosition.x,defaulty + Random.Range(-1,1) * 2, heart.transform.localPosition.z);
                shakecounter = 0.05f;
            }
            shakecounter -= Time.deltaTime;
        } else {
            heart.transform.localPosition = new Vector3(heart.transform.localPosition.x,defaulty, heart.transform.localPosition.z);
        }

       // this.Sprite = 2
    }
}
