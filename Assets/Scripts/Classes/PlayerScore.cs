using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScore : MonoBehaviour {

	public GameManager.PlayerIdentifier playerIdentifier;
	public enum Turn { FirstTurn, SecondTurn, ThirdTurn , End}
	public string PlayerName = "Jugador: ";
	public int MaxFrames = 10;
	public int MaxScore = 10;
	public Turn currentTurn;
	public bool _testPlayerScores;
	bool playerEndOfGame;
	int PlayerTotalScore;
	
	public List<Frame> Frames;
	
	int FramePointer = 0;
	

	
	void Update()
	{	
		//TODO: Remove test player scores
		if(_testPlayerScores)
		{
			//GoodTests
			//int[] list = {2,2, 2,2, 2,2, 2,2, 2,2, 2,2, 2,2, 2,2};
			//int[] list = {4,2, 2,8, 10, 2,2, 2,2, 7,2, 7,3, 2,2, 9,1, 0,2}; //Value 85
			//int [] list = {0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 0,0, 10,0 }; //Value 10
			int [] list = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }; // Value 300
			//int [] list = {2,6, 4,6, 5,3, 7,3, 3,2, 1,7, 9,1, 5,5, 4,2, 8,1 }; //Value 101 
			
			foreach(int l in list)
			{
				int _testScore = l;
				PlayerScores(_testScore);
			}
		}
		_testPlayerScores = false;
		
	}
	
	
	void Start()
	{
		InitializeFrames();
		PlayerName = playerIdentifier.ToString();
	}

	void InitializeFrames ()
	{
	
		Frames = new List<Frame>(MaxFrames);
		for(int i = 0; i < MaxFrames; i++)
		{
			Frame f;	
			if(i +1 < MaxFrames)
			{
				f = new Frame(MaxScore);
			}
			else
			{
				f = new Frame(MaxScore, true);
			}
			
			Frames.Add(f);
		}	
		
		currentTurn  = Turn.FirstTurn;
		
	}
	
	public void PlayerScores(int Score)
	{
		//If player hasn't run out of turns
		if(!playerEndOfGame && FramePointer < Frames.Count)
		{
			Frame thisFrame = Frames[FramePointer];
		
			thisFrame.SetTurnValue(currentTurn, Score);
			currentTurn = thisFrame.NextTurn;
			
			if(thisFrame.FrameComplete) 
			{
				FramePointer++;
			}	
			
			//Check frames for missing points because Spare or Strike
			PlayerTotalScore = 0;
			
			for(int i = 0; i < Frames.Count; i++)
			{
				Frame f = Frames[i];

				if(!f.IsLastFrame())
				{
					if(f.gotSpare() && Frames[i +1].finishFirstTurn)
					{
						int ExtraScore = Frames[i +1].firstTurn;
						f.AddToFrameValue(ExtraScore);
					}
					if(f.gotStrike())
					{
						Frame nextFrame = Frames [i + 1];
						if(!nextFrame.gotStrike () || nextFrame.IsLastFrame())
						{
							if(nextFrame.finishFirstTurn && nextFrame.finishSecondTurn)
							{
								int ExtraScore = nextFrame.firstTurn + nextFrame.secondTurn;
								f.AddToFrameValue(ExtraScore);
							}
						}
						else if(!nextFrame.IsLastFrame())
						{
							Frame Next2Frames = Frames[i +2];
							if(nextFrame.finishFirstTurn && Next2Frames.finishFirstTurn)
							{
								int ExtraScore = nextFrame.firstTurn + nextFrame.firstTurn;
								f.AddToFrameValue(ExtraScore);
							}
						}
					}	
				}
				
				PlayerTotalScore += f.FrameValue; 

				if(f.IsLastFrame() && f.FrameComplete) 
					playerEndOfGame = true;
					
				
			}
			
		}
		
	}
	
	
	
	

	
}
