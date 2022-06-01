using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lamp_script : MonoBehaviour
{

    public Sprite on;
    public Sprite off;
    bool isOn=false;
    public static bool power{get;set;}


    void Start(){
        gameObject.GetComponent<Image>().sprite = off;
        power=false;
    }
    public void On(bool value){
        isOn=value;
        gameObject.GetComponent<Image>().sprite = (isOn&&power)? on:off;
    }

    public void changePower(bool value){
        gameObject.GetComponent<Image>().sprite = (isOn&&power)? on:off;
    }

}
