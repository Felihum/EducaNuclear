using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EducaNuclear
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private Navigation navigationRef;
        [SerializeField] private GameObject playBtn;
        [SerializeField] private GameObject newGameBtn;
        [SerializeField] private GameObject sairBtn;
        [SerializeField] private GameObject characterSelectionObj;
        [SerializeField] private GameObject logoObj;


        public void OpenCharacterSelection()
        {
            if (playBtn.activeSelf)
            {
                playBtn.SetActive(false);
            }

            newGameBtn.SetActive(false);
            logoObj.SetActive(false);
            sairBtn.SetActive(false);
            characterSelectionObj.SetActive(true);
        }

        public void SelectFemale()
        {
            PlayerPrefs.SetString("Gender", "female");
            navigationRef.NewGame();
        }

        public void SelectMale()
        {
            PlayerPrefs.SetString("Gender", "male");
            navigationRef.NewGame();
        }
    }
}

