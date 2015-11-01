using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	
	public enum PlayerIdentifier { Player1, Player2, Player3, Player4 }
	
	//public enum LevelSelection { Desert, Tropical, Snow, Future, Egypt, Mayan, Candy, City }

	public GameSettings.NumberOfPlayersSelected numberOfPlayers;
	
	
	public PlayerIdentifier currentPlayer;
 	
 	
 	bool _gameStarted;
 	bool _playerScoresCreated;
 	
 	void Start()
 	{
 		
 	}
			
	
	public bool GameIsRunning(){
		return _gameStarted;
	}
	
	public void StartGame(){
	
		_gameStarted = true;	
	}
	
	public void PauseGame(){
		_gameStarted = !_gameStarted;
		
		if(_gameStarted) Time.timeScale = 0.000000001f;
		else Time.timeScale = 1.0f;
	}
	
	public void ResetScene(){
		
		Application.LoadLevel(Application.loadedLevel);
	}

	void CreatePlayerScores()
	{
		
		PlayerScore[] ls1 = gameObject.GetComponentsInChildren<PlayerScore>();
		
		foreach(PlayerScore ls in ls1)
		{
			Destroy(ls.gameObject);
		}		
				
		int number = 0;
		switch(numberOfPlayers){
			
			case GameSettings.NumberOfPlayersSelected.OnePlayer:
				number = 1;
				break;
			case GameSettings.NumberOfPlayersSelected.TwoPlayers:
				number = 2;
				break;
			case GameSettings.NumberOfPlayersSelected.ThreePlayers:
				number = 3;
				break;	
			case GameSettings.NumberOfPlayersSelected.FourPlayers:
				number = 4;
				break;
		}
		
		for(int i = 0; i < number; i++)
		{
			GameObject go = new GameObject();
			go.transform.SetParent(gameObject.transform);
			
			PlayerScore ls = go.AddComponent<PlayerScore>();			
			if(i ==0) {
				go.name = PlayerIdentifier.Player1.ToString() + "'s score";
				ls.playerIdentifier = PlayerIdentifier.Player1;
			}
			if(i ==1) {
				go.name = PlayerIdentifier.Player2.ToString() + "'s score";
				ls.playerIdentifier = PlayerIdentifier.Player2;
			}
			if(i ==2) {
				go.name = PlayerIdentifier.Player3.ToString() + "'s score";
				ls.playerIdentifier = PlayerIdentifier.Player3;
			}
			if(i ==3) {
				go.name = PlayerIdentifier.Player4.ToString() + "'s score";
				ls.playerIdentifier = PlayerIdentifier.Player4;
			}
		}

	}

	public bool _testscore;
	void Update()
	{	
		//TODO: Remove Test Player Scores
		if(_testscore)
		{
			CreatePlayerScores();
		}
		_testscore = false;
		
		if(_gameStarted)
		{
			if(!_playerScoresCreated)
			{
			 	CreatePlayerScores();
			}
			_playerScoresCreated = true;
		}
	}
	
	
	public void ExitToMenu()
	{
	
		//Todo: SAVE CURRENT GAME
		
		
	
		Application.LoadLevel("1a_MainMenu");
	}
}
