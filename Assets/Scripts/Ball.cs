using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ball : MonoBehaviour {


	public enum BallState { Idle, OutOfBounds, InPlay, Goal }
	public enum Tilt { Left, Right, none }
	
	public BallState ballState;
	
	public float myForceMagnitude;
	public float BaseForceMagnitude = 1.0f;
	
	public float tiltingForce = 1.0f;
	public float sensibilityFactor = 3.0f;
	public float jumpForce = 100.0f;
	
	public GameManager gameManager;
	public GameManager.PlayerIdentifier OwnerPlayer;
	public bool PlayerReady;
	
	//local Private fields
	bool _inPlay = false;
	bool _isJumping = false;
	public Tilt _tilt = Tilt.none;
	bool _startedTouchingBall = false;
	Transform _startingPoint;
	float _timeSinceStartedTouchingBallEqualsTrue;
	Slider _forceDisplay;
	Rigidbody ballRigidBody;
	
	
	public bool _TestResetBall = false;
	
	void Start () {
	
		gameManager = GameObject.FindObjectOfType<GameManager>();
		ballRigidBody = GetComponent<Rigidbody>();
		
		ResetBall();	
		_forceDisplay = GameObject.Find("BallForceSlider").GetComponent<Slider>();
		
	}
	
	void Update () {
	
		if(gameManager.GameIsRunning())
		{
			if(!InPlay())
			{
				//Captura el frame donde empezò a detectar el dedo/mouse sobre la bola
				if (Input.GetMouseButtonDown(0)) {
					
					if(CheckForPointerOnBall())
					{
						_startedTouchingBall = true;
					}	
				}
				
				//Detecta el momento cuando suelta la bola, solamente si detectó el frame cuando comenzó a tocarla
				if(Input.GetMouseButtonUp(0) && _startedTouchingBall)
				{
					if(CheckForPointerOnBall())
					{
						_startedTouchingBall = false;
						Debug.Log(_timeSinceStartedTouchingBallEqualsTrue);
						SetBallInPlay();
					}
					else{
						_startedTouchingBall = false;
					}
					
				}
				
				//Se asegura de mostrar el tiempo que ha presionado la bola por medio del slider
				if(_startedTouchingBall)
				{
					_timeSinceStartedTouchingBallEqualsTrue += Time.deltaTime * sensibilityFactor;
					_forceDisplay.value = _timeSinceStartedTouchingBallEqualsTrue;
				}
				else
				{
					if(_timeSinceStartedTouchingBallEqualsTrue > 0) _timeSinceStartedTouchingBallEqualsTrue -= Time.deltaTime * sensibilityFactor;
					_forceDisplay.value = _timeSinceStartedTouchingBallEqualsTrue;
				}
	
			}
			
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					if(CheckForPointerOnBall() && !_isJumping)
					{
						_isJumping = true;
					}
				}
				if(Input.GetButtonDown("Jump"))
				{
					_isJumping = true;
				}
				
				if(Input.GetAxis("Horizontal") >0) ApplyTilt(Tilt.Right);
				if(Input.GetAxis("Horizontal") <0) ApplyTilt(Tilt.Left);
			}
		}
		
		//Reset ball from out of bounds after "timetoAutoReset" seconds
		if(ballState == BallState.OutOfBounds)
		{
			timeSinceOutOfBounds += Time.deltaTime;
			if(timeSinceOutOfBounds >= timeToAutoReset) ResetBall();
		}
		
		//Reset ball from Goal after "timetoAutoReset" seconds
		if(ballState == BallState.Goal)
		{
			timeSinceGoal += Time.deltaTime;
			if(timeSinceGoal >= timeToAutoReset) 
			{
				StopBall();
				ResetBall();
				
			}
		}
		
		if(_TestResetBall) {
			ResetBall();
			_TestResetBall = false;	
		}	
	}
	
	void FixedUpdate()
	{
		if(InPlay() && ballState != BallState.Goal)
		{
			//TODO:
			//Ocasiona que al saltar la bola se acelere..
			//rigidbody.AddForce(0,0, BaseForceMagnitude);
			
			//TODO:
			//Ocasiona que la velocidad si sea constante, pero está evitando el salto y el efecto de chanfle es mas lento, y los pines no afectan la fuerza
			//ballRigidBody.velocity = new Vector3(0,0,myForceMagnitude);
			ballRigidBody.AddForce(0,0,myForceMagnitude);
		}
	
		if(InPlay())
		{
			switch(_tilt)
			{
			case Tilt.Left:
				ballRigidBody.AddForce(new Vector3(-tiltingForce, 0f,0f));
				break;
				
			case Tilt.Right:
				ballRigidBody.AddForce(new Vector3(tiltingForce, 0f,0f));
				break;
				
			case Tilt.none:
				
				break;
			}
		}
		
		if(_isJumping && IsGrounded())
		{
			ballRigidBody.AddForce(new Vector3(0,jumpForce,0), ForceMode.VelocityChange);
			_isJumping = false;
		}
	
	}

	//get setter of ball.InPlay
	void SetBallInPlay ()
	{
		ThrowBall();
		if(_timeSinceStartedTouchingBallEqualsTrue < 3)
		{
			myForceMagnitude = BaseForceMagnitude * _timeSinceStartedTouchingBallEqualsTrue;
		}
		else
		{
			myForceMagnitude = BaseForceMagnitude * 3;
		}
		
		//TODO:
		//Ocasiona el efecto de que la bola inicia muy rápido y se alenta
		//Mover la bola agregando una Constant Force
		//rigidbody.velocity = new Vector3(0,0,BaseForceMagnitude);
		//gameObject.AddComponent<ConstantForce>().force = new Vector3(0,0,BaseForceMagnitude);
		//Debug.Log(BaseForceMagnitude);
		
		ballRigidBody.AddForce(new Vector3(0,0,myForceMagnitude), ForceMode.VelocityChange);
	}

	bool CheckForPointerOnBall ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		bool hitFound =	 Physics.Raycast(ray, out hit);
		
		Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.red, 10.0f);
		
		if(hitFound){
		if(!hit.Equals(null) && !hit.collider.Equals(null))
			if(hit.collider.name.Equals(gameObject.name))
				return true;
		}
		return false;
	}
	
	bool IsGrounded()
	{
		//If mesh is on the center
		//Ray ray = new Ray(transform.position, Vector3.down);
		
		//If mesh its outside the center
		Ray ray = new Ray(transform.gameObject.GetComponent<MeshRenderer>().bounds.center, Vector3.down);
		
		RaycastHit hit = new RaycastHit();
		bool hitFound = Physics.Raycast(ray, out hit);
	
		//Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.green, 10.0f);
		//Debug.DrawRay(transform.gameObject.GetComponent<MeshRenderer>().bounds.center, Vector3.down, Color.green, 100.0f);
		if(hitFound)
		{
			if(hit.distance > 0.2f)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		return false;
	}
	
	public void ThrowBall()
	{
		SetBallState(BallState.InPlay);
		_inPlay = true;	
	}
	
	public void StopBall()
	{
		_inPlay = false;
	}
	
	public bool InPlay()
	{
		return _inPlay;
	}
	
	public void ApplyTilt(Tilt tilt)
	{
		if(tilt != Tilt.none)
			_tilt = tilt;
		else
			_tilt = Tilt.none;
	}

	float timeSinceOutOfBounds;
	float timeSinceGoal;
	
	float timeToAutoReset = 3.0f;
	public void SetBallState (BallState newBallState)
	{
		ballState = newBallState;
		
		switch (ballState)
		{
			case BallState.OutOfBounds:
				StopBall();
				timeSinceOutOfBounds = 0f;
				break;
			
			case BallState.Goal:
				timeSinceGoal = 0f;
				break;
			
			default:
				timeSinceOutOfBounds = 0f;
				break;	
			
		}
			
	}

	void ResetBall ()
	{
		
		myForceMagnitude = BaseForceMagnitude;
		
		ballRigidBody.velocity = Vector3.zero;
		ballRigidBody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		
		//La bola buscara el punto inicial de cada nivel
		_startingPoint =  GameObject.FindGameObjectWithTag(Hash.Tags.StartingPoint).transform;
		if(_startingPoint) 
		{
			//Cuando el pivote está en el centro del mesh renderer
			transform.position = transform.position - transform.gameObject.GetComponent<MeshRenderer>().bounds.center + _startingPoint.position;
			
			//Locate above ground
			transform.Translate(new Vector3(0.0f, transform.localScale.y / 2, transform.localScale.z /2));
			
			
		}
		
		else
		{ throw new UnityException("La bola no encontró un punto de inicio");	}
		
		SetBallState(BallState.Idle);
	}

}
