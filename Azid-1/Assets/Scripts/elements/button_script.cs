using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class button_script : MonoBehaviour
{
    public BoolEvent BClick;
    public event Func<int> condition;
    [SerializeField] Sprite on;
    [SerializeField] Sprite off;
    bool isOn = false;
    Image image;
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        isOn=false;
        image.sprite = off;
    }

    public void Click(){
        if (condition!=null && !isOn && condition.Invoke()!=0) return;
        isOn=!isOn;
        image.sprite = isOn?on:off;
        BClick?.Invoke(isOn);
    }
}
[System.Serializable]
public class BoolEvent : UnityEvent<bool> {}