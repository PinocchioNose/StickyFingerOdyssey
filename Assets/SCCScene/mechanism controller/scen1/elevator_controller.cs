using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator_controller : MonoBehaviour
{
    public GameObject up_button, down_button;
    public GameObject[] body;
    public GameObject father;
    public float max_height, min_height;
    public float min_speed;
    float speed, accelerate;
    public float max_speed;
    public int state = 0;                              //0 stop;1 up;2 down

    void Start()
    {
        accelerate = max_speed * max_speed / (max_height - min_height);
        max_height += father.transform.position.y;
        min_height += father.transform.position.y;
    }

    void FixedUpdate()
    {
        if(state == 1)
        {
            if (gameObject.transform.position.y >= max_height)
            {
                state = 0;
            }
            for (int i=0;i<body.Length;i++)
            {
                body[i].transform.position += new Vector3(0.0f, speed, 0.0f);
            }
            if(gameObject.transform.position.y >= max_height)
            {
                state = 0;
            }
        }
        if (state == 2)
        {
            if (gameObject.transform.position.y <= min_height)
            {
                state = 0;
            }
            for (int i = 0; i < body.Length; i++)
            {
                body[i].transform.position -= new Vector3(0.0f, speed, 0.0f);
            }
            if (gameObject.transform.position.y <= min_height)
            {
                state = 0;
            }
        }
        if (state != 0)
        {
            speed = cal_speed();
        }
    }

    float cal_speed()
    {
        float spd;
        if(state == 0)
        {
            return min_speed;
        }
        if((state == 1 && gameObject.transform.position.y <= 0.5*(max_height - min_height)) || (state == 2 && gameObject.transform.position.y >= 0.5 * (max_height - min_height)))
        {
            float deltax = Mathf.Abs((max_height + min_height)/2 - gameObject.transform.position.y);
            float spd2 = max_speed * max_speed - 2 * accelerate * deltax;
            if (spd2 <= 0)
            {
                spd = min_speed;
            }
            else
            {
                spd = Mathf.Sqrt(spd2);
            }
            /*Debug.Log(deltax);
            Debug.Log(max_speed * max_speed - 2 * accelerate * deltax);*/
        }
        else if(state == 1 && gameObject.transform.position.y > 0.5 * (max_height - min_height))
        {
            spd =  Mathf.Sqrt(2*accelerate*(max_height - gameObject.transform.position.y));
        }
        else
        {
            spd =  Mathf.Sqrt(2 * accelerate * (gameObject.transform.position.y - min_height));
        }

        spd = Mathf.Max(min_speed, spd);
        return spd;
    }
}
