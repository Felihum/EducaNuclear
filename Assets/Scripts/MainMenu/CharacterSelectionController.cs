using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EducaNuclear
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private Navigation navigationRef;
        [SerializeField] private GameObject characterSelectionObj;
        [SerializeField] private GameObject menuInicial;


        public void OpenCharacterSelection()
        {
            menuInicial.SetActive(false);
            characterSelectionObj.SetActive(true);
        }

        public void CloseCharacterSelection()
        {
            menuInicial.SetActive(true);
            characterSelectionObj.SetActive(false);
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

