using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000018 RID: 24
public class MoveableWindow : MonoBehaviour, IDragHandler
{
	public void OnDrag(PointerEventData eventData)
    {
        base.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
}
