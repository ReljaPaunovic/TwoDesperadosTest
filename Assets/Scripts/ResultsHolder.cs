using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keep information about a run in a struct
public struct RunResult
{
    public RunResult(int sizeOfMap, int numberOfObstacles, List<string> algNames, List<int> numOfFieldsChecked, List<float> timePerAlg)
    {
        mapSize = sizeOfMap;
        obstacleCount = numberOfObstacles;
        algorithmNames = algNames;
        numberOfFieldsChecked = numOfFieldsChecked;
        timeSpent = timePerAlg;
    }

    public int mapSize;
    public int obstacleCount;
    public List<string> algorithmNames;
    public List<int> numberOfFieldsChecked;
    public List<float> timeSpent;
}

public static class ResultsHolder
{
    //Static class, not atached to any objects, used as data holder
    public static List<RunResult> runResults = new List<RunResult>();
}
