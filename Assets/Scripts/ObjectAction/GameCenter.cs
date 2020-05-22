using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : MonoBehaviour
{
    // Start is called before the first frame update
    // 该脚本用于游戏中物体状态的控制，各种复杂的状态在这里汇总
    // 铁门交互的状态指示器
    // ironDoorEvent == IDE
    private bool IDEironDoorEventComplete; // 保证方法只被调用一次
    private bool IDElockIsBroken;
    private bool IDElatchIsToken;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject leftBase;
    public GameObject rightBase;


    // ...
    [Space(20)]
    public int bias;

    // ...

    public void IDEbreakLock()
    {
        Debug.Log("Lock break!");
        IDElockIsBroken = true;
    }

    public void IDEtakeLatch()
    {
        Debug.Log("latch token!");
        IDElatchIsToken = true;
    }

    // 解除铁门锁定
    private void IDEreleaseIronDoor()
    {
        Debug.Log("iron door released");
        leftDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rightDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        leftBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rightBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }


    void Start()
    {
        IDEironDoorEventComplete = false;
        IDElockIsBroken = false;
        IDElatchIsToken = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IDEironDoorEventComplete)
        {
            if(IDElatchIsToken && IDElockIsBroken)
            {
                IDEironDoorEventComplete = true;
                IDEreleaseIronDoor();
            }
        }
    }
}
