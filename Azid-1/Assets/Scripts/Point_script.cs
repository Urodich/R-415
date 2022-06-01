using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point_script : MonoBehaviour
{
    bool check = false;
    public bool Check { get { return check; } set { check = value; if (value == true) _image.color = Color.green; else _image.color = Color.white; } }
    [SerializeField] Image _image;

    
}
