using Alquimia;
using Coleta;
using Corrida;
using EducaNuclear;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject background;
    public GameObject HUD;
    public GameObject nextBtn;

    public GameObject introExplanation;

    public List<GameObject> tutorialObjects;
    public List<GameObject> explanationObjects;

    public GameManagerAlquimia gameManagerAlquimia;
    public TimerScript timerManagerRef;

    public Navigation navigationManager;

    public bool isCorrida;
    public bool isTrunfo;

    int cont = 0;

    private void Start()
    {
        //tutorialObjects[0].transform.SetParent(background.transform);
    }

    public void NextTutorial()
    {
        if (cont <= tutorialObjects.Count)
        {
            Debug.Log("Entrou");
            
            if (cont == 0)
            {
                introExplanation.SetActive(false);
                explanationObjects[cont].SetActive(true);
                tutorialObjects[cont].transform.SetParent(background.transform);
            }
            else if (cont < tutorialObjects.Count)
            {
                tutorialObjects[cont - 1].transform.SetParent(HUD.transform);
                tutorialObjects[cont].transform.SetParent(background.transform);
                explanationObjects[cont - 1].SetActive(false);
                explanationObjects[cont].SetActive(true);
            }
            else if(cont >= tutorialObjects.Count)
            {
                explanationObjects[cont - 1].SetActive(false);
                tutorialObjects[cont - 1].transform.SetParent(HUD.transform);
                nextBtn.SetActive(false);
                background.SetActive(false);
                
                if (gameManagerAlquimia != null)
                {
                    tutorialObjects[cont - 1].SetActive(false);
                    gameManagerAlquimia.CloseTutorial();
                }
                if (timerManagerRef != null)
                {
                    tutorialObjects[cont - 1].SetActive(false);
                    timerManagerRef.timerOn = true;
                }
                if (isCorrida == true)
                {
                    navigationManager.CloseTutorialCorrida();
                }
                if (isTrunfo == true)
                {
                    navigationManager.OpenTrunfo();
                }
            }

            cont++;
        }
    }
}
