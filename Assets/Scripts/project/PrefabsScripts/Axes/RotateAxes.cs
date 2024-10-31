using System.Collections;
using System.Collections.Generic;
using AXES;
using UnityEngine;

public class RotateAxes : AxesBaseClass
{


    private Vector3 screenPosition;
    private Vector3 initialPosition;

    private Vector3 currenthitRayPoint;

    private Vector3 offset;

    private Vector3 temp = Vector3.zero;
    

    protected override void OnDrag(){

        if(axisType == AxisType.None){
            return;
        }
        // 1、 要 点击
        // 2、 要 知道 点击的哪个轴
        // 3、 获取点击了哪个物体
        if(Input.GetMouseButtonDown(0)){

            DragState = DragStateType.mouseButtonDown;
            // Debug.Log("点击了"+ currentAxisParent.name);
            initialPosition = RotateAxesBase.getInitialPosition(transform, axisType, currenthitRayPoint, currentAxisParent);
            temp = Vector3.zero;
        }

        if(Input.GetMouseButton(0) && DragState == DragStateType.mouseButtonDown){
            // 准备拖动
            Debug.Log("准备拖动 DragState =="+ DragState);

            float angle = RotateAxesBase.ComputeRotate(transform, axisType, currenthitRayPoint, initialPosition);

            switch (axisType)
            {

                case AxisType.Z:

                    if(temp.z != angle){
                        currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.forward, angle - temp.z);
                        temp.z = angle;
                    }
                    break;
                case AxisType.Y:

                    if(temp.y != angle){
                        // currentAxisParent.transform.localEulerAngles = currentAxisParent.transform.localEulerAngles - new Vector3(0, temp.y - angle, 0);
                        currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.up, angle - temp.y);
                        temp.y = angle;
                    }

                    break;
                case AxisType.X:
                    if(temp.x != angle){
                        currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.right, angle - temp.x);
                        temp.x = angle;
                    }
                    break;
            }

            
        }

        if(Input.GetMouseButtonUp(0)){
            // 鼠标弹起
            if(axisType != AxisType.None){
                DragState = DragStateType.normal;
            }
            // Transform XYZAxisTransform = transform.Find(axisType.ToString());
            // if (XYZAxisTransform.TryGetComponent(out LineRenderer line)) Destroy(line); // 删除之前添加的高亮线

            DrawLine.removeLine(axisType, transform);
            Done(); // 调用完成方法
        }
    }



    protected override void Raycast()
    {
        base.Raycast();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;//   碰撞点 是 世界 坐标系
        bool res = Physics.Raycast(ray,out hit, 10000, 1<<arrowLayer);

        if(res){
            // Debug.Log("hit =="+ hit.transform.name);
            bool isXYZ = hit.transform.parent.TryGetComponent(out Axis axis);
            if(isXYZ){
                // Debug.Log("Axis  === "+ hit.transform.gameObject.GetComponent<Axis>());
                // Axis axis = hit.transform.gameObject.GetComponent<Axis>();
                // Debug.DrawLine(currentAxisParent.transform.position, hit.point, Color.red, 0.1f);
                currenthitRayPoint = hit.point;

                DragState = DragStateType.hover;
                axisType = axis.axisType;
                axis.setActive(true);
            }
        } else{
            axisType = AxisType.None;
            DragState = DragStateType.normal;

        }

    }







}
