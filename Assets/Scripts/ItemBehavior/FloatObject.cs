using UnityEngine;
using System.Collections;

public class FloatObject : MonoBehaviour {
    private bool isInWater = false;
    private GameObject Water;
    private float waterY;
    private const float floatageForce = 0;
    private const float density = 1;
    private const float g = 9.8f;
    private const float waterDrag = 5;


    public bool getIsInWater()
    {
        return isInWater;
    }

    public void setIsInWater(bool isInWater)
    {
        this.isInWater = isInWater;
    }

    private void Start()
    {
        isInWater = false;
        Water = GameObject.FindWithTag("Water");
    }

    private void FixedUpdate()
    {
        if (isInWater)
        {
            Debug.Log("is in water");
            calFloatage();
            GetComponent<Rigidbody>().drag = waterDrag;
        }
    }

    void calFloatage()
    {
        waterY = 0.85f; // Water.transform.position.y

        if (waterY > (transform.position.y - transform.localScale.y))
        {
            float h = waterY - (transform.position.y - transform.localScale.y / 2) > transform.localScale.y ? transform.localScale.y : waterY - (transform.position.y - transform.localScale.y / 2);
            float floatageForce = density * g * transform.localScale.x * transform.localScale.z * h;
            GetComponent<Rigidbody>().AddForce(0, floatageForce, 0);
        }
    }

}
