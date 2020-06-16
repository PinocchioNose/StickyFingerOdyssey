using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cable_move : MonoBehaviour
{
    // Start is called before the first frame update
    public int path_idx = 31;
    public static Vector3[] cable_path = new Vector3[31];

    public static Vector3 det_h = new Vector3(1.512f,-5.95f,0.0f);

    public int start_idx;
    public GameObject path_father;

    int idx;
    public int inter_frame;
    int j;
    void Start()
    {
        for(int i = 0;i < path_idx;i++)
        {
            cable_path[i] = path_father.transform.GetChild(i).position;
        }
        idx = start_idx;
        j = inter_frame;
    }

    // Update is called once per frame
    void Update()
    {
        j = j + 1;
        if (j >= inter_frame)
        {
            gameObject.transform.position = cable_path[idx]+det_h;
            idx = idx + 1;
            if (idx >= path_idx)
            {
                idx = 0;
            }
            j = 0;
        }
    }
}
