using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Corrida;
using UnityEngine.Rendering;


public class QuizManager : MonoBehaviour
{
    public List<QuestionadnAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public TMP_Text QuestionTxt;


    public static QuizManager instance { get; set; }

    private void Start()
    {
        //generatorQuestion();
    }

    public void GerarPergunta()
    {
        generatorQuestion();
    }

    public void correct()
    {
        QnA.RemoveAt(currentQuestion);
        generatorQuestion();
    }


    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

    }

    void generatorQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswer();
        }
        else
        {
            Debug.Log("Out of Questions");
        }
    }
}

