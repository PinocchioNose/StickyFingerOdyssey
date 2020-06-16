using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    public float accelerate;
    public GameObject camera1;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * -accelerate);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * accelerate);
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * accelerate);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * -accelerate);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * accelerate);
        }
        camera1.transform.position = gameObject.transform.position + new Vector3(0.0f,2.0f,5.0f);
    }

    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag != "ground")
        {
            Debug.Log(co.gameObject.name);
        }
    }
}
