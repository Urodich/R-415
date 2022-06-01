using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BO2 : MonoBehaviour
{
    public bool power=false;
    public  int broadcast1 {get; set;}
    public int broadcast2{get; set;}
    public int broadcast3{get; set;}
    public int receiving1{get; set;}
    public int receiving2{get; set;}
    public int receiving3{get; set;}
    //public bool indicationOn{get; set;}
    //public bool tuning{get; set;}
    [SerializeField] AudioSource audioSource1;
    [SerializeField] Point_script step4;
    BO1 bO1;
    Logic logic;
    void Start(){
        bO1=GameObject.Find("Б01").GetComponent<BO1>();
        logic=GameObject.Find("Canvas").GetComponent<Logic>();
        logic.IntputRate=GameObject.Find("Inx100").GetComponent<text_script>().text.text+GameObject.Find("Inx10").GetComponent<text_script>().text.text+GameObject.Find("Inx1").GetComponent<text_script>().text.text;
        logic.OutputRate=GameObject.Find("Outx100").GetComponent<text_script>().text.text+GameObject.Find("Outx10").GetComponent<text_script>().text.text+GameObject.Find("Outx1").GetComponent<text_script>().text.text;
    }

    public void Indication(bool value){
        text_script.isOn=value;
        foreach(text_script elem in gameObject.GetComponentsInChildren<text_script>()) elem.Reload();
    }

    public Coroutine tunning;

    public void SettingUp(){
        if(!power)return;
        if (bO1.duplexValue) {
            logic.ShowDialog("переключатель \"Дупл.-Деж.Прием\" в состояние \"Деж.Прием\""); 
            logic.mistakes++;
            return;
        }
        tunning=StartCoroutine(Tuning());
    }

    IEnumerator Tuning(){
        lamp_script recLamp = GameObject.Find("receiving_control").GetComponent<lamp_script>();
        lamp_script broadLamp = GameObject.Find("broadcast_control").GetComponent<lamp_script>();
        recLamp.On(true);
        //sound
        audioSource1.Play();
        yield return new WaitForSecondsRealtime(2);
        List<string> list = new List<string>(){"Outx100","Outx10","Outx1"};
        foreach(text_script elem in GetComponentsInChildren<text_script>()) 
            if (!list.Contains(elem.name)) elem.ResetValues();
        recLamp.On(false);
        broadLamp.On(true);
        //sound
        audioSource1.Play();
        yield return new WaitForSecondsRealtime(2);
        audioSource1.Stop();
        foreach(text_script elem in GetComponentsInChildren<text_script>()) 
            if (list.Contains(elem.name)) elem.ResetValues();
        broadLamp.On(false);
        
        bO1.Output=100;
        yield return new WaitForSecondsRealtime(.5f);
        bO1.Output=0;

        GameObject.Find("receiving").GetComponent<lamp_script>().On(false);
        //GameObject.Find("broadcast").GetComponent<lamp_script>().On(false);
        GameObject.Find("properly").GetComponent<lamp_script>().On(true);

        step4.Check=true;

        logic.IntputRate=GameObject.Find("Inx100").GetComponent<text_script>().text.text+GameObject.Find("Inx10").GetComponent<text_script>().text.text+GameObject.Find("Inx1").GetComponent<text_script>().text.text;
        logic.OutputRate=GameObject.Find("Outx100").GetComponent<text_script>().text.text+GameObject.Find("Outx10").GetComponent<text_script>().text.text+GameObject.Find("Outx1").GetComponent<text_script>().text.text;
        tunning=null;
    }
}
