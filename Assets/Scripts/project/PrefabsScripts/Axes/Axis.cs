using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AXES;

public class Axis : MonoBehaviour
{
    public AxisType axisType;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        switch (transform.name)
        {
            case "X":
                axisType = AxisType.X;
                break;
            case "Y": 
                axisType = AxisType.Y;
                break;
            case "Z":
                axisType = AxisType.Z;
                break;
            case "XY":
                axisType = AxisType.XY;
                break;
            case "XZ":
                axisType = AxisType.XZ;
                break;
            case "YZ":
                axisType = AxisType.YZ;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            // 放大一倍
            // transform.localScale = transform.localScale * 2;
            // transform.
        } else {

        }
    }

    public void setActive(bool data){
        isActive = data;
    }
}
