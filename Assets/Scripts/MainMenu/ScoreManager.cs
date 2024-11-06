using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int totalScore;

    public static int GetTotalScore()
    {
        if (PlayerPrefs.HasKey("TotalScore"))
        {
            totalScore = PlayerPrefs.GetInt("TotalScore");
        }

        return totalScore;
    }

    public static void UpdateTotalScore(int score)
    {
        if (PlayerPrefs.HasKey("TotalScore"))
        {
            totalScore = PlayerPrefs.GetInt("TotalScore");
        }

        totalScore += score;
        PlayerPrefs.SetInt("TotalScore", totalScore);
    }
}
