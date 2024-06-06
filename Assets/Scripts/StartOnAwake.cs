using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class StartOnAwake : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Stuff());
    }

    IEnumerator Stuff()
    {
        yield return new WaitForSeconds(delay);
        this.StuffToDo.Invoke();
    }
    public UnityEvent StuffToDo;
    [SerializeField] private int delay = 0;
}
