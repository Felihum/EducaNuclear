using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Coleta;
using Corrida;
using UnityEngine.SocialPlatforms.Impl;
using EducaNuclear;
using TMPro;

namespace Trunfo {
    public class GameManager : MonoBehaviour {
        public Transform myHandsPanel; // Painel do jogador
        public Transform enemyHandsPanel; // Painel do inimigo
        public UIManager uiManager; // Gerencia a UI
        public Button compararMassaAtomicaButton; 
        public Button compararPontoDeFusaoButton;
        public Button compararAbundanciaCorpoButton;
        public Button compararEletronegatividadeButton;
        public CardPositionManager cardPositionManager;
        public Text cardCountDisplay; // Referência ao texto de contagem de cartas
        private List<Carta> playerHand; // Cartas do jogador
        private List<Carta> enemyHand; // Cartas do inimigo
        public bool jogoAtivo = true; // Estado do jogo
        public TimerTurno timerTurnoRef;
        public GameObject loseGamePanel;
        public GameObject winGamePanel;
        public GameObject pauseModal;
        public TMP_Text winGameText;
        public TimerController timerController;
        private int score;
        private bool verified = false;

        private string[] caracteristicas = { "MassaAtomica", "PontoDeFusao", "AbundanciaCorpo", "Eletronegatividade" }; // Características disponíveis

        void Start() {
            InitializeHands();
            AtualizarContagemDeCartas(); // Exibe a contagem inicial
            StartCoroutine(StartTurnWithDelay(1.0f)); // Aguarda 2 segundos antes de começar o turno
        }

        private void Update()
        {
            VerificarFimDeJogo();
        }

        void InitializeHands() {
            playerHand = GetCardsFromPanel(myHandsPanel);
            enemyHand = GetCardsFromPanel(enemyHandsPanel);
        }

        List<Carta> GetCardsFromPanel(Transform panel) {
            List<Carta> cartas = new List<Carta>();
            foreach (Transform child in panel) {
                Carta carta = child.GetComponent<Carta>();
                if (carta != null) {
                    cartas.Add(carta);
                }
            }
            return cartas;
        }

public IEnumerator StartTurnWithDelay(float delay) {
    yield return new WaitForSeconds(delay); // Aguarda o tempo especificado
    StartTurn(); // Chama o StartTurn após o atraso
}

        void StartTurn() {
            if (!jogoAtivo) return;

            timerTurnoRef.ResetTimer();

            AtualizarContagemDeCartas(); // Atualiza a contagem de cartas no início do turno

            EnableButtons(true); // Habilita os botões no turno do jogador
            ResetButtonColors(); // Remove a cor cinza dos botões

            Carta cartaAtualJogador = playerHand[playerHand.Count - 1]; // Última carta do jogador
            Carta cartaAtualInimigo = enemyHand[enemyHand.Count - 1]; // Última carta do inimigo

            Transform[] cardInimigoFacesWithTag = cartaAtualInimigo.GetComponentsInChildren<Transform>(true);
            Transform[] cardJogadorFacesWithTag = cartaAtualJogador.GetComponentsInChildren<Transform>(true);

            if (cardInimigoFacesWithTag.Length > 0)
            {
                foreach (Transform cardFace in cardInimigoFacesWithTag)
                {
                    if(cardFace.CompareTag("cardFront")){
                        cardFace.gameObject.SetActive(false);    
                    }
                }
            }

            if (cardJogadorFacesWithTag.Length > 0)
            {
                foreach (Transform cardFace in cardJogadorFacesWithTag)
                {
                    if(cardFace.CompareTag("cardFront")){
                        cardFace.gameObject.SetActive(true);    
                    }
                }
            }

            // Atualiza a UI e loga as cartas atuais e o número de cartas de cada jogador
            uiManager.UpdateCurrentCards(cartaAtualJogador, cartaAtualInimigo);
            Debug.Log($"Início do turno: Jogador tem {playerHand.Count} cartas. Inimigo tem {enemyHand.Count} cartas.");
            
            // Ativa o trigger 'comparacao' para as animações de destaque

            cartaAtualJogador.GetComponent<Animator>().SetTrigger("comparacao");
            cartaAtualInimigo.GetComponent<Animator>().SetTrigger("comparacao");
        

            uiManager.ShowPlayerTurn();
        }

        public void AtualizarContagemDeCartas() {
            if (cardCountDisplay != null) {
                cardCountDisplay.text = $"Jogador: {playerHand.Count} | Inimigo: {enemyHand.Count}";
            }
        }

        public void OnCompararMassaAtomica() => CompararCartas("MassaAtomica");
        public void OnCompararPontoDeFusao() => CompararCartas("PontoDeFusao");
        public void OnCompararAbundanciaCorpo() => CompararCartas("AbundanciaCorpo");
        public void OnCompararEletronegatividade() => CompararCartas("Eletronegatividade");

        void CompararCartas(string caracteristica) {
            Carta cartaAtualJogador = playerHand[playerHand.Count - 1];
            Carta cartaAtualInimigo = enemyHand[enemyHand.Count - 1];

            float playerValue = GetCardValue(cartaAtualJogador, caracteristica);
            float enemyValue = GetCardValue(cartaAtualInimigo, caracteristica);

            Debug.Log($"Comparando característica: {caracteristica}");
            Debug.Log($"Valor do Jogador: {playerValue}, Valor do Inimigo: {enemyValue}");

            Transform[] cardInimigoFacesWithTag = cartaAtualInimigo.GetComponentsInChildren<Transform>(true);

            if (cardInimigoFacesWithTag.Length > 0)
            {
                foreach (Transform cardFace in cardInimigoFacesWithTag)
                {
                    if(cardFace.CompareTag("cardFront")){
                        cardFace.gameObject.SetActive(true);    
                    }
                }
            }

            if (playerValue > enemyValue) {
                uiManager.ShowResult("Jogador venceu o turno!");
                Debug.Log("Resultado: Jogador venceu o turno.");
                StartCoroutine(MoverCartasComDelay(playerHand, cartaAtualJogador, enemyHand, cartaAtualInimigo, 5.5f));
            } else if (playerValue < enemyValue) {
                uiManager.ShowResult("Inimigo venceu o turno!");
                Debug.Log("Resultado: Inimigo venceu o turno.");
                StartCoroutine(MoverCartasComDelay(enemyHand, cartaAtualInimigo, playerHand, cartaAtualJogador, 5.5f));
            }
            else {
                uiManager.ShowResult("Empate!");
                Debug.Log("Resultado: Empate.");
            }
            
            cartaAtualJogador.GetComponent<Animator>().SetTrigger("RetornarIdle");
            cartaAtualInimigo.GetComponent<Animator>().SetTrigger("RetornarIdle");

            AtualizarContagemDeCartas(); // Atualiza a contagem de cartas após a comparação

            
            if (jogoAtivo) {
                EnableButtons(false); // Desabilita os botões no turno do inimigo
                SetButtonsGray(); // Torna os botões cinza
                StartCoroutine(StartEnemyTurnWithDelay(6.5f));
            }

            timerTurnoRef.timerOn = false;
        }

        public void TimerOver(){
            Carta cartaAtualJogador = playerHand[playerHand.Count - 1];
            Carta cartaAtualInimigo = enemyHand[enemyHand.Count - 1];

            uiManager.ShowResult("Inimigo venceu o turno!");
            Debug.Log("Resultado: Inimigo venceu o turno.");

            Transform[] cardInimigoFacesWithTag = cartaAtualInimigo.GetComponentsInChildren<Transform>(true);

            if (cardInimigoFacesWithTag.Length > 0)
            {
                foreach (Transform cardFace in cardInimigoFacesWithTag)
                {
                    if (cardFace.CompareTag("cardFront"))
                    {
                        cardFace.gameObject.SetActive(true);
                    }
                }
            }

            StartCoroutine(MoverCartasComDelay(enemyHand, cartaAtualInimigo, playerHand, cartaAtualJogador, 5.5f));

            cartaAtualJogador.GetComponent<Animator>().SetTrigger("RetornarIdle");
            cartaAtualInimigo.GetComponent<Animator>().SetTrigger("RetornarIdle");

            AtualizarContagemDeCartas(); // Atualiza a contagem de cartas após a comparação

            if (jogoAtivo) {
                EnableButtons(false); // Desabilita os botões no turno do inimigo
                SetButtonsGray(); // Torna os botões cinza
                StartCoroutine(StartEnemyTurnWithDelay(6.5f));
            }

            timerTurnoRef.timerOn = false;
        }

        public IEnumerator StartEnemyTurnWithDelay(float delay) {
            yield return new WaitForSeconds(delay); // Aguarda o tempo especificado
            StartEnemyTurn(); // Inicia o turno do inimigo
        }

     public void StartEnemyTurn() {
        if (!jogoAtivo) return;

        // Exibe "Turno do Inimigo" antes da comparação para evitar a sobreposição do resultado anterior
        uiManager.ShowEnemyTurn();
        AtualizarContagemDeCartas();

        Debug.Log($"Início do turno do inimigo: Jogador tem {playerHand.Count} cartas. Inimigo tem {enemyHand.Count} cartas.");

        string caracteristicaAleatoria = caracteristicas[Random.Range(0, caracteristicas.Length)];
        Carta cartaAtualJogador = playerHand[playerHand.Count - 1];
        Carta cartaAtualInimigo = enemyHand[enemyHand.Count - 1];

        Transform[] cardInimigoFacesWithTag = cartaAtualInimigo.GetComponentsInChildren<Transform>(true);
        Transform[] cardJogadorFacesWithTag = cartaAtualJogador.GetComponentsInChildren<Transform>(true);

        if (cardInimigoFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardInimigoFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(false);    
                }
            }
        }

        if (cardJogadorFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardJogadorFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(true);    
                }
            }
        }

        Debug.Log($"Comparando cartas: Jogador - {cartaAtualJogador.Name}, Inimigo - {cartaAtualInimigo.Name}");

        cartaAtualJogador.GetComponent<Animator>().SetTrigger("comparacao");
        cartaAtualInimigo.GetComponent<Animator>().SetTrigger("comparacao");

        // Inicia a comparação após um pequeno delay para que a mudança de turno seja percebida
        StartCoroutine(CompararCartasInimigo(caracteristicaAleatoria, 3.0f));
    }

    IEnumerator CompararCartasInimigo(string caracteristica, float delay) {
        yield return new WaitForSeconds(delay);

        Carta cartaAtualJogador = playerHand[playerHand.Count - 1];
        Carta cartaAtualInimigo = enemyHand[enemyHand.Count - 1];

        float playerValue = GetCardValue(cartaAtualJogador, caracteristica);
        float enemyValue = GetCardValue(cartaAtualInimigo, caracteristica);

        Debug.Log($"Turno do inimigo: Comparando característica: {caracteristica}");
        Debug.Log($"Valor do Jogador: {playerValue}, Valor do Inimigo: {enemyValue}");

        Transform[] cardInimigoFacesWithTag = cartaAtualInimigo.GetComponentsInChildren<Transform>(true);

        if (cardInimigoFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardInimigoFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(true);    
                }
            }
        }

        // Exibe o resultado apenas após a comparação
        if (enemyValue > playerValue) {
            uiManager.ShowResult("Inimigo venceu o turno!");
            Debug.Log("Resultado: Inimigo venceu o turno.");
            StartCoroutine(MoverCartasComDelay(enemyHand, cartaAtualInimigo, playerHand, cartaAtualJogador, 5.5f));
        } else if (enemyValue < playerValue) {
            uiManager.ShowResult("Jogador venceu o turno!");
            Debug.Log("Resultado: Jogador venceu o turno.");
            StartCoroutine(MoverCartasComDelay(playerHand, cartaAtualJogador, enemyHand, cartaAtualInimigo, 5.5f));
        }

        else {
            uiManager.ShowResult("Empate!");
            Debug.Log("Resultado: Empate.");
        }

        // Reinicia as animações para o próximo turno
        cartaAtualJogador.GetComponent<Animator>().SetTrigger("RetornarIdle");
        cartaAtualInimigo.GetComponent<Animator>().SetTrigger("RetornarIdle");

        AtualizarContagemDeCartas(); // Atualiza a contagem de cartas após a comparação

        //VerificarFimDeJogo();
        if (jogoAtivo) StartCoroutine(StartTurnWithDelay(6.5f));
    }

        // Funções para gerenciar botões no turno do oponente
        public void EnableButtons(bool enable) {
            compararMassaAtomicaButton.interactable = enable;
            compararPontoDeFusaoButton.interactable = enable;
            compararAbundanciaCorpoButton.interactable = enable;
            compararEletronegatividadeButton.interactable = enable;
        }

        public void SetButtonsGray() {
            ColorBlock colorBlock = compararMassaAtomicaButton.colors;
            colorBlock.normalColor = Color.gray;
            compararMassaAtomicaButton.colors = colorBlock;
            compararPontoDeFusaoButton.colors = colorBlock;
            compararAbundanciaCorpoButton.colors = colorBlock;
            compararEletronegatividadeButton.colors = colorBlock;
        }

        void ResetButtonColors() {
            ColorBlock colorBlock = compararMassaAtomicaButton.colors;
            colorBlock.normalColor = Color.white;
            compararMassaAtomicaButton.colors = colorBlock;
            compararPontoDeFusaoButton.colors = colorBlock;
            compararAbundanciaCorpoButton.colors = colorBlock;
            compararEletronegatividadeButton.colors = colorBlock;
        }
       IEnumerator MoverCartasComDelay(List<Carta> vencedorHand, Carta vencedorCarta, List<Carta> perdedorHand, Carta perdedorCarta, float delay) {
    // Aguarda antes de mover as cartas
    yield return new WaitForSeconds(delay);

    Transform[] cardInimigoFacesWithTag;
    Transform[] cardJogadorFacesWithTag;

    if(vencedorCarta.CompareTag("cardJogador")){
        cardJogadorFacesWithTag = vencedorCarta.GetComponentsInChildren<Transform>(true);
        cardInimigoFacesWithTag = perdedorCarta.GetComponentsInChildren<Transform>(true);

        if (cardInimigoFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardInimigoFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(true);    
                }
            }
        }

        if (cardJogadorFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardJogadorFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(true);    
                }
            }
        }
    } else{
        cardInimigoFacesWithTag = vencedorCarta.GetComponentsInChildren<Transform>(true);
        cardJogadorFacesWithTag = perdedorCarta.GetComponentsInChildren<Transform>(true);

        if (cardInimigoFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardInimigoFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(false);    
                }
            }
        }

        if (cardJogadorFacesWithTag.Length > 0)
        {
            foreach (Transform cardFace in cardJogadorFacesWithTag)
            {
                if(cardFace.CompareTag("cardFront")){
                    cardFace.gameObject.SetActive(false);    
                }
            }
        }
    }

    // Remover as cartas das mãos
    vencedorHand.Remove(vencedorCarta);
    perdedorHand.Remove(perdedorCarta);

    // Posiciona a carta perdedora na mesma posição da carta vencedora
    perdedorCarta.transform.position = vencedorCarta.transform.position;

    // Adicionar ambas as cartas ao final da mão do vencedor
    vencedorHand.Insert(0, vencedorCarta); // Carta do vencedor vai para o início da pilha
    vencedorHand.Insert(0, perdedorCarta); // Carta do perdedor vai para o início

    // Atualizar a UI para refletir as mudanças
    UpdateCardUI(myHandsPanel, playerHand);
    UpdateCardUI(enemyHandsPanel, enemyHand);

    // Atualiza as posições das cartas visualmente
    cardPositionManager.UpdateCardPositions();

    //VerificarFimDeJogo();
}

        void UpdateCardUI(Transform handPanel, List<Carta> hand) {
            foreach (Transform cardTransform in handPanel) {
                cardTransform.SetParent(null); // Remove temporariamente as cartas do painel
            }

            foreach (Carta carta in hand) {
                carta.transform.SetParent(handPanel); // Adiciona as cartas na nova ordem
                carta.transform.SetAsLastSibling(); // Coloca no final da hierarquia
            }
        }

        public void PauseTrunfo()
        {
            pauseModal.SetActive(true);
            Time.timeScale = 0;
        }

        public void ResumeTrunfo()
        {
            Time.timeScale = 1;
            pauseModal.SetActive(false);
        }


        public void VerificarFimDeJogo() {
            if (!verified)
            {
                if (playerHand.Count == 0 || enemyHand.Count == 0 || !timerController.timerOn)
                {
                    jogoAtivo = false;
                    timerController.timerOn = false;

                    if (playerHand.Count < enemyHand.Count)
                    {
                        loseGamePanel.SetActive(true);
                        winGamePanel.SetActive(false);
                    }
                    else
                    {
                        winGamePanel.SetActive(true);
                        loseGamePanel.SetActive(false);

                        float endGameTime = timerController.timeLeft;

                        if (endGameTime <= 150 && endGameTime >= 90)
                        {
                            score = 9000;
                        }
                        else if (endGameTime < 90 && endGameTime >= 60)
                        {
                            score = 6000;
                        }
                        else if (endGameTime < 60 && endGameTime >= 30)
                        {
                            score = 3000;

                        }
                        else if (endGameTime < 30)
                        {
                            score = 1000;
                        }

                        ScoreManager.UpdateBestTrunfoScore(score);

                        winGameText.text = "Parabéns\nPontos ganhos: " + score.ToString();

                        winGamePanel.SetActive(true);

                        PlayerData playerData;

                        if (SaveAndLoad.playerData.currentPhase <= 1)
                        {
                            playerData = new PlayerData(2);
                        }
                        else
                        {
                            playerData = new PlayerData(SaveAndLoad.playerData.currentPhase);
                        }

                        SaveAndLoad.SavePlayerData(playerData);

                        verified = true;
                    }
                }
            }
        }

        float GetCardValue(Carta carta, string caracteristica) {
            switch (caracteristica) {
                case "MassaAtomica":
                    return carta.MassaAtomica;
                case "PontoDeFusao":
                    return carta.PontoDeFusao;
                case "AbundanciaCorpo":
                    return carta.AbundanciaCorpo;
                case "Eletronegatividade":
                    return carta.Eletronegatividade;
                default:
                    return 0;
            }
        }
    }
}