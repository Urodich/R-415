using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class B17 : MonoBehaviour
{
    public bool power=false; //its Global power;
    public int LevelControl{get;set;}
    public int PVU{get;set;}
    public bool CL {get;set;}=false;
    public bool IG {get;set;}=false;
    public bool Duplex {get;set;}
    public db_script scale;

    const float zero=0;
    Logic logic;
    [SerializeField] Point_script step2;
    [SerializeField] Point_script step7;
    [SerializeField] Point_script step8;
    [SerializeField] GameObject interactive;

    [SerializeField] sound_script sound_1;
    [SerializeField] sound_script sound_2;
    [SerializeField] tumb_script[] tumbs;
    void Awake(){
        logic=GameObject.Find("Canvas").GetComponent<Logic>();
    }
    void Start(){
        scale.SetNewBoards(0,100);  
        foreach (EventTrigger elem in interactive.GetComponentsInChildren<EventTrigger>()) elem.triggers[0].callback.AddListener((data) => Check2((PointerEventData)data));
        GameObject.Find("Cl").GetComponent<button_script>().condition += ()=>{
            if(!power) return 0;
            if(IG){
                logic.ShowDialog("Иг должен быть выключен"); 
                return 1;
            }
            if(!Duplex) {
                logic.ShowDialog("переключатель \"Деж. прием/ Дупл.\" в состояние \"Дупл.\"");
                return 1;
            }
            return 0;
        };
        GameObject.Find("Ig").GetComponent<button_script>().condition += ()=>{
            if(!power) return 0;
            if(!Duplex) {
                logic.ShowDialog("переключатель \"Деж. прием/ Дупл.\" в состояние \"Дупл.\"");
                return 1;
            }
            if (CL) {
                logic.ShowDialog("Сл. должен быть выключен");
                return 1;
            }
            //if(!(PVU==0 && Phone1Settings()==0) && !(PVU==2 && Phone2Settings()==0)) return 1;
            return 0;
        };
        foreach (tumb_script tumb in tumbs)
            tumb.condition+=()=>{
                if(IG){
                logic.ShowDialog("Иг должен быть выключен"); 
                return 1;
                }
                if (CL) {
                logic.ShowDialog("Сл. должен быть выключен");
                return 1;
                }
                return 0;
            };
    }

    public void Cl(bool value){
        if(!power)return;
        

        if (value) 
            ig=StartCoroutine(Change());

        if(value){
            Debug.Log("Connect command");
            logic.Connect();
            step8.Check=true;
        }
        else{
            logic.Disconnect();
            Debug.Log("Disconnect command");
        }
    }


    void Check2(PointerEventData eventData){
        if(LevelControl==4 && PVU==1 && phone1==0 && call1==1 && phone2==0 && call2==1) step2.Check=true;
        else if(!power) step2.Check=false;
    }

    public bool preparing(){
        if (LevelControl!=4){
            logic.ShowDialog("переключатель \"Контроль Уровня\" в состояние \"Выкл.\"");
            return false;
        }
        if (PVU!=1){
            logic.ShowDialog("переключатель \"ПВУ\" в состояние \"Выкл.\"");
            return false;
        }
        if (phone1!=0){
            logic.ShowDialog("переключатель \"I Тлф.\" в состояние \"2 Пр. Окон.\"");
            return false;
        }
        if (call1!=1){
            logic.ShowDialog("переключатель \"Кан.- Раб.- Сл.\" в состояние \"Раб.\"");
            return false;
        }
        if (phone2!=0){
            logic.ShowDialog("переключатель \"II Тлф.\" в состояние \"2 Пр. Окон.\"");
            return false;
        }
        if (call2!=1){
            logic.ShowDialog("переключатель \"Кан.- Раб.- Сл.\" в состояние \"Раб.\"");
            return false;
        }
        return true;
    }

    float CurrentValue(){
        if (!power)return zero;
        switch (PVU){
            case 1:{
                return zero;
            }
            case 0:{
                if(LevelControl==3) 
                    return logic.OutputValue;
                else if(LevelControl<3)
                    return  _volume1;
                else return zero; 
            }

            case 2:{
                if(LevelControl==5) 
                    return logic.OutputValue;
                else if(LevelControl>5)
                    return _volume2;
                else return zero; 
            }

            default: return zero;
        }
    }

    public Coroutine ig;
    public void Ig(bool value){
        if(!power) return;
        if (value) 
            ig=StartCoroutine(Change());
        
    }
    
    IEnumerator Change(){
        while(IG || CL){
            if(PVU==0 && Phone1Settings()==0){
                switch(LevelControl) {
                        case 3:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                        case 2:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                        case 1:{
                            scale.SetNewValue( CurrentValue()-20);
                            //scale.SetNewBoards(20,120);
                            break;
                        }
                        case 0:{
                            scale.SetNewValue( CurrentValue()-35);
                            //scale.SetNewBoards(35,135);                        
                            break;
                        }
                        default:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                }
                
            }

            if(PVU==2 && Phone2Settings()==0){
                switch(LevelControl) {
                        case 5:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                        case 6:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                        case 7:{
                            scale.SetNewValue( CurrentValue()-20);
                            //scale.SetNewBoards(20,120);
                            break;
                        }
                        case 8:{
                            scale.SetNewValue( CurrentValue()-35);
                            //scale.SetNewBoards(35,135);
                            break;
                        }
                        default:{
                            scale.SetNewValue( CurrentValue());
                            //scale.SetNewBoards(0,100);
                            break;
                        }
                }
            }
            if((logic.Phone1Volume>50 && logic.Phone1Volume<60)&&(logic.Phone2Volume>50 && logic.Phone2Volume<60))step7.Check=true;
            else step7.Check=false;
            yield return new WaitForSeconds(.3f);
        }
        scale.SetNewValue( zero);
    }
    public int Phone1Settings(){
        //logic.mistakes++;
        if (phone1!=2){
            //logic.ShowDialog("переключатель \"I Тлф\" в состояние \"4Пр.-Окон.\"");
            return 1;
        }
        if (call1!=0){
            //logic.ShowDialog("переключатель \"Вылов I Тлф\" в состояние \"Кан.\"");
            return 1;
        }
        //logic.mistakes--;
        return 0;
    }
    public int Phone2Settings(){
        //logic.mistakes++;
        if (phone2!=2){
            //logic.ShowDialog("переключатель \"II Тлф\" в состояние \"4Пр.-Окон.\"");

            return 1;
        }
        if (call2!=0){
            //logic.ShowDialog("переключатель \"Вылов II Тлф\" в состояние \"Кан.\"");
            return 1;
        }
        //logic.mistakes--;
        return 0;
    }
    //
    //phone 1
    //
    public int call1{get;set;}
    float _volume1;
    public float volume1{get{return _volume1;} set{_volume1=value; logic.Phone1Volume=value;}}
    public int phone1 {get;set;}

    //useless
    public bool broadOn1{get;set;}
    public bool THK1{get;set;}
    public bool Control1{get;set;}

    //
    //phone 2
    //
    public int call2{get;set;}
    public int phone2{get;set;}
    float _volume2;
    public float volume2{get{return _volume2;} set{_volume2=value; logic.Phone2Volume=value;}}

    //useless
    public bool broadOn2{get;set;}
    public bool THK2{get;set;}
    public bool Control2{get;set;}

}
