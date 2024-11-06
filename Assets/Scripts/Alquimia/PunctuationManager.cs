using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Alquimia
{
    public class PunctuationManager : MonoBehaviour
    {
        public Color color;
        private int punctuation;
        public int totalPunctuation;
        public bool isFinished;
        [SerializeField] private GameManagerAlquimia gameManagerRef;
        private TotalPunctuation[] totalPoints;

        private void Start()
        {
            gameObject.GetComponent<TMP_Text>().SetText(punctuation.ToString());
            totalPoints = FindObjectsOfType<TotalPunctuation>();

            foreach (TotalPunctuation totalPoint in totalPoints)
            {
                if (totalPoint.color == color)
                {
                    totalPoint.GetComponent<TMP_Text>().text = "/ " + totalPunctuation.ToString();
                }
            }
        }

        public void UpdatePunctuation(int punctuationCount)
        {
            punctuation += punctuationCount;

            if (punctuation >= totalPunctuation && isFinished == false)
            {
                isFinished = true;
                gameManagerRef.punctuationFinished++;
                punctuation = totalPunctuation;
                gameObject.GetComponent<TMP_Text>().SetText(punctuation.ToString());
            }

            if (!isFinished)
            {
                gameObject.GetComponent<TMP_Text>().SetText(punctuation.ToString());
            }
        }
    }
}
