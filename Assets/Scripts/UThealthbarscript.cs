using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UThealthbarscript : MonoBehaviour
{
    public UTControllerScript uc;
    private float hpsize = 0.01254736f;
    public GameObject bghp;
    public GameObject yellowhp;
    public GameObject krhp;
    public GameObject hpamounttext;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bghp.transform.localScale = new Vector3(hpsize * uc.maxHP, bghp.transform.localScale.y, bghp.transform.localScale.z);
        hpamounttext.transform.localPosition = new Vector3(55.4f + ((1.88f * 2/3) * (uc.maxHP + 10)), hpamounttext.transform.localPosition.y, hpamounttext.transform.localPosition.z);

        if (uc.krEnabled && uc.kr > 0)
            yellowhp.transform.localScale = new Vector3(hpsize * (uc.hp - uc.kr), yellowhp.transform.localScale.y, yellowhp.transform.localScale.z);
        else
            yellowhp.transform.localScale = new Vector3(hpsize * uc.hp, yellowhp.transform.localScale.y, yellowhp.transform.localScale.z);
    }
}
