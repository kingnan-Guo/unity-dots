using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class camera : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }



//摄像机操作   
//删减版   在实际的使用中可能会有限制的需求  比如最大远离多少  最近距离多少   不能旋转到地面以下等
public class camera : MonoBehaviour
{

    private GameObject LookAtCube;
    /// <summary>
    /// 注视点位置
    /// </summary>
    private Vector3 LookAtCubePosition = new Vector3(0, 0, 0);
    /// <summary>
    /// 相机与注视点 距离
    /// </summary>
    public int cameraDistance = 20;

    
    /// <summary>
    /// 相机旋转角度 绕 Y 轴
    /// </summary>
    public float RotateVectorUp = 0.0f;
    /// <summary>
    /// 相机旋转角度 绕 X 轴
    /// </summary>
    public float RotateVectorRight = 0.0f;

    /// <summary>
    /// 相机旋转速度
    /// </summary>
    public float CameraRotateSpeed = 2.0f;
    /// <summary>
    /// 相机前进移动速度
    /// </summary>
    public float CameraMoveSpeed = 10.0f;

    /// <summary>
    /// 更新相机的横向平移
    /// </summary>
    public float MoveCameraSidewaysSpeed = 100.0f;
    /// <summary>
    /// 更新相机的纵向平移
    /// </summary>
    public float MoveCameraUpwardsSpeed = 100.0f;

    private string LookAtCubeName = "LookAtCube";


    void Awake(){

    }
    void Start()
    {
        
        transform.position = LookAtCubePosition + Vector3.back * cameraDistance;

        // 创建 cube
        LookAtCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        LookAtCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // LookAtCube.transform.localScale = new Vector3(1f, 1f, 1f);
        // 设置 cube 的位置
        LookAtCube.transform.position = LookAtCubePosition;
        // 设置 cube 的颜色
        LookAtCube.GetComponent<Renderer>().material.color = Color.red;
        LookAtCube.name = LookAtCubeName;

        transform.RotateAround(LookAtCube.transform.position, Vector3.right, RotateVectorRight);

        transform.RotateAround(LookAtCube.transform.position, Vector3.up, RotateVectorUp);
        LookAtCube.transform.Rotate(Vector3.up, RotateVectorUp);
        


    }
    void Update()
    {

        Ctrl_Cam_Move();
        Cam_Ctrl_Rotation();

        // Input.GetKey(KeyCode.Mouse2) ||
        if ( true)
        {
            Vector3 inputDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                inputDir.z = +1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDir.z = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDir.x = +1f;
            }
            // 设置摄像机的旋转
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            // moveDir = new Vector3(moveDir.x, 0, moveDir.z);

            float moveSpeed = 15f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            LookAtCube.transform.position += LookAtCube.transform.right * inputDir.x * moveSpeed * Time.deltaTime;
            // LookAtCube.transform.Translate(new Vector3(data.x, 0, data.z));
            if (Input.GetMouseButton(2)){
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                // 横向平移
                MoveCameraSideways(mouseX);
                // 纵向平移，若相机垂直地面则向前平移
                MoveCameraUpwards(mouseY);
            }

        }


    }
    //镜头的远离和接近
    public void Ctrl_Cam_Move()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(Vector3.forward * CameraMoveSpeed);//速度可调  自行调整
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(Vector3.forward * -CameraMoveSpeed);//速度可调  自行调整
        }
    }
    //摄像机的旋转
    public void Cam_Ctrl_Rotation()
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(LookAtCube.transform.position, Vector3.up, mouse_x * CameraRotateSpeed);
            transform.RotateAround(LookAtCube.transform.position, transform.right, mouse_y * CameraRotateSpeed);

            // 旋转 Cube

            LookAtCube.transform.Rotate(Vector3.up, mouse_x * CameraRotateSpeed);
        }
    }


    // 更新相机的横向平移
    private void MoveCameraSideways(float mouseX)
    {
        // return;
        transform.Translate(Vector3.right * MoveCameraSidewaysSpeed * Time.deltaTime * -mouseX);

        Vector3 data = Vector3.right * MoveCameraSidewaysSpeed * Time.deltaTime * - mouseX;

        LookAtCube.transform.Translate(new Vector3(data.x, 0, data.z));

        

    }

    // 更新相机的纵向平移
    private void MoveCameraUpwards(float mouseY)
    {
        transform.Translate(Vector3.up * MoveCameraUpwardsSpeed * Time.deltaTime * -mouseY);

        LookAtCube.transform.position = LookAtCube.transform.position + LookAtCube.transform.forward * MoveCameraUpwardsSpeed * Time.deltaTime * -mouseY; 

    }


}
