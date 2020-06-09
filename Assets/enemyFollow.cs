using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    private Vector3 init_pos;
    private Vector3 range_forward, range_right;
    private Vector3 base_point, boundry_p1, boundry_p2;
    public int range=2;
    public bool showline = true;
    private float start_time;
    // Start is called before the first frame update
    void Start()
    {
        init_pos = this.GetComponent<Transform>().position;
        range_forward = transform.forward;
        range_right = transform.right;
        base_point = init_pos + range * range_forward + range * range_right;
        boundry_p1 = init_pos + range * range_forward - range * range_right;
        boundry_p2 = init_pos - range * range_forward + range * range_right;
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(showline)
        {
            Debug.DrawLine(init_pos, init_pos + range * range_forward, Color.red);
            Debug.DrawLine(init_pos, init_pos + range * range_right, Color.red);
            Debug.DrawLine(init_pos, base_point, Color.red);
            Debug.DrawLine(init_pos, boundry_p2, Color.red);
            Debug.DrawLine(init_pos, boundry_p1, Color.red);
        }
        RaycastHit hitinfo;
        Physics.SphereCast(transform.position, 1.0f, transform.forward, out hitinfo, range);
        FollowHit(hitinfo);
    }
    void FollowHit(RaycastHit hit){
        Transform toFollow = hit.transform;
        this.GetComponent<Transform>().LookAt(toFollow);
        
    }
}
