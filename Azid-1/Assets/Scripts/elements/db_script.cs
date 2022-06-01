using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_script : MonoBehaviour
{
    float minValue=0;
    float maxValue=100;
    [SerializeField] GameObject obj;
    float refValue=0;
    float angle;
    float oldAngle;
    Logic logic;
    void Start()
    {
        logic=GameObject.Find("Canvas").GetComponent<Logic>();
        angle=50;
        oldAngle=angle;
        obj.transform.rotation=Quaternion.Euler(0f,0f,angle);
    }
    public void SetNewValue(float value){
        refValue = value;
    }
    public void SetNewBoards(float minValue, float maxValue){
        this.minValue=minValue;
        this.maxValue=maxValue;
    }

    void FixedUpdate(){
        if(!logic.Power) {obj.transform.rotation=Quaternion.Euler(0f,0f,50); return;}
        angle = (refValue-minValue)/(maxValue-minValue)*100-50;
        if (angle>60) angle=60;
        if (angle<-60) angle=-60;
        angle*=-1;
        if(oldAngle==angle) return;
        oldAngle+=(angle-oldAngle)*Time.deltaTime;
        obj.transform.rotation=Quaternion.Euler(0f,0f, oldAngle);
    }
}
