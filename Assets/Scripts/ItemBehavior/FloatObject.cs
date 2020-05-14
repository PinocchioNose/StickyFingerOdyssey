using UnityEngine;
using System.Collections;

public class FloatObject : MonoBehaviour {
    [Tooltip("水面高度")]
    public float waterLevel;
    public float floatHeight;
    [Tooltip("漂浮中心和物体质心之间的偏移")]
	public Vector3 buoyancyCentreOffset;
    [Tooltip("阻尼")]
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
