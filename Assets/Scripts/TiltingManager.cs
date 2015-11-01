using UnityEngine;
using System.Collections;

public class TiltingManager : MonoBehaviour {

	
	public enum TiltingDirection{None, Left, Right }
	
	public TiltingDirection Direction;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float ZTilting = Input.acceleration.x;
		
		if(ZTilting < -0.3)
			Direction = TiltingDirection.Left;
		
		else if(ZTilting > 0.3)
			Direction = TiltingDirection.Right;
		
		else
			Direction = TiltingDirection.None;
				
		//Debug.Log (Direction + " " + ZTilting );
		//Debug.Log(Input.deviceOrientation);
	}
}
