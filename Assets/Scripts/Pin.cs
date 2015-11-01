using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour {

	
	
	public enum PinStance { Upright, Defeated }
 	PinStance _stance;
	
	Transform body;
	
	void Start()
	{
		body = transform.FindChild("body").transform;
	}
	
	void LateUpdate()
	{	
		
		Vector3 centerOfBody = body.GetComponent<MeshRenderer>().bounds.center;
		
		Ray ray = new Ray(centerOfBody, body.up);
		RaycastHit hit;
		bool hitFound = Physics.Raycast(ray, out hit);	
		
		Debug.DrawRay(centerOfBody, body.up, Color.red);
		
		if(hitFound)
		{
			if(!hit.Equals(null) && !hit.collider.Equals(null))
			{
				if(hit.collider.gameObject.tag == Hash.Tags.PinCeiling)
				{
					_stance = PinStance.Upright;
				}
			else _stance = PinStance.Defeated;
			}
		}
		else _stance = PinStance.Defeated;
	
	}
	
	public PinStance Stance()
	{
		return _stance;
	
	}
}
