using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeXCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private float beginTime;
    private float ranTime;
    private Animation ani;
    private bool canPlay;
    void Start()
    {
        canPlay = false;
        ani = this.GetComponent<Animation>();
        beginTime = Time.time;
        Random.InitState((int)((this.transform.position.x + this.transform.position.z)*100));
        ranTime = Random.Range(0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - beginTime > ranTime && !canPlay)
        {
            Debug.Log("treeX begin!");
            canPlay = true;
            
        }
        if(canPlay)
        {
            ani.Play("TreeX");
        }
    }
}
