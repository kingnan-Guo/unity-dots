using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public unsafe void test(){
        // int* p = (int*)0x7FF7D9C6B9D0;
        // *p = 10;
        // Debug.Log(*p);
        int i = 3;
        int* p = &i;

        // Debug.Log("test" + p.ToString());
        Debug.Log(*p);
    }
}
