using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


//摄像机操作   
//删减版   在实际的使用中可能会有限制的需求  比如最大远离多少  最近距离多少   不能旋转到地面以下等
public class cameraPosition : MonoBehaviour
{
    private Transform CenObj;//围绕的物体
    private Vector3 Rotion_Transform;
    private new Camera camera;

    private GameObject LookAtCube;
    // int count = 90;
    public float time;

    private float speed = 2.0f;
    void Start()
    {



        

        print("cameraPosition ==" + this.gameObject.transform.position);

        camera = GetComponent<Camera>();
        // transform.position = new Vector3(0, 0, 0);
        transform.position = new Vector3(70,0, -250);



        // Rotion_Transform = new Vector3(0, 0, 0);
        Rotion_Transform = new Vector3(70, 0, 0);
        // camera.transform.LookAt(Rotion_Transform);
        // 创建 cube
        LookAtCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        LookAtCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // 设置 cube 的位置
        LookAtCube.transform.position = Rotion_Transform;
        // 设置 cube 的颜色
        LookAtCube.GetComponent<Renderer>().material.color = Color.red;
        LookAtCube.name = "LookAtCube";


        transform.RotateAround(LookAtCube.transform.position, Vector3.right, 20);
        transform.RotateAround(LookAtCube.transform.position, Vector3.up, -36);

        LookAtCube.transform.Rotate(Vector3.up, -36);
        // transform.RotateAround(LookAtCube.transform.position, Vector3.right, 20);
        // transform.RotateAround(LookAtCube.transform.position, Vector3.up, -36);

        // LookAtCube.transform.Rotate(Vector3.up, -36);
        // Debug.Log("Rotion_Transform ==" + Rotion_Transform);

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
            // CenObj.rotation = Quaternion.Euler(CenObj.rotation.eulerAngles.x, CenObj.rotation.eulerAngles.y, 0);
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            // moveDir = new Vector3(moveDir.x, 0, moveDir.z);

            // 设置摄像机的旋转
            // if (Input.GetKey(KeyCode.Mouse2)) {
            //     // Debug.Log(transform.forward);
            //     Debug.Log("moveDir ==" + moveDir);
            // }
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
            // if(transform.position.y <= 0){
            //     transform.position = new Vector3(transform.position.x, 0,transform.position.z);
            // }
        }




        //  if (Input.GetMouseButtonDown(2))
        // {
        //     // 开始拖动操作
        //     Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        //     Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //     // transform.position = objPosition;

        //     Debug.Log("GetMouseButtonDown =="+ mousePosition);
        // }
 
        // // 检查鼠标中键是否持续按下
        // if (Input.GetMouseButton(2))
        // {
        //     // 正在拖动
        //     // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        //     // Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //     // transform.position = objPosition;

        //     Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        //     Debug.Log("GetMouseButton mousePosition =="+ mousePosition);
        // }
 
        // // 检查鼠标中键是否松开
        // if (Input.GetMouseButtonUp(2))
        // {
        //     Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

        //     Debug.Log("GetMouseButtonUp mousePosition =="+ mousePosition);
        // }



    }
    //镜头的远离和接近
    public void Ctrl_Cam_Move()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(Vector3.forward * 10f);//速度可调  自行调整
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(Vector3.forward * -10f);//速度可调  自行调整
        }
    }
    //摄像机的旋转
    public void Cam_Ctrl_Rotation()
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(LookAtCube.transform.position, Vector3.up, mouse_x * speed);
            transform.RotateAround(LookAtCube.transform.position, transform.right, mouse_y * speed);

            // 旋转 Cube

            LookAtCube.transform.Rotate(Vector3.up, mouse_x * speed);
            // LookAtCube.transform.Rotate(Vector3.right, mouse_y * 20);
        }
    }


    // 更新相机的横向平移
    private void MoveCameraSideways(float mouseX)
    {
        // return;
        transform.Translate(Vector3.right * 50 * Time.deltaTime * -mouseX);

        Vector3 data = Vector3.right * 50 * Time.deltaTime * - mouseX;

        LookAtCube.transform.Translate(new Vector3(data.x, 0, data.z));

        // Debug.Log("LookAtCube.transform =="+ LookAtCube.transform);

        // Rotion_Transform = (Vector3.right * 50 * Time.deltaTime * -mouseX);
        

    }

    // 更新相机的纵向平移
    private void MoveCameraUpwards(float mouseY)
    {
        transform.Translate(Vector3.up * 50 * Time.deltaTime * -mouseY);
        // Rotion_Transform = (Vector3.up * 50 * Time.deltaTime * -mouseY);
        Vector3 data = Vector3.up * 50 * Time.deltaTime * -mouseY;
        // transform 旋转角度
        // Debug.Log("transform.localRotation =="+ transform.rotation);
        // Quaternion rotationAsQuaternion = transform.rotation;
        // 获取物体的当前旋转作为欧拉角
        Vector3 rotationAsEulerAngles = transform.eulerAngles;
        // 打印当前的欧拉角到控制台
        // Debug.Log("当前旋转为欧拉角 (x, y, z): " + rotationAsEulerAngles);
        // data * Mathf.Cos(rotationAsEulerAngles.x);
        // Debug.Log("data 2=="+ data);
        // Debug.Log("data 2=="+ data * Mathf.Cos((90 - rotationAsEulerAngles.x) * (Mathf.PI / 180)));
        // Debug.Log("rotationAsEulerAngles =="+ (90 - rotationAsEulerAngles.x) * (Mathf.PI / 180));
        // 乘以 cos (lookAtCube.transform.up.y) 来调整平移的方向
        // Mathf.Cos(LookAtCube.transform.up.y);
        // LookAtCube.transform.Translate(new Vector3(0, data.y * Mathf.Cos(LookAtCube.transform.up.y), 0));
        // Debug.Log("Vector3.up  "+ Vector3.up);
        // Debug.Log("LookAtCube.transform.up  "+ LookAtCube.transform.up);
        // Debug.Log("transform == "+ transform.position );
        // Debug.Log("LookAtCube.transform.forward = "+ LookAtCube.transform.forward);
        // Debug.Log("mouseY == "+ -mouseY );
        // Debug.DrawLine(LookAtCube.transform.position, LookAtCube.transform.position + LookAtCube.transform.forward, Color.red, 50);
        // Vector3 moveDir = -transform.forward * 100 * Time.deltaTime * -mouseY;
        // LookAtCube.transform.Translate(new Vector3(moveDir.x, 0 , moveDir.z));
        // LookAtCube.transform.Translate(LookAtCube.transform.right * 100 * Time.deltaTime);
        LookAtCube.transform.position = LookAtCube.transform.position + LookAtCube.transform.forward * 50 * Time.deltaTime * -mouseY; 
        // Debug.Log("LookAtCube.transform =="+ data);

    }


    public void changeLookAtPosition(Vector3 position, Quaternion rotation, int distance = 0){
        // transform.position = position;
        // transform.rotation = rotation;

        Transform LookAtCube = GameObject.Find("LookAtCube").transform;

        LookAtCube.transform.position = position;

        LookAtCube.transform.rotation = rotation;

        // changeCameraPosition( position, rotation, distance);
    }

    public void changeCameraPosition(Vector3 position, Quaternion rotation, Vector3 offset, int RotateAround = 30){
        // transform.position = position;
        // transform.rotation = rotation;

        transform.position = position - offset;
        transform.rotation = rotation;
        transform.RotateAround(position, GameObject.Find("LookAtCube").transform.right, RotateAround);


        
    }
    


}
