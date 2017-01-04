using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParameters : MonoBehaviour {

    public static int totalFrameNumber = 10;
    public static int frameRoundsCount = 2;
    public static int lastFrameRoundsCount = 3;

    private List<int[]> frameResults = new List<int[]>();
    private int[] rounds = new int[lastFrameRoundsCount];
    private bool isLastFrame = false;
    private int currentFrameNumber = 0;
    private int roundNumber = 0;

    private readonly object syncLock = new object();

    public List<int[]> FrameResults
    {
        get
        {
            return frameResults;
        }        
    }

    public int[] Rounds
    {
        get
        {
            return rounds;
        }        
    }

    public bool IsLastFrame
    {
        get
        {
            return isLastFrame;
        }

        set
        {
            isLastFrame = value;
        }
    }

    // Increment the number of pins that have fallen
    public void incrementFallenPins()
    {
        lock (syncLock)
        {
            rounds[roundNumber] += 1;
        }        
    }

    // Initialize the next round of the frame
    public void initNextRound()
    {
        if (isLastRound())
            initNextFrame();

        if (!isLastFrame)
            roundNumber = (roundNumber + 1) % frameRoundsCount;
        else
            roundNumber = (roundNumber + 1) % lastFrameRoundsCount;

        rounds[roundNumber] = 0;
    }

    /**
     * Return true if the current round is the last of the frame, return false if not
     */
    public bool isLastRound()
    {
        return (isLastFrame && (roundNumber + 1 >= lastFrameRoundsCount)) || (!isLastFrame && (roundNumber + 1 >= frameRoundsCount));
    }

    // Initialize the next frame
    public void initNextFrame()
    {
        addFrameResult(rounds);

        currentFrameNumber++;
        if (currentFrameNumber >= totalFrameNumber)
            isLastFrame = true;

        resetRounds();

        Debug.Log("Frame results: " + frameResults.ToArray());
    }

    // Add a result to the frame result list
    private void addFrameResult(int[] frameResult)
    {
        frameResults.Add(frameResult);
    }

    // Reset all the rounds of the frame
    public void resetRounds()
    {
        foreach(int i in rounds)
            rounds[i] = 0;
    }

    // Reset all the parameters, frame results and rounds
    public void resetParameters()
    {
        currentFrameNumber = 0;
        roundNumber = 0;
        resetRounds();
        frameResults.Clear();
        isLastFrame = false;
    }

    // Use this for initialization
    void Start () {
        frameResults = new List<int[]>();
        rounds = new int[lastFrameRoundsCount];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
