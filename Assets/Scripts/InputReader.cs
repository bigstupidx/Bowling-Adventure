using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


/// <summary>
/// Input reader. It reads whatever input the user is feeding into the device, Can talk directly to the Character (Ball)
/// </summary>
public class InputReader : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler// required interface when using the OnPointerEnter method.

{

	public TiltingManager.TiltingDirection TiltingDirectionToListen;
	public TiltingManager tiltingManager;
	
	public Ball.Tilt BallTiltingProperty;
	public Ball ball;
	
	bool pointerIsOn;
	
	
	/// <summary>
	/// Find the ball that will be affected by the input
	/// </summary>
	void OnStart()
	{
		ball = GameObject.Find ("Ball").gameObject.GetComponent<Ball>();
		tiltingManager = GameObject.Find("TiltingManager").gameObject.GetComponent<TiltingManager>();
		
	}
	
	void Update()
	{

		if(pointerIsOn || tiltingManager.Direction == TiltingDirectionToListen)
		{
			ball.ApplyTilt(BallTiltingProperty);
		}
		
	}
	
	
	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter (PointerEventData eventData) 
	{
		pointerIsOn = true;
	}
	
	public void OnPointerExit (PointerEventData eventData) 
	{
		pointerIsOn = false;
	}
	
}
