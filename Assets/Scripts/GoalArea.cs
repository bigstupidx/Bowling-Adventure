using UnityEngine;
using System.Collections;

public class GoalArea : MonoBehaviour {


	void OnTriggerEnter(Collider col)
	{
	
		Ball ball = col.gameObject.GetComponent<Ball>();
		
		if(ball)
			ball.SetBallState(Ball.BallState.Goal);
	
	
	}
}
