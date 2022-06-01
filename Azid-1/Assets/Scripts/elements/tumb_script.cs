using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[System.Serializable]
public class FlEvent : UnityEvent<float>{}
public class tumb_script : MonoBehaviour
{
    public text_script text;
    public int[] values;
    public int[] rotations;
    int index = 0;
    public int curValue;
    GameObject obj;
    public IntEvent setValue;
    public event Func<int> condition;
    void Start()
    {
        index = UnityEngine.Random.Range(0,values.Length-1);
        Set();
    }

    public void OnMouseUpAdButton(){        //мб будут проблемы
        if (condition!=null && condition.Invoke()!=0) return;
        if(Input.GetMouseButton(0)&&index>0) index--;
        if(Input.GetMouseButton(1)&&index<values.Length-1) index++;
        Set();
    }

    void Set(){
        gameObject.transform.rotation=Quaternion.Euler(0,0,rotations[index]);
        curValue = values[index];
        if(setValue!=null)setValue.Invoke(values[index]);
        if(text!=null) text.newValue=values[index].ToString();
    }
}
[System.Serializable]
public class IntEvent : UnityEvent<int> {}
