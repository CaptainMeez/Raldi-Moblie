using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class ItemImageScript : MonoBehaviour
{
	// Token: 0x06000046 RID: 70 RVA: 0x00003318 File Offset: 0x00001718
	private void Update()
	{
		if (this.gs != null)
		{
			Sprite texture = this.gs.itemSlot[this.gs.itemSelected].sprite;
			if (texture == this.blankSprite)
			{
				this.sprite.sprite = noItemSprite;
			}
			else
			{
				this.sprite.sprite = texture;
			}
		}
		else
		{
			this.sprite.sprite = noItemSprite;
		}
	}

	// Token: 0x04000054 RID: 84
	public Image sprite;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private Sprite noItemSprite;

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private Texture blankSprite;

	// Token: 0x04000057 RID: 87
	public GameControllerScript gs;
}
