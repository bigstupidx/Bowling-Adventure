using UnityEngine;
using System.Collections;
using System.Collections.Generic;





[System.Serializable]
public class Frame{


	//Sets the maximum amount of score available
	int MaxScore;
		
	//First, Second and Third Turn scores
	public bool finishFirstTurn;
	public int firstTurn;
	
	public bool finishSecondTurn;
	public int secondTurn;
	
	public bool finishThirdTurn;
	public int thirdTurn;
	
	public int FrameValue;
	public bool _gotSpare;
	public bool _gotStrike;
	
	public PlayerScore.Turn NextTurn;
	public bool FrameComplete;
	
	//It's the last frame
	bool _isLastFrame;
		
	/// <summary>
	/// The frame value.
	/// </summary>
	
	
	private Frame(){}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="LevelScore+Frame"/> class.
	/// </summary>
	/// <param name="frameId">Frame identifier.</param>
	/// <param name="maxScore">Max score.</param>	
	public Frame(int maxScore)
	{
		MaxScore = maxScore;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="LevelScore+Frame"/> class. Sets the check for Last Frame
	/// </summary>
	/// <param name="frameId">Frame identifier.</param>
	/// <param name="maxScore">Max score.</param>
	/// <param name="LastFrame">If set to <c>true</c> last frame.</param>
	public Frame(int maxScore, bool lastFrame)
	{
		MaxScore = maxScore;
		if(lastFrame) _isLastFrame = true; else _isLastFrame = false;
	}
	
	public void SetTurnValue(PlayerScore.Turn turn, int turnValue)
	{
		switch(turn)
		{
			case PlayerScore.Turn.FirstTurn:
				firstTurn = turnValue;
				if(turnValue == MaxScore) 
					{
						if(!_isLastFrame) 
						{
							FrameComplete = true;
							NextTurn = PlayerScore.Turn.FirstTurn;
							finishSecondTurn = true;
						}
						else 
						{
							NextTurn = PlayerScore.Turn.SecondTurn;
						}
						
					}
				else
					{
						NextTurn = PlayerScore.Turn.SecondTurn;
					}
				finishFirstTurn = true;
				break;
				
			case PlayerScore.Turn.SecondTurn:
				secondTurn = turnValue;
				if(!ThirdTurnAwarded())
				{ 
					FrameComplete = true; 
					NextTurn = PlayerScore.Turn.FirstTurn;
				}	
				else
				{
					NextTurn = PlayerScore.Turn.ThirdTurn;
				}
				finishSecondTurn = true;
				break;
				
			case PlayerScore.Turn.ThirdTurn:
				thirdTurn = turnValue;
				FrameComplete = true;
				finishThirdTurn = true;
				break;
		}
		SetFrameValue();
		
	}
	
	void SetFrameValue()
	{
		FrameValue = firstTurn + secondTurn + thirdTurn;
	}
	
	public bool IsLastFrame()
	{
		return _isLastFrame;
	}
	
	bool gotExtraPoints = false;
	public void AddToFrameValue(int Score)
	{
		if(!gotExtraPoints)
		{
			FrameValue+= Score;
		}
		gotExtraPoints = true;
	}
	
	
	public bool gotSpare()
	{
		_gotSpare = (firstTurn < MaxScore && firstTurn + secondTurn == MaxScore);
		return _gotSpare;
	}
	
	public bool gotStrike()
	{
		_gotStrike = (firstTurn == MaxScore);
		if(_isLastFrame) _gotStrike = (secondTurn == MaxScore);
		return _gotStrike;
	}
	

	public bool ThirdTurnAwarded()
	{
		if(_isLastFrame)
		{
			bool gotMaxScore = (firstTurn + secondTurn >= MaxScore);
			
			if(gotMaxScore)
			{
				return true;
			}
		
		}	
		return false;	
	}
	

}
