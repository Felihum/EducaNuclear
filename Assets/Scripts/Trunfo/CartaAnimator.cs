using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo{
   public class CardAnimator : MonoBehaviour
    {
        public float moveDuration = 0.5f; // Duração do movimento
        public float scaleDuration = 0.2f; // Duração da animação de escala
        public float offset = 0.5f; // Offset para o posicionamento das cartas
        private List<GameObject> cartasNaPilha = new List<GameObject>(); // Lista de cartas na pilha

        // Método para mover uma carta para o topo
        public void MoveCard(Transform carta, Transform destino)
        {
            StartCoroutine(MoveCardCoroutine(carta, destino));
        }

        // Corrotina para mover a carta
        private IEnumerator MoveCardCoroutine(Transform carta, Transform destino)
        {
            Vector3 startPos = carta.position;
            Vector3 endPos = destino.position;
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                carta.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            carta.position = endPos; // Garantir que a carta chegue ao destino
            ReorganizarPilha(); // Reorganiza a pilha após o movimento
        }

        // Método para escalar a carta
        public void ScaleCard(Transform carta, float scaleFactor)
        {
            StartCoroutine(ScaleCardCoroutine(carta, scaleFactor));
        }

        // Corrotina para escalar a carta
        private IEnumerator ScaleCardCoroutine(Transform carta, float scaleFactor)
        {
            Vector3 originalScale = carta.localScale;
            Vector3 targetScale = originalScale * scaleFactor;
            float elapsed = 0f;

            while (elapsed < scaleDuration)
            {
                carta.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / scaleDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            carta.localScale = targetScale; // Garantir que a carta chegue ao tamanho final
            yield return new WaitForSeconds(0.2f); // Espera um tempo antes de voltar ao tamanho original

            // Volta ao tamanho original
            elapsed = 0f;
            while (elapsed < scaleDuration)
            {
                carta.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / scaleDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            carta.localScale = originalScale; // Garantir que a carta volte ao tamanho original
        }

        // Método para reorganizar a pilha de cartas
        public void ReorganizarPilha()
        {
            for (int i = 0; i < cartasNaPilha.Count; i++)
            {
                // Atualiza a posição de cada carta na pilha
                cartasNaPilha[i].transform.localPosition = new Vector3(0, -i * offset, 0);
            }
        }

        // Método para adicionar uma carta à pilha
        public void AddCardToPilha(GameObject carta)
        {
            cartasNaPilha.Add(carta);
            ReorganizarPilha(); // Reorganiza após adicionar a carta
        }

        // Método para remover uma carta da pilha
        public void RemoveCardFromPilha(GameObject carta)
        {
            cartasNaPilha.Remove(carta);
            ReorganizarPilha(); // Reorganiza após remover a carta
        }

        // Novo método para animar o movimento da carta
        public void AnimateCardMovement(Carta carta, List<Carta> vencedoraHand)
        {
            // Defina a posição onde a carta deve ser movida
            Transform destino = vencedoraHand[vencedoraHand.Count - 1].transform; // Última carta da mão do vencedor
            MoveCard(carta.transform, destino);
        }
    }
 
}
