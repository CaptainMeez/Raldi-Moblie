using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class KidnapTextScript : MonoBehaviour
{
	private void Start()
	{
        this.text = base.GetComponent<TMP_Text>();
    }

	private void Update()
	{
		if (this.vanman.kidnapTime > 0f)
		{
			this.text.text = "You have been Kidnapped! \n" + Mathf.CeilToInt(this.vanman.kidnapTime) + " seconds remain!";
		}
		else
		{
			this.text.text = string.Empty;
		}
	}

	// Token: 0x0400002B RID: 43
	public PlaytimeScript vanman;

	// Token: 0x0400002C RID: 44
	private TMP_Text text;
}
