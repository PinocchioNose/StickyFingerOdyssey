using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmill : MonoBehaviour
{
    public GameObject center,father;
    public GameObject[] leaves;
    public float rot_speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < leaves.Length; i++)
        {
            leaves[i].transform.RotateAround(center.transform.position, father.transform.right, rot_speed);
        }
    }
}
