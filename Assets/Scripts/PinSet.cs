using UnityEngine;
using System.Collections;

public class PinSet : MonoBehaviour {

	
	
	
	public int _uprightPins;
	Pin[] pins;
	
	// Use this for initialization
	void Start () {
		pins = GetComponentsInChildren<Pin>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

		int i = 0;
		foreach(Pin pin in pins)
		{
			if(pin.Stance() == Pin.PinStance.Upright)
				i++ ;
		}
		
		_uprightPins = i;

		
	}
	
	public int DefeatedPinsCount()
	{
		
		return pins.Length - _uprightPins;
	
	}
}
