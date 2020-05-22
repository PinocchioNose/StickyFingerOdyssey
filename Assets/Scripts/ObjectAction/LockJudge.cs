using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockJudge : MonoBehaviour
{
    // Start is called before the first frame update

    //public GameObject leftDoor;
    //public GameObject rightDoor;
    //public GameObject leftBase;
    //public GameObject rightBase;

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "pickable")
        //{
        //    Debug.Log("exit!");
        //    leftDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //    rightDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //    leftBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //    rightBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //}
        if(other.gameObject.tag == "pickable")
        {
            GameObject.Find("GameControlCenter").GetComponent<GameCenter>().IDEtakeLatch();
        }

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
