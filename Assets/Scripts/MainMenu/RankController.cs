using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EducaNuclear
{
    public class RankController : MonoBehaviour
    {
        [SerializeField] private TMP_Text bestPunctuationTxt;

        private void Start()
        {
            SaveAndLoad.LoadPlayerData();

            /*if (PlayerPrefs.HasKey("TotalScore"))
            {
                if (SaveAndLoad.playerData.punctuation > PlayerPrefs.GetInt("BestScore"))
                {
                    PlayerPrefs.SetInt("BestScore", SaveAndLoad.playerData.punctuation);
                    
                }

               
            }
            else
            {
                PlayerPrefs.SetInt("TotalScore", SaveAndLoad.playerData.punctuation);
                bestPunctuationTxt.text = "Melhor Pontuação: " + PlayerPrefs.GetInt("BestScore").ToString();
            }*/

            bestPunctuationTxt.text = "Pontuação total: " + PlayerPrefs.GetInt("TotalScore").ToString();
        }
    }
}

