using System.Collections.Generic;
using UnityEngine;
namespace Trunfo{
    public class CardPositionManager : MonoBehaviour
    {
        public Transform myHands; // Referência ao painel das cartas do jogador
        public Transform enemyHands; // Referência ao painel das cartas do inimigo
        public float cardSpacing = 1.5f; // Espaçamento entre as cartas

        // Atualiza as posições das cartas nos painéis do jogador e do inimigo
        public void UpdateCardPositions()
        {
            PositionCards(myHands);
            PositionCards(enemyHands);
        }

        // Ajusta a posição das cartas em um painel específico
        private void PositionCards(Transform handPanel)
        {
            for (int i = 0; i < handPanel.childCount; i++)
            {
                Transform card = handPanel.GetChild(i);
                float newZPosition = -8 + (i * cardSpacing); // Ajuste para espaçamento entre as cartas
                card.localPosition = new Vector3(0, 0, newZPosition); // Atualiza a posição local
            }
        }
    }
}