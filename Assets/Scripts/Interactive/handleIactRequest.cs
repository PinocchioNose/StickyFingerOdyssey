using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleIactRequest : MonoBehaviour
{
    // Start is called before the first frame update
    private bool inputStatus = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inputStatus == true)
            if(Input.GetKeyDown("e")){
                this.gameObject.GetComponent<Iiact>().changeStat();
                
            }
    }

    //interface opened for objects require interaction with thisobj.
    public void readyForIact(){
        inputStatus = true;
    }
}


