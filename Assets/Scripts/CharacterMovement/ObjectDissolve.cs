using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDissolve : MonoBehaviour
{
    private Material _material;
    private Texture2D _DissolveMap;
    void Start()
    {
        _material = gameObject.GetComponent<Renderer>().material;
        _material.SetTexture("_DissolveMap", TextureCreator.texture_static);
    }

    // Update is called once per frame
    void Update()
    {
        _material.SetFloat("_DissolveDistance", CameraFollow.DistanceBetweenCamAndSpbtm);
        //_material.SetTexture("_DissolveMap", TextureCreator.texture_static);
    }
}
