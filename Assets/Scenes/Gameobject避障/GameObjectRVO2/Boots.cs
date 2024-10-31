using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : MonoBehaviour
{
    [Header("测试资源 游戏对象预制体")]
    public GameObject mainPrefab;
    private GameObject playerRoot;

    public  Charactor[] CharactorArr = null;

    // public RVOWapper RVOWapper;
    public void Awake(){
        // 初始化游戏框架
        this.playerRoot = GameObject.Find("player");
        GameObject.DontDestroyOnLoad(this.gameObject);

        RVOWapper.GetInstance().Init();
        
    }
    void Start()
    {
        this.EnterGame();
    }

    public void CreateFameEnitiy(string name = "0"){
        GameObject game = GameObject.Instantiate(mainPrefab);
        game.transform.SetParent(this.playerRoot.transform, false);
        game.name = name;

        Vector3 vector3;
        vector3.y = 0;
        vector3.x = Random.Range(-10.0f, 10.0f);
        vector3.z = Random.Range(-10.0f, 10.0f);
        game.transform.localPosition = vector3;


        game.AddComponent<Charactor>().Init(vector3, 20.0f);

        

    }
    public void EnterGame(){
        // 释放 地图，角色，障碍物，道具等
        for (int i = 0; i < 100; i++){
            this.CreateFameEnitiy(i.ToString());
        }

        this.CharactorArr = this.playerRoot.GetComponentsInChildren<Charactor>(true);
    }

    public void NavToDest(Vector3 targetPos){
        Debug.Log("导航到目标点 "+ targetPos);
        // 导航到目标点
        for (int i = 0; i < this.CharactorArr.Length; i++)
        {
            this.CharactorArr[i].NavTo(targetPos);
        }

    }
    
    public void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                // Debug.Log(hit.collider.gameObject.name);
                Vector3 targetPos = hit.point;
                NavToDest(targetPos);

                
            }
        }



    }

    void OnApplicationQuit(){
        Debug.Log("退出游戏");
        RVOWapper.GetInstance().Exit();

    }
}
