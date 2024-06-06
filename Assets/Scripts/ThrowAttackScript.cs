using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowAttackScript : MonoBehaviour
{
    public SelectHud parent;
    public int selectedButton;
    public bool canInteract = false;
    public Image bar;
    public Image judgementbar;
    public float judgementpos = 0;
    private Vector3 ogbarpos;
    private float dealdamagepercent;
    public float dealdamage;
    private bool attacked;
    public Animator damagetextanimator;
    public Animator playerAnimator;
    public TextMeshProUGUI damagetext;
    public AudioSource funny;
    public AudioClip whoop;
    public AudioClip crit;
    public AudioClip hit;

    void Start()
    {
        ogbarpos = bar.transform.position;
    }

    public void Reset(GameObject menu)
    {
        menu.SetActive(false);
        
        bar.transform.localPosition = new Vector3(-28.3f, 33.33f, 0);
        attacked = false;
    }


    void Update()
    {
        judgementpos = Mathf.Sqrt(Mathf.Pow(judgementbar.transform.position.x - bar.transform.position.x, 2)); 
        if (canInteract) 
        {
            if (!attacked)
                bar.transform.position -= new Vector3(Time.deltaTime * 500,0,0);

            if (bar.transform.position.x < judgementbar.transform.position.x - 50) 
            {
                dealdamage = 0;
                attacked = true;
                StartCoroutine(damageanim());
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) 
            {
                attacked = true;
                if (FindObjectOfType<BattleControllerScript>().playeratk != 9999)
                {
                    if ((180 - Mathf.Round(judgementpos)) > 0)
                        dealdamagepercent = (180 - Mathf.Round(judgementpos));
                    else
                        dealdamagepercent = 0;
                    if (dealdamagepercent < 170 || dealdamagepercent > 185)
                        dealdamage = Mathf.Round(((dealdamagepercent/180)*FindObjectOfType<BattleControllerScript>().playeratk)/2);
                    else
                        dealdamage = FindObjectOfType<BattleControllerScript>().playeratk;
                    FindObjectOfType<BattleControllerScript>().beasthp -= dealdamage;
                    if (dealdamage != 0)
                        damagetext.text = "" + dealdamage;
                    else
                        damagetext.text = "MISS";
                } else {
                    dealdamage = 9999;
                    damagetext.text = "" + dealdamage;
                    FindObjectOfType<BattleControllerScript>().beasthp -= dealdamage;
                }
                StartCoroutine(damageanim());
            }
        }   
    }

    IEnumerator damageanim()
    {
        playerAnimator.SetTrigger("Attack");
        canInteract = false;
        funny.PlayOneShot(whoop);
        yield return new WaitForSeconds(whoop.length);
        if (dealdamagepercent < 170 || dealdamagepercent > 185) {
            if (dealdamage != 0)
                funny.PlayOneShot(hit);
        } else {
            funny.PlayOneShot(hit);
            funny.PlayOneShot(crit);
        }
        if (FindObjectOfType<BattleControllerScript>().fight == 0)
            damagetextanimator.SetTrigger("Damage");
        else if (FindObjectOfType<BattleControllerScript>().fight == 1)
            damagetextanimator.SetTrigger("Centered Damage");

        yield return new WaitForSeconds(0.4f);

        FindObjectOfType<BattleControllerScript>().Attack();
    }
}