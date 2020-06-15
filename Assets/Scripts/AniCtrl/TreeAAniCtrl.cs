using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAAniCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private Animation ani;
    void Start()
    {
        ani = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.Play("MainTreeA");
    }
}
