using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDissolve : MonoBehaviour
{
    private Material _material;
    void Start()
    {
        _material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _material.SetFloat("_DissolveDistance", CameraFollow.DistanceBetweenCamAndSpbtm - 1.5f);
    }
}
