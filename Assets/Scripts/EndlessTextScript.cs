using System;
using TMPro;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class EndlessTextScript : MonoBehaviour
{
    // Token: 0x06000031 RID: 49 RVA: 0x00002B8C File Offset: 0x00000F8C
    private void Start()
    {
        FindObjectOfType<PlayerStats>().TryLoad();
        
        this.text.text = string.Concat(new object[]
        {
            this.text.text,
            "\nPB: ",
            FindObjectOfType<PlayerStats>().data.highBooks,
            " Notebooks",
            " (",
            FindObjectOfType<PlayerStats>().data.modifhighBooks,
            " with modifiers)"
        });
    }

    // Token: 0x04000032 RID: 50
    public TMP_Text text;
}
