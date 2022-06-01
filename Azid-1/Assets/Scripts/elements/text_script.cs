using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_script : MonoBehaviour
{
    public static bool power{private get;set;}
    public static bool isOn{private get;set;}
    string curValue;
    public string newValue {private get;set;}
    public Text text;

    void Awake(){
        curValue = Random.Range(0,7).ToString();
    }
    void Start(){
        
        text.text=curValue;
        text.color=new Color(1,0.76f,0.01f,0);
    }

    public void ResetValues(){
        curValue=newValue;
        text.text=curValue;
    }
    public void Reload(){
        if(power&&isOn) text.color=new Color(1,0.76f,0.01f,1);
        else text.color=new Color(1,0.76f,0.01f,0);
    }
}
