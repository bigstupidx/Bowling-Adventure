using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	
	
	
	public void LoadMenu(string sceneName)
	{
		Application.LoadLevel(sceneName);
	}
	
	
	public void LoadGame(string gameName)
	{
		Application.LoadLevel(gameName);
	}
}
