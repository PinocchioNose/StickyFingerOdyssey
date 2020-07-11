using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideInput : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeCount = 1.0f;
    public float forceOffset = 0.1f;
    public float force = 10f;
    private bool deformFlag=false;
    private Vector3 forcePoint;
    private float _time;
    private bool no_recover = false;

    private MeshDeformer deformer;

    void Start()
    {
        Debug.Log(TimeCount);
        _time = TimeCount;
        if(TimeCount<0)
            no_recover = true;
        deformer = this.gameObject.GetComponent<MeshDeformer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(deformer){
            if(no_recover && deformFlag){
                deformer.AddDeformingForce(forcePoint,force);
            }
            else if(_time>0 && deformFlag==true){
                deformer.AddDeformingForce(forcePoint,force);
                _time -= Time.deltaTime;
            }
            else if(_time<=0){
                deformFlag = false;
                _time = TimeCount;
            }
            
        }
        else
        {
            Debug.Log("deformer err");
        }
        // Debug.Log(test);
    }

    private void OnCollisionEnter(Collision collision){
        // Debug.Log(collision.GetContact(0));
        ContactPoint CPoint = collision.contacts[0];
        Vector3 point = CPoint.point;
        forcePoint=point-CPoint.normal*forceOffset;
        deformFlag=true;
        Debug.Log(CPoint.point);
    }
}
