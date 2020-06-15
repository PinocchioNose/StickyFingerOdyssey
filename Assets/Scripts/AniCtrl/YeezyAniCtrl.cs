using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeezyAniCtrl : MonoBehaviour
{
    // Start is called before the first frame update、
    private Animation ani;
    void Start()
    {
        ani = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!ani.IsPlaying("YeezyLeaveAni"))
        {
            ani.Play("YeezyLeaveAni");
        }
    }
}
