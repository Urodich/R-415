using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class switch_script : MonoBehaviour
{
    public bool isOn;
    public BoolEvent BClick;
    public event Func<int> condition;

    void Start()
    {
        isOn=UnityEngine.Random.Range(0,1)==1?true:false;
        if (isOn)gameObject.transform.Rotate(0,0,180, Space.World);
        BClick.Invoke(isOn);
    }

    public void Click(){
        if (condition!=null && condition.Invoke()!=0) return;
        isOn=!isOn;
        gameObject.transform.Rotate(0,0,180, Space.World);
        BClick.Invoke(isOn);
    }
}
