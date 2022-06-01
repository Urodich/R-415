using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class BO1 : MonoBehaviour
{
    public bool power;
    public int indicationValue{get;set;}
    public int work_ControlValue{get;set;}
    public bool powerValue{get;set;}

    public bool duplexValue{get;set;}
    public bool duChanal{get;set;}
    

    public db_script scale;
    Logic logic;
    BO2 bo2;
    float conI;
    public float ConnectionInput{get{return conI;}set{conI=value; ChangeWorkControl();}}
    float conO;
    public float ConnectionOutput{get{return conO;}set{conO=value; ChangeWorkControl();}}
    float _input;
    public float Input{get{return _input;}set{_input=value; ChangeIndication();}}
    float _output;
    public float Output{get{return _output;}set{_output=value; ChangeIndication();}}
    const float OutBroad = 52;
    const float InRec = 42;
    const float Default=0;
    [SerializeField]Point_script step1;
    [SerializeField]Point_script step5;
    [SerializeField] GameObject interactive;

    void Awake(){
        logic = GameObject.Find("Canvas").GetComponent<Logic>();
        bo2 = GameObject.Find("Б02").GetComponent<BO2>();
    }
    void Start(){
        foreach (EventTrigger elem in interactive.GetComponentsInChildren<EventTrigger>()) elem.triggers[0].callback.AddListener((data) => Check1((PointerEventData)data));
        GameObject.Find("duplex").GetComponent<switch_script>().condition+=()=>{
            if(bo2.tunning!=null) {
                logic.ShowDialog("Дождитесь завершения настройки");
                return 1;
            }
            return 0;
        };
    }
    public void ChangeWorkControl(){        
        switch (work_ControlValue){
            case 0:{
                Input=ConnectionInput;
                Output=ConnectionOutput;
                break;
            }
            case 1:{
                Input=InRec;
                Output=0;
                break;
            }
            case 2:{
                Input=0;
                Output=OutBroad;
                break;
            }
        }
        
    }

    public void ChangeIndication(){
        switch (indicationValue){
            case 0:{
                scale.SetNewValue(Input);
                scale.SetNewBoards(0,10);  
                break;
            }
            case 1:{
                scale.SetNewValue(Input);
                scale.SetNewBoards(10,30);
                break;
            }
            case 2:{
                scale.SetNewValue(Input);
                scale.SetNewBoards(30,50);
                break;
            }
            case 3:{
                scale.SetNewValue(Input);
                scale.SetNewBoards(50,70);
                break;
            }
            case 4:{
                scale.SetNewValue(Output);
                scale.SetNewBoards(0,100);
                break;
            }
            default:{
                scale.SetNewValue(0);
                break;
            }
        }
    }
    ///
    ///Step 1
    ///
    void Check1(PointerEventData eventData){
        if(work_ControlValue==0 && !duplexValue && powerValue) step1.Check=true;
        else if(!power) step1.Check=false;
    }

    public bool preparing(){
        logic.mistakes++;
        if (work_ControlValue!=0){
            logic.ShowDialog("переключатель \"Работа-Контроль\" в состояние \"Работа\"");
            return false;
        }
        if (duplexValue){
            logic.ShowDialog("переключатель \"Дупл.-Деж.Прием\" в состояние \"Деж.Прием\"");
            return false;
        }
        if (!powerValue){
            logic.ShowDialog("переключатель \"Мощность\" в состояние \"Пониженная\"");
            return false;
        }
        logic.mistakes--;
        return true;
    }

    ///
    ///Step 2
    ///
    public void DuplexOn(bool value){
        logic.Duplex=value;
        if(!power)return;
        GameObject.Find("broadcast").GetComponent<lamp_script>().On(!value);
        if(GameObject.Find("Пункт4").GetComponent<Point_script>().Check)step5.Check=value;

        logic.SelfCheckFun(value);
        logic.SelfCheck=true;
    }
    public lamp_script energy;
    public lamp_script receiving;
    public lamp_script broadcast;
    public lamp_script properly;
    public lamp_script rejection1;
    public lamp_script rejection2;
    public lamp_script rejection3;
    public lamp_script rejection4;
    public lamp_script rejection5;
    public lamp_script rejection6;
    public lamp_script broadcastOn;
}
