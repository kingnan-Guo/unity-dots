using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /// <summary>
    /// 全局事件 中心 名称
    /// </summary>
    public struct gloab_EventCenter_Name
    {

        //// start ========== 鼠标事件   ===============

        // MouseButtonUp
        /// <summary>
        /// 鼠标按键抬起 事件
        /// </summary>
        public static string MOUSE_BUTTON_UP = "MouseButtonUp";

        // mouseMovePositionPhysics
        /// <summary>
        /// 来源是 鼠标 按钮 MouseButtonUp 事件的 后的 坐标的 射线检测碰撞体  
        /// </summary>
        public static string MOUSE_MOVE_POSITION_PHYSICS = "mouseMovePositionPhysics";

        // mousePositionPhysics
        /// <summary>
        /// MOUSE_POSITION_PHYSICS  射线检测鼠标位置  ; 输出 参数 类型 GameObject
        /// </summary>
        public static string MOUSE_POSITION_PHYSICS = "mousePositionPhysics";

        /// <summary>
        /// 鼠标移动 
        /// </summary>
        // public static string MOUSE_MOVE_POSITION_CANVAS = "mouseMovePositionCanvas";


        // MousePosition
        /// <summary>
        /// 3d 场景 鼠标位置 
        /// </summary>
        public static string MOUSE_POSITION = "MousePosition";


        //// end ========== 鼠标事件   ===============


        //// start ============= 摄像机相关 ====================

        // cameraPosition
        /// <summary>
        /// 更换 相机 及其 lookAtCube 位置;
        /// 参数  gloabCameraLookAtInfo
        /// </summary>
        public static string CAMERA_POSITION = "cameraPosition";
        //// end ============= 摄像机相关 ==================== 






        //// start ============= 建筑物相关 ==============

        //mainMapLoadDone 地图加载完成
        /// <summary>
        /// mainMapLoadDone 地图加载完成
        /// </summary>
        public static string MAIN_MAP_LOAD_DONE = "mainMapLoadDone";


        // global_currentMainParentTransform
        /// <summary>
        /// 更换建筑我的 主父级 , 更改 GameMainManager 的 全局变量 global_currentMainParentTransform
        /// </summary>
        public static string GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM = "global_currentMainParentTransform";




        //showAppointFloor
        /// <summary>
        /// SHOW_APPOINT_FLOOR 显示指定楼层 传入 楼层名称 string
        /// </summary>
        public static string SHOW_APPOINT_FLOOR = "showAppointFloor";


        // buildingInfoDictionary
        /// <summary>
        /// 发送 楼层按钮 的数据 到 buildingView 用于处理显示隐藏 某些 楼层 
        /// </summary>
        public static string BUILDING_INFO_DICTIONARY = "buildingInfoDictionary";

        // BUILDING_INFO_OF_JSON
        /// <summary>
        /// 来自json 的 建筑物 模型 信息
        /// </summary>
        public static string BUILDING_INFO_OF_JSON = "buildingInfoOfJSON";


        // subThreadToMainThreadOfBuildingInfo
        /// <summary>
        /// 子线程 到 主线程 的 建筑物信息
        /// </summary>
        public static string SUB_THREAD_TO_MAN_THREAD_OF_BUILDING_INFO = "subThreadToMainThreadOfBuildingInfo";

        // switch_UNDER_GROUND
        /// <summary>
        /// 显隐 地下层
        /// </summary>
        public static string SWITCH_UNDER_GROUND = "switchUnderGround";



        //// end ============= 建筑物相关 ==============



        /// start ================  canvas 相关  ===========
        


        /// <summary>
        /// canvas 按钮 更换 主 建筑物  楼幢 
        /// </summary>
        public static string CHANGE_MAIN_BUILDING = "changeMainbuilding";



        // changeBuilding
        /// <summary>
        /// 更换 建筑物 卡片 active
        /// </summary>
        public static string CHANGE_BUILDING_CARD_ACTIVE = "changeBuildingCardActive";

        // changeFloor
        /// <summary>
        /// 更换 楼层 卡片 active
        /// </summary>
        public static string CHANGE_FLOOR_CARD_ACTIVE = "changeFloorCardActive";


        // CANVAS_BUTTON
        /// <summary>
        /// canvas 按钮
        /// </summary>
        public static string CANVAS_BUTTON = "canvasButton";

        // 设备列表按钮点击事件
        public static string CLICK_TWO_D_BUTTON = "click2dButton";

        // 设备列表设备点击事件
        public static string CLICK_TWO_D_DEVICE = "click2dDevice";

        // 设备列表设备hover事件
        public static string HOVER_DEVICE_BOX = "hoverDeviceBox";


        // refreshCanvasDeviceList
        /// <summary>
        /// 刷新 canvas 设备列表
        /// </summary>
        public static string REFRESH_CANVAS_DEVICE_LIST = "refreshCanvasDeviceList";

        /// end ================  canvas 相关  ===========









        /// start =========== 物体拖拽 ===============

        // 2dModelTo3DModel
        /// <summary>
        /// 2dModelTo3DModel  2d 模型拖拽到 3d 模型
        /// </summary>
        public static string TWO_D_MODEL_TO_THREE_D_MODEL = "2dModelTo3DModel";



        //2dModelIsIn3DScene
        /// <summary>
        /// 2d 模型 在 3d 场景 目的 是 投个影 便于知道 位置 : params gameObject
        /// </summary>
        public static string TWO_D_MODEL_IS_IN_THREE_D_SCENE = "2dModelIsIn3DScene";



        // DONE_ADD_MODEL_TO_MAIN_MAP
        /// <summary>
        /// DONE_ADD_MODEL_TO_MAIN_MAP 添加模型到 主场景
        /// </summary>
         public static string DONE_ADD_MODEL_TO_SECENE = "DoneAddModelToSecene";



        //  DONE_UPDATE_MODEL_POSITION
        /// <summary>
        /// DONE_UPDATE_MODEL_POSITION 拖拽模型 更改位置
        /// </summary>
        public static string DONE_UPDATE_MODEL = "DoneUpdateModelPosition";





        // DONE_DELETE_MODEL
        /// <summary>
        /// DONE_DELETE_MODEL 删除模型
        /// </summary>
        public static string DONE_DELETE_MODEL = "DoneDeleteModel";


        // HAVE_AXES_TRANSFORM
        /// <summary>
        /// HAVE_AXES_TRANSFORM 有轴的模型
        /// </summary>
        public static string HAVE_AXES_TRANSFORM = "HaveAxesTransform";


        // CHANGE_AXES
        /// <summary>
        /// CHANGE_AXES 更改轴
        /// </summary>
        public static string CHANGE_AXES = "ChangeAxes";

        // Update

        // changeAxisParentPosition
        /// <summary>
        /// 更改轴的父节点位置
        /// </summary>
        public static string CHANGE_AXIS_PARENT_POSITION = "changeAxisParentPosition";


        /// end =========== 物体拖拽 ===============





        /// start =========== 数据库相关 ===============


        public static string DATABASE_CONNECT_SUCCESS = "databaseConnectSuccess";
        public static string DATABASE_CONNECT_FAIL = "databaseConnectFail";
        public static string DATABASE_CONNECT_CLOSE = "databaseConnectClose";
        public static string DATABASE_INSERT = "databaseInsert";


        public static string DATABASE_CONFIG = "databaseConfig";

        // 数据库操作
        public static string DATABASE_OPERATE = "databaseOperate";


        // DATABASE_OPERATE_END
        /// <summary>
        /// 数据库操作结束 
        /// </summary>
        public static string DATABASE_OPERATE_END = "databaseOperateEnd";


        /// end ============= 数据库相关 ===============
        /// 

        /// 设备相关 ===========
        /// 

        // UPDATE_EQUIPMENT_INFO
        /// <summary>
        /// 更新设备信息
        /// </summary>
        public static string UPDATE_EQUIPMENT_INFO = "updateEquipmentInfo";

        
        // RECEIVE_DATA_FROM_WEB
        /// <summary>
        /// 从web 接收数据
        /// </summary>
        public static string RECEIVE_DATA_FROM_WEB = "receiveDataFromWeb";

        // sendDataToCommunicationToWeb
        /// <summary>
        /// 发送 数据到 web
        /// </summary>
        public static string SEND_DATA_TO_COMMUNICATION_TO_WEB = "sendDataToCommunicationToWeb";
    }