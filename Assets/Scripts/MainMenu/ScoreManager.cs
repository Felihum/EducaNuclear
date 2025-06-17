using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private static int bestScoreCorrida;
    private static int bestScoreAlquimia;
    private static int bestScoreColeta;
    private static int bestScoreTrunfo;

    private static int currentAlquimiaTotalScore;
    private static int currentCorridaTotalScore;
    private static int currentColetaTotalScore;
    private static int currentTrunfoTotalScore;

    private static int totalScore;


    //---------------------------------------- TOTAL SCORE ----------------------------------------

    public static int GetTotalScore()
    {
        if (PlayerPrefs.HasKey("TotalScore"))
        {
            totalScore = PlayerPrefs.GetInt("TotalScore");
        }

        return totalScore;
    }

    public static void UpdateTotalScore()
    {
        totalScore = 0;

        int alquimiaScore = GetBestAlquimiaScore();
        int coletaScore = GetBestColetaScore();
        int trunfoScore = GetBestTrunfoScore();
        int corridaScore = GetBestCorridaScore();

        totalScore = alquimiaScore + coletaScore + trunfoScore + corridaScore;

        PlayerPrefs.SetInt("TotalScore", totalScore);

    }


    //---------------------------------------- ALQUIMIA SCORE ----------------------------------------


    public static int GetBestAlquimiaScore()
    {
        bestScoreAlquimia = 0;

        if (PlayerPrefs.HasKey("BestAlquimiaScore"))
        {
            bestScoreAlquimia = PlayerPrefs.GetInt("BestAlquimiaScore");
        }

        return bestScoreAlquimia;
    }

    public static int GetCurrentAlquimiaTotalScore()
    {
        if (PlayerPrefs.HasKey("CurrentAlquimiaTotalScore"))
        {
            currentAlquimiaTotalScore = PlayerPrefs.GetInt("CurrentAlquimiaTotalScore");
        }

        return currentAlquimiaTotalScore;
    }

    public static void UpdateBestAlquimiaScore(int score)
    {
        bestScoreAlquimia = GetBestAlquimiaScore();

        if (score > bestScoreAlquimia)
        {
            PlayerPrefs.SetInt("BestAlquimiaScore", score);
            UpdateTotalScore();
        }
        
    }

    public static void UpdateCurrentAlquimiaTotalScore(int score)
    {
        if (score == -1)
        {
            if (PlayerPrefs.HasKey("CurrentAlquimiaTotalScore"))
            {
                PlayerPrefs.SetInt("CurrentAlquimiaTotalScore", 0);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("CurrentAlquimiaTotalScore"))
            {
                currentAlquimiaTotalScore = PlayerPrefs.GetInt("CurrentAlquimiaTotalScore");
            }

            currentAlquimiaTotalScore += score;

            PlayerPrefs.SetInt("CurrentAlquimiaTotalScore", currentAlquimiaTotalScore);
        }
    }


    //---------------------------------------- COLETA SCORE ----------------------------------------


    public static int GetBestColetaScore()
    {
        bestScoreColeta = 0;

        if (PlayerPrefs.HasKey("BestColetaScore"))
        {
            bestScoreColeta = PlayerPrefs.GetInt("BestColetaScore");
        }

        return bestScoreColeta;
    }

    public static int GetCurrentColetaTotalScore()
    {
        if (PlayerPrefs.HasKey("CurrentColetaTotalScore"))
        {
            currentColetaTotalScore = PlayerPrefs.GetInt("CurrentColetaTotalScore");
        }

        return currentColetaTotalScore;
    }

    public static void UpdateBestColetaScore(int score)
    {
        bestScoreColeta = GetBestColetaScore();

        if (score > bestScoreColeta)
        {
            PlayerPrefs.SetInt("BestColetaScore", score);
            UpdateTotalScore();
        }
    }

    public static void UpdateCurrentColetaTotalScore(int score)
    {
        if (score == -1)
        {
            if (PlayerPrefs.HasKey("CurrentColetaTotalScore"))
            {
                PlayerPrefs.SetInt("CurrentColetaTotalScore", 0);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("CurrentColetaTotalScore"))
            {
                currentColetaTotalScore = PlayerPrefs.GetInt("CurrentColetaTotalScore");
            }

            currentColetaTotalScore += score;

            PlayerPrefs.SetInt("CurrentColetaTotalScore", currentColetaTotalScore);
        }
    }


    //---------------------------------------- TRUNFO SCORE ----------------------------------------


    public static int GetBestTrunfoScore()
    {
        bestScoreTrunfo = 0;

        if (PlayerPrefs.HasKey("BestTrunfoScore"))
        {
            bestScoreTrunfo = PlayerPrefs.GetInt("BestTrunfoScore");
        }

        return bestScoreTrunfo;
    }

    public static int GetCurrentTrunfoTotalScore()
    {
        if (PlayerPrefs.HasKey("CurrentTrunfoTotalScore"))
        {
            currentTrunfoTotalScore = PlayerPrefs.GetInt("CurrentTrunfoTotalScore");
        }

        return currentTrunfoTotalScore;
    }

    public static void UpdateBestTrunfoScore(int score)
    {
        bestScoreTrunfo = GetBestTrunfoScore();

        if (score > bestScoreTrunfo)
        {
            PlayerPrefs.SetInt("BestTrunfoScore", score);
            UpdateTotalScore();
        }

    }

    public static void UpdateCurrentTrunfoTotalScore(int score)
    {
        if (score == -1)
        {
            if (PlayerPrefs.HasKey("CurrentTrunfoTotalScore"))
            {
                PlayerPrefs.SetInt("CurrentTrunfoTotalScore", 0);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("CurrentTrunfoTotalScore"))
            {
                currentTrunfoTotalScore = PlayerPrefs.GetInt("CurrentTrunfoTotalScore");
            }

            currentTrunfoTotalScore += score;

            PlayerPrefs.SetInt("CurrentTrunfoTotalScore", currentTrunfoTotalScore);
        }
    }


    //---------------------------------------- CORRIDA SCORE ----------------------------------------


    public static int GetBestCorridaScore()
    {
        bestScoreCorrida = 0;

        if (PlayerPrefs.HasKey("BestCorridaScore"))
        {
            bestScoreCorrida = PlayerPrefs.GetInt("BestCorridaScore");
        }

        return bestScoreCorrida;
    }

    public static int GetCurrentCorridaTotalScore()
    {
        if (PlayerPrefs.HasKey("CurrentCorridaTotalScore"))
        {
            currentCorridaTotalScore = PlayerPrefs.GetInt("CurrentCorridaTotalScore");
        }

        return currentCorridaTotalScore;
    }

    public static void UpdateBestCorridaScore(int score)
    {
        bestScoreCorrida = GetBestCorridaScore();

        if (score > bestScoreCorrida)
        {
            PlayerPrefs.SetInt("BestCorridaScore", score);
            UpdateTotalScore();
        }

    }

    public static void UpdateCurrentCorridaTotalScore(int score)
    {
        if (score == -1)
        {
            if (PlayerPrefs.HasKey("CurrentCorridaTotalScore"))
            {
                PlayerPrefs.SetInt("CurrentCorridaTotalScore", 0);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("CurrentCorridaTotalScore"))
            {
                currentCorridaTotalScore = PlayerPrefs.GetInt("CurrentCorridaTotalScore");
            }

            currentCorridaTotalScore += score;

            PlayerPrefs.SetInt("CurrentCorridaTotalScore", currentCorridaTotalScore);
        }
    }
}
