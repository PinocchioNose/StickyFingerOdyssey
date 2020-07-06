using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickyUI : MonoBehaviour
{
    // 两个UI
    public Image leftUI;
    public Image rightUI;
    public void leftImageUIStatusChange(bool status)
    {
        Debug.Log("leftUI changed");
        if(status)
        {
            // 粘性的
            leftUI.enabled = false;
        }
        else
        {
            leftUI.enabled = true;
        }
    }

    public void rightImageUIStatusChange(bool status)
    {
        Debug.Log("rightUI changed");
        if (status)
        {
            // 粘性的
            rightUI.enabled = false;
        }
        else
        {
            rightUI.enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        leftUI.enabled = false;
        rightUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
