using UnityEngine;
using System.Collections;

public class CameraFollowBall : MonoBehaviour {

	

	public float cameraDistance = 1.0f;
	public float cameraHeight = 0.33f;
	public float cameraSmooth = 3.0f;
	
	public Ball ball;
	public bool isFollowing;
	
	
	// Use this for initialization
	void Start () {
	
		
		ball = GameObject.FindObjectOfType<Ball>();
		
		CanFollowBall();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		if(isFollowing)
		//When pivot is at the center of the mesh
		//transform.position = Target.position + new Vector3(0,0.3f,-cameraDistance);
		{
			//When pivot is outside the center of the mesh
			
			Vector3 newPos = new Vector3(0,cameraHeight,-cameraDistance);
			Vector3 ballCenter = ball.gameObject.GetComponent<MeshRenderer>().bounds.center;
			
			transform.position = Vector3.Lerp(transform.position,  ballCenter + newPos, cameraSmooth * Time.deltaTime);
		}
		
		CanFollowBall();
	}
	
	
	
	void CanFollowBall ()
	{
		if(ball.ballState != Ball.BallState.OutOfBounds && ball.ballState != Ball.BallState.Goal) 
			isFollowing = true;
		else
			isFollowing = false;
	}
}
