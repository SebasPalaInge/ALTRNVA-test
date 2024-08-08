using System.Collections.Generic;

[System.Serializable]
public class Results
{
    public int total_clicks;
    public int total_time;
    public int pairs;
    public int score;
}

[System.Serializable]
public class ResultsObject
{
    public Results results;
}

