using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EducaNuclear
{
    public class NewGameController : MonoBehaviour
    {
        [SerializeField] private GameObject JogarBtn;

        private void Start()
        {
            SaveAndLoad.LoadPlayerData();
            if (SaveAndLoad.playerData != null)
            {
                if (SaveAndLoad.playerData.currentPhase > 0)
                {
                    JogarBtn.SetActive(true);
                }
            }
                
        }
    }
}

