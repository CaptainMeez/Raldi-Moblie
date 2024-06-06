using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BasicButtonScript : MonoBehaviour
{
	private Button button;
	public GameObject screen;

	private void Start()
	{
		button = base.GetComponent<Button>();
		button.onClick.AddListener(new UnityAction(OpenScreen));
	}

	private void OpenScreen()
	{
		this.screen.SetActive(true);
	}
}
