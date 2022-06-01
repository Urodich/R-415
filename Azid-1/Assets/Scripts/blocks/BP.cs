using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class BP : MonoBehaviour
{
    public Logic logic;
    
    bool b220;
    public bool B220{get{return b220;}set{b220=value; logic.Power=value; Check3(value);}}
    public bool B27{get; set;}

    BO1 bo1;
    B17 b17;
    [SerializeField] Point_script step3;
    
    void Start(){
        bo1 = GameObject.Find("Б01").GetComponent<BO1>();
        b17 = GameObject.Find("Б17").GetComponent<B17>();
        GameObject.Find("220").GetComponent<button_script>().condition += preparing;
        GameObject.Find("27").GetComponent<button_script>().condition += ()=>{OnB27();return 1;};
    }

    int preparing(){
        return (bo1.preparing() && b17.preparing())? 0:1;    
    }

    void Check3(bool value){
        if(value) step3.Check=true;
        else step3.Check=false;
    }

    void OnB27(){
        if(b220) logic.ShowDialog("нельзя включать 2 источника питания одновременно");
        else logic.ShowDialog("в данном случае питание отсутствует, включите 220В");
    }
}
