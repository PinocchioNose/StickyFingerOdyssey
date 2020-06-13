using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    private Vector3 init_pos;
    private Vector3 range_forward, range_right;
    private Vector3 A, B, C, D;
    public int range=2;
    public bool showline = true;
    private float start_time;
    // Start is called before the first frame update
    void Start()
    {
        init_pos = this.GetComponent<Transform>().position;
        range_forward = transform.forward;
        range_right = transform.right;
        A = init_pos + range * range_forward + range * range_right;
        B = init_pos + range * range_forward - range * range_right;
        C = init_pos - range * range_forward - range * range_right;
        D = init_pos - range * range_forward + range * range_right;
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(showline)
        {
            // Debug.DrawLine(init_pos, init_pos + range * range_forward, Color.red);
            // Debug.DrawLine(init_pos, init_pos + range * range_right, Color.red);
            Debug.DrawLine(init_pos, A, Color.red);
            Debug.DrawLine(init_pos, B, Color.red);
            Debug.DrawLine(init_pos, D, Color.red);
            Debug.DrawLine(init_pos, C, Color.red);

            Debug.DrawLine(transform.position, transform.position + 2 * transform.forward, Color.blue);
        }
        RaycastHit hitinfo;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(players[0].transform.Find("Armature/spbtm").name);
        /*
            todo:
            use cast while random rotating,
                find gameObject with tag "Player"
                then followhit
        */
        // Physics.SphereCast(transform.position, 1.0f, transform.forward, out hitinfo, range);
        // FollowHit();
        if(inRange(players[0].transform.Find("Armature/spbtm").position)){
            FollowTrans(players[0].transform.Find("Armature/spbtm"));
        }
        
    }
    void FollowTrans(Transform toFollow,int speed = 1){
        // transform.LookAt(toFollow);
        transform.rotation = Quaternion.LookRotation(toFollow.position-transform.position)/*Quaternion.AngleAxis(90, transform.up)*/;
        
        transform.Translate(transform.forward * 1/10 * Time.deltaTime, Space.World);
    }
    bool inRange(Vector3 pos){
        float cond1 = Vector3.Dot(Vector3.Cross(B - A, pos - A), Vector3.Cross(D - C, pos - C));
        float cond2 = Vector3.Dot(Vector3.Cross(A - D, pos - D), Vector3.Cross(C - B, pos - B));
        return cond1 * cond2 >= 0 ? true : false;
    }
}
