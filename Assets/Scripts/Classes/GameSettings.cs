using UnityEngine;
using System.Collections;
using System.Xml;

public class GameSettings : MonoBehaviour{

	
	public enum GameMode { Adventure, QuickGame }
	public enum NumberOfPlayersSelected { OnePlayer, TwoPlayers, ThreePlayers, FourPlayers }
	public enum LevelSelection { Tropical, Snow, Future, Desert, Egypt, Mayan, Candy, City }
	public enum AdventureMode { NewGame, Continue }
	public enum GameSave { SaveFile1, SaveFile2, Savefile3 }
	
	
	GameMode _gameMode;
	NumberOfPlayersSelected _numberOfPlayers;
	LevelSelection _levelSelected;
	AdventureMode _adventureMode;
	GameSave _gameSave;
	
	public GameSettings()
	{
		_gameMode = GameMode.Adventure;
		_numberOfPlayers  = NumberOfPlayersSelected.OnePlayer;
		_levelSelected = LevelSelection.Tropical;
		_adventureMode = AdventureMode.Continue;
		_gameSave = GameSave.SaveFile1;
	}
	
	
	
	
	
}
