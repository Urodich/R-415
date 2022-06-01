using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    bool power;
    public bool Power{get{return power;} set{power=value; ChangePower(value);}}
    public BO1 BO1;
    public BO2 BO2;
    public B17 B17;
    public BP BP;
    public GameObject BO4;

    public float InputValue=40;
    public float OutputValue=40;
    public bool Duplex;

    //Test check//
    public bool SelfCheck=false;
    public float Phone1Volume=100;     
    public float Phone2Volume=100;
    public string IntputRate;
    public string OutputRate;
    public int mistakes=0;
    float time=0;    

    [SerializeField] GameObject statistic;
    [SerializeField] GameObject dialog;
    VoiceChat voiceChatManager;
    void Start(){
        Time.timeScale=1;
        statistic.SetActive(false);
        voiceChatManager=GameObject.Find("VoiceChatManager").GetComponent<VoiceChat>();
    }
    public void End(){
        voiceChatManager.LeaveChannel();
        Time.timeScale=0;
        statistic.SetActive(true);
        GameObject.Find("input").GetComponent<Text>().text=IntputRate;
        GameObject.Find("output").GetComponent<Text>().text=OutputRate;
        GameObject.Find("mistakes").GetComponent<Text>().text=mistakes.ToString();
        GameObject.Find("selfCheck").GetComponent<Text>().text=SelfCheck?"выполнено":"не выполнено";
        GameObject.Find("volumeReg").GetComponent<Text>().text=(GameObject.Find("Пункт7").GetComponent<Point_script>().Check)?"выполнено":"не выполнено";
        GameObject.Find("time").GetComponent<Text>().text=Mathf.Round(time/60)+":"+Mathf.Round(time%60);
        
    }
    void FixedUpdate(){
        time+=Time.fixedDeltaTime;
    }
    
    public void SelfCheckFun(bool value){
        GameObject.Find("Пункт6").GetComponent<Point_script>().Check=true;
        SelfCheck=true;
        voiceChatManager.SelfCheck(value);

    }
    public void ShowDialog(string message){
        dialog.SetActive(true);
        GameObject.Find("DialogText").GetComponent<Text>().text=message;
    }
    public void Connect(){
        voiceChatManager.InputRate=System.Convert.ToInt32(this.IntputRate);
        voiceChatManager.OutputRate=System.Convert.ToInt32(this.OutputRate);
        voiceChatManager.SelfCheck(false);
        voiceChatManager.JoinChannel("test");

        voiceChatManager.MuteMe(!Duplex);
    }
    public void Disconnect(){
        voiceChatManager.LeaveChannel();
        voiceChatManager.SelfCheck(Duplex);
    }

    //
    //Power On
    //
    void ChangePower(bool isOn){
        lamp_script.power=isOn;
        text_script.power=isOn;
        BO1.power=isOn;
        B17.power=isOn;
        BO2.power=isOn;
        foreach(lamp_script lamp in  GetComponents<lamp_script>()) lamp.changePower(isOn);
        foreach(text_script elem in BO2.GetComponentsInChildren<text_script>()) elem.Reload();
        lighting1(isOn);
        if(!isOn)Disconnect();
    }
    

    void lighting1(bool value){
        List<string> list = new List<string>{"energy","broadcast","receiving"};
        foreach (lamp_script elem in BO1.GetComponentsInChildren<lamp_script>()){
            if (list.Contains(elem.name)) {elem.On(value);}
        }
        List<string> list1 = new List<string>{"power","220"};
        foreach (lamp_script elem in BO4.GetComponentsInChildren<lamp_script>()){
            if (list1.Contains(elem.name)) elem.On(value);
        }
    }
}
