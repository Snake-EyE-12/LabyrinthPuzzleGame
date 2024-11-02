using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour, Pointer
{
    public UnityEvent OnClick;

    public UnityEvent OnHover;
    public UnityEvent OnEndHover;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnEndHover?.Invoke();
    }
}
