using UnityEngine;
using System.Collections;

public class PlayArea : MonoBehaviour {


	public bool LeftPlayArea = false;
	
	void Start(){
		
		
	
	}
	
	void OnTriggerExit(Collider col)
	{
		Ball ball = col.gameObject.GetComponent<Ball>();
		
		if(ball)
		{
			ball.SetBallState(Ball.BallState.OutOfBounds);	
			LeftPlayArea = true;
		}
	}
	
	
}
