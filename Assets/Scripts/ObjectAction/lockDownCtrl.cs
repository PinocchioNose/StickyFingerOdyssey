using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockDownCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool rbAdded;
    private void OnCollisionEnter(Collision collision)
    {
        if(rbAdded == false)
        {
            if (collision.gameObject.name == "use_stone")
            {
                Debug.Log("added!");
                this.gameObject.AddComponent<Rigidbody>();
                rbAdded = true;
                GameObject.Find("GameControlCenter").GetComponent<GameCenter>().IDEbreakLock();
            }
        }
        
    }

    void Start()
    {
        rbAdded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
