using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickyUI : MonoBehaviour
{
    // 两个UI
    public GameObject leftUI;
    public GameObject rightUI;
    public GameObject leftUI_after;
    public GameObject rightUI_after;

    private float lastTime;
    public void leftImageUIStatusChange(bool status)
    {
        Debug.Log("leftUI changed");
        if(status)
        {
            // 粘性的
            leftUI.SetActive(true);
            leftUI_after.SetActive(false);
            //leftUI.enabled = true;
            //leftUI_after.enabled = false;
            lastTime = Time.time;
        }
        else
        {
            leftUI.SetActive(false);
            leftUI_after.SetActive(true);
            //leftUI.enabled = false;
            //leftUI_after.enabled = true;
            lastTime = Time.time;
        }
    }

    public void rightImageUIStatusChange(bool status)
    {
        Debug.Log("rightUI changed");
        if (status)
        {
            // 粘性的
            rightUI.SetActive(true);
            rightUI_after.SetActive(false);
            //rightUI.enabled = true;
            //rightUI_after.enabled = false;
            lastTime = Time.time;
        }
        else
        {
            rightUI.SetActive(false);
            rightUI_after.SetActive(true);
            //rightUI.enabled = false;
            //rightUI_after.enabled = true;
            lastTime = Time.time;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //leftUI.enabled = false;
        //rightUI.enabled = false;
        //leftUI_after.enabled = false;
        //rightUI_after.enabled = false;
        leftUI.SetActive(false);
        leftUI_after.SetActive(false);
        rightUI.SetActive(false);
        rightUI_after.SetActive(false);

        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > 3.0)
        {
            //Debug.Log("Disable UI");
            //leftUI.enabled = false;
            //rightUI.enabled = false;
            //leftUI_after.enabled = false;
            //rightUI_after.enabled = false;
            leftUI.SetActive(false);
            leftUI_after.SetActive(false);
            rightUI.SetActive(false);
            rightUI_after.SetActive(false);
        }
    }
}
