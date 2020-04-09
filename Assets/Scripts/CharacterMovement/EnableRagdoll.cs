using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRagdoll : MonoBehaviour
{
    int count = 0;
    void setKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }
    
    void Start()
    {
        setKinematic(true);

    }

    // Update is called once per frame
    void Update()
    {
        //++count;
        //if (count > 500)
        //    setKinematic(false);
    }
}
