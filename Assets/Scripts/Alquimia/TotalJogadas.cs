using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Alquimia
{
    public class TotalJogadas : MonoBehaviour
    {
        public static TotalJogadas instance;

        public int totalJogadas;
        public bool isFinished = false;
        private int jogadasFeitas = 0;
        public int score;

        [SerializeField] private TMP_Text totalJogadasObj;
        private GameManagerAlquimia gameManagerObj;

        private void Start()
        {
            gameObject.GetComponent<TMP_Text>().text = totalJogadas.ToString();
            gameManagerObj = FindObjectOfType<GameManagerAlquimia>();
        }

        public void UpdateTotalJogadas()
        {
            if (totalJogadas > 1)
            {
                totalJogadas--;
                jogadasFeitas++;

                UpdateScore();

                totalJogadasObj.text = "Parabéns!\nPontos ganhos: " + score.ToString();
                gameObject.GetComponent<TMP_Text>().text = totalJogadas.ToString();
            }
            else if ((totalJogadas == 1) && (gameManagerObj.punctuationFinished == gameManagerObj.punctuationsInScene.Length))
            {
                totalJogadas--;
                jogadasFeitas++;

                UpdateScore();

                totalJogadasObj.text = "Parabéns!\nPontos ganhos: " + score.ToString();
                gameObject.GetComponent<TMP_Text>().text = totalJogadas.ToString();
            }
            else if ((totalJogadas == 1) && (gameManagerObj.punctuationFinished != gameManagerObj.punctuationsInScene.Length))
            {
                totalJogadas--;
                jogadasFeitas++;

                UpdateScore();

                totalJogadasObj.text = "Parabéns!\nPontos ganhos: " + score.ToString();
                gameObject.GetComponent<TMP_Text>().text = totalJogadas.ToString();
                isFinished = true;
            }
        }

        private void UpdateScore()
        {
            if (jogadasFeitas <= totalJogadas / 3)
            {
                score = 9000;
            }
            else if ((jogadasFeitas > totalJogadas / 3) && (jogadasFeitas <= (totalJogadas / 3) * 2))
            {
                score = 5000;
            }
            else if (jogadasFeitas > (totalJogadas / 3) * 2)
            {
                score = 1000;
            }
        }
    }
}
