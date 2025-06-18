using Corrida;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public TimeController timeController;
    public UIManager uiManager;

    public void Answer()
    {
        if (isCorrect)
        {
            PlayerCorrida.Instance.lives++;
            UIManager.Instance.UpdateLivesUI(PlayerCorrida.Instance.lives);

            Time.timeScale = 1;
            PlayerCorrida.Instance.telaDePegunta.SetActive(false);
            Debug.Log("Resposta Certa");
            PlayerCorrida.Instance.questionsAnswered++;
            uiManager.UpdateQuestionPointsUI();
            Debug.Log(PlayerCorrida.Instance.questionsAnswered);
           
        }
        else
        {
            Time.timeScale = 1;
            PlayerCorrida.Instance.telaDePegunta.SetActive(false);
            /*PlayerCorrida.Instance.telaDeDerrota.SetActive(true);*/
            Debug.Log("Resposta Errada");

            /*TimeController.Instance.GetComponent<TimeController>().StopTimer();*/
            
        }
    }
}
