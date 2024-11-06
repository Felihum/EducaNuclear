using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EducaNuclear
{
    public class CorrenteController : MonoBehaviour
    {
        [SerializeField] private GameObject corrente_usina_lab;
        [SerializeField] private GameObject corrente_lab_fab;
        [SerializeField] private GameObject corrente_fab_fazenda;
        [SerializeField] private GameObject corrente_fazenda_floresta;

        [SerializeField] private GameObject btn_usina;
        [SerializeField] private GameObject btn_lab;
        [SerializeField] private GameObject btn_fab;
        [SerializeField] private GameObject btn_fazenda;
        [SerializeField] private GameObject btn_floresta;

        [SerializeField] private Image map;

        [SerializeField] private List<Sprite> mapSprites;
 


        private void Start()
        {
            SaveAndLoad.LoadPlayerData();
            map.sprite = mapSprites[1];

            switch (SaveAndLoad.playerData.currentPhase)
            {
                case 2:
                    map.sprite = mapSprites[2];

                    btn_usina.SetActive(true);
                    break;
                case 3:
                    map.sprite = mapSprites[3];
                    corrente_usina_lab.SetActive(true);

                    btn_usina.SetActive(true);
                    btn_lab.SetActive(true);
                    break;
                case 4:
                    map.sprite = mapSprites[0];
                    corrente_usina_lab.SetActive(true);
                    corrente_lab_fab.SetActive(true);

                    btn_usina.SetActive(true);
                    btn_lab.SetActive(true);
                    btn_fab.SetActive(true);
                    break;
                case 5:
                    map.sprite = mapSprites[0];
                    corrente_usina_lab.SetActive(true);
                    corrente_lab_fab.SetActive(true);
                    corrente_fab_fazenda.SetActive(true);

                    btn_usina.SetActive(true);
                    btn_lab.SetActive(true);
                    btn_fab.SetActive(true);
                    btn_fazenda.SetActive(true);
                    break;
                case 6:
                    map.sprite = mapSprites[0];
                    corrente_usina_lab.SetActive(true);
                    corrente_lab_fab.SetActive(true);
                    corrente_fab_fazenda.SetActive(true);
                    corrente_fazenda_floresta.SetActive(true);

                    btn_usina.SetActive(true);
                    btn_lab.SetActive(true);
                    btn_fab.SetActive(true);
                    btn_fazenda.SetActive(true);
                    btn_floresta.SetActive(true);
                    break;
            }
        }
    }

}
