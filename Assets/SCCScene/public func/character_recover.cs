using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_recover : MonoBehaviour
{
    public float min_height = -15.0f;
    public Vector3 recover_pos = new Vector3(0, 0, 0);
    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y <= min_height)
        {
            gameObject.transform.position = recover_pos;
        }
    }
}
