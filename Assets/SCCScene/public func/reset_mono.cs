using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_mono : MonoBehaviour
{
    public float min_height;
    public Vector3 delta;
    Vector3 reborn_pos;
    void Start()
    {
        reborn_pos = gameObject.transform.position + delta;
        min_height = gameObject.transform.position.y - 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y <= min_height)
        {
            gameObject.transform.position = reborn_pos;
            gameObject.GetComponent<Rigidbody>().Sleep();
            gameObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
