using UnityEngine;
using System.Collections;

public class FloatObject : MonoBehaviour {
    [Tooltip("ˮ��߶�")]
    public float waterLevel;
    public float floatHeight;
    [Tooltip("Ư�����ĺ���������֮���ƫ��")]
	public Vector3 buoyancyCentreOffset;
    [Tooltip("����")]
	public float bounceDamp;
	
	

	void FixedUpdate () 
    {
		Vector3 buoyancyActionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		float forceFactor = 1f - ((buoyancyActionPoint.y - waterLevel) / floatHeight);
		
		if (forceFactor > 0f) 
        {
			Vector3 uplift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * bounceDamp);
			GetComponent<Rigidbody>().AddForceAtPosition(uplift, buoyancyActionPoint);
		}
	}
}
