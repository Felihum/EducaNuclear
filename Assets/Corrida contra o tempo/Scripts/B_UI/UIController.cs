using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Corrida
{
    public class UIController : MonoBehaviour
    {
        public void LoadSenes(string cena)
        {
            SceneManager.LoadScene(cena);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }

}

