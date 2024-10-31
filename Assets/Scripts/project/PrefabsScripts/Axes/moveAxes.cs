using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using AXES;


public class moveAxes : AxesBaseClass
{

    private Vector3 screenPosition;
    private Vector3 initialPosition;

    private Vector3 offset;
    protected override void OnDrag(){
        if(axisType == AxisType.None){
            return;
        }
        // 1、 要 点击
        // 2、 要 知道 点击的哪个轴
        // 3、 获取点击了哪个物体
        if(Input.GetMouseButtonDown(0)){
            // Debug.Log("点击了"+ currentAxisParent.name);
            // currentAxisParent = Transform.parent;

            // 记录 当前 要拖动 模型的 初始的位置
            initialPosition = currentAxisParent.position;

            screenPosition = Camera.main.WorldToScreenPoint(initialPosition);
            // Debug.Log("screenPosition =="+ screenPosition);

            offset = initialPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));


            DragState = DragStateType.mouseButtonDown;
            // 获取点击了 哪个轴  然后 绘制直线
            MoveAxesBase.addLine(axisType, transform);
        }

        if(Input.GetMouseButton(0)){
            // 准备拖动
            // currentAxisParent.transform.position = initialPosition + tempPos;
            currentAxisParent.position = initialPosition + MoveAxesBase.computePosition(transform, currentAxisParent, axisType, offset, initialPosition);
            // Debug.Log("currentAxisParent.position =="+ currentAxisParent.position);

            // 通知 entity 修改位置
            EventCenterOptimize.getInstance().EventTrigger<Vector3>(gloab_EventCenter_Name.CHANGE_AXIS_PARENT_POSITION, currentAxisParent.position);

        
        }

        if(Input.GetMouseButtonUp(0)){
            // 鼠标弹起
            if(axisType != AxisType.None){
                DragState = DragStateType.normal;
            }
            DrawLine.removeLine(axisType, transform);
            Done(); // 调用完成方法
        }
    }
}
