using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float TimeCount=1.0f;
    public float forceOffset = 0.1f;
    public float force = 10f;
    private bool deformFlag=false;
    private Vector3 forcePoint;
    // Update is called once per frame
    void Update()
    {
        if(TimeCount>0 && deformFlag==true){
            MeshDeformer deformer = this.gameObject.GetComponent<MeshDeformer>();
            if(deformer){
                deformer.AddDeformingForce(forcePoint,force);
                TimeCount-=Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision){
        ContactPoint CPoint = collision.contacts[0];
        Vector3 point = CPoint.point;
        forcePoint=point-CPoint.normal*forceOffset;
        deformFlag=true;
    }
}
