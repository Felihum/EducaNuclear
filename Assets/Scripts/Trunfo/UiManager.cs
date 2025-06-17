using UnityEngine;
using UnityEngine.UI;

namespace Trunfo {
    public class UIManager : MonoBehaviour {
        public GameObject characteristicButtonsPanel; // Painel com botões de características
        public Text resultText; // Texto para mostrar o turno atual
        public GameObject gameOverPanel; // Painel para exibir a tela de fim de jogo

    public void ShowEnemyTurn() {
    resultText.text = "Turno do Inimigo";
    }


        void Start() {
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(false); // Certifique-se de que o painel de fim de jogo está oculto no início
            }
        }

        public void ShowPlayerTurn() {
            Debug.Log("UIManager: Exibindo turno do jogador");
            characteristicButtonsPanel.SetActive(true); // Torna o painel visível
            resultText.text = "Seu Turno";
            HideGameOverPanel(); // Garante que o painel de fim de jogo esteja escondido
        }

        public void ShowInimigoTurn() {
            Debug.Log("UIManager: Exibindo turno do inimigo");
            HideCharacteristicButtons(); // Esconde os botões
            resultText.text = "Turno do Oponente";
            HideGameOverPanel(); // Garante que o painel de fim de jogo esteja escondido
        }

        public void ShowResult(string result) {
            Debug.Log("UIManager: Exibindo resultado final: " + result);
            resultText.text = result;
            HideGameOverPanel(); // Garante que o painel de fim de jogo esteja escondido
        }

        public void ShowGameOver(string message) {
            Debug.Log("UIManager: Exibindo fim de jogo: " + message);
            resultText.text = message;
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(true); // Mostra o painel de fim de jogo
            }
            HideCharacteristicButtons(); // Esconde os botões de características
        }

        public void ShowFinalResult(string finalMessage) {
            Debug.Log("UIManager: Exibindo resultado final: " + finalMessage);
            resultText.text = finalMessage;
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(true); // Mostra o painel de fim de jogo
            }
        }

        // Método para atualizar as cartas atuais na UI
        public void UpdateCurrentCards(Carta cartaJogador, Carta cartaInimigo) {
            Debug.Log($"Atualizando cartas: Jogador - {cartaJogador.Name}, Inimigo - {cartaInimigo.Name}");
        }

        void HideGameOverPanel() {
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(false); // Garante que o painel de fim de jogo esteja escondido
            }
        }

        void HideCharacteristicButtons() {
            characteristicButtonsPanel.SetActive(false); // Torna o painel invisível
        }
    }
}