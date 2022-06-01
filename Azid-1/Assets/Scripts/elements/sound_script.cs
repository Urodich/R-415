using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sound_script : MonoBehaviour
{
    [SerializeField] int angle;
    [SerializeField] float speed;
    [SerializeField] GameObject axis;
    float curAngle=0;
    float value;
    RectTransform trans;
    bool left;
    bool right;
    [SerializeField] FlEvent setValue;
    void Start()
    {
        trans=axis.GetComponent<RectTransform>();
        val();
    }
    void FixedUpdate()
    {
        if(left)leftRotate();
        if(right)rightRotate();
    }

    public void Click()
    {
    if(Input.GetMouseButtonDown(0)) {right = false; left=true;}
    if(Input.GetMouseButtonDown(1)) {left = false; right=true;}
    }

    public void UnClick(){
        right = false;
        left=false;
    }

    void leftRotate() {
        if (curAngle<angle) 
        {
            curAngle += speed*Time.deltaTime;
            trans.eulerAngles= new Vector3(0,0,curAngle);
            val();
        }
    }
    void rightRotate() {
        if (curAngle>-angle) 
        {
            curAngle -= speed*Time.deltaTime;
            trans.eulerAngles= new Vector3(0,0,curAngle);
            val();
        }
    }

    void val(){
        value = (-curAngle+angle)/(2*angle)*100;
        if (value<0) value=0;
        if (value>100) value = 100;
        if(setValue!=null) setValue.Invoke(value);
    }

}
