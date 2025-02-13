using EducaNuclear;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Alquimia
{
    public class GameManagerAlquimia : MonoBehaviour
    {
        public Camera mainCamera;

        public GameObject pointPrefab;
        public int gridWidth = 5;
        public int gridHeight = 5;
        private Point[,] grid;
        public PunctuationManager[] punctuationsInScene;
        public int punctuationFinished;
        private InputHandler inputHandler;
        private TotalJogadas totalJogadasText;
        public float pointGap;
        public float pointScale;
        public float gridMarginTop;

        [SerializeField] private GameObject modalResults;
        [SerializeField] private GameObject containerModalResults;
        [SerializeField] private GameObject containerModalFinalResults;
        [SerializeField] private Point[] pointsToSpawn;
        [SerializeField] private Color[] colors;
        [SerializeField] private float duration;
        [SerializeField] private Vector3 offset;
        [SerializeField] private GameObject pauseModal;
        [SerializeField] private GameObject endGame;

        void Start()
        {
            punctuationsInScene = FindObjectsOfType<PunctuationManager>();
            inputHandler = FindObjectOfType<InputHandler>();
            totalJogadasText = FindObjectOfType<TotalJogadas>();

            grid = new Point[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    CreatePoint(x, y);
                }
            }

            Point midPoint = grid[(gridWidth - 1) / 2, (gridHeight - 1) / 2];

            mainCamera.transform.SetPositionAndRotation(new Vector3(midPoint.transform.position.x, midPoint.transform.position.y - 3, -10), Quaternion.identity);

            if (SceneManager.GetActiveScene().name == "Alquimia1")
            {
                inputHandler.enabled = false;
                ScoreManager.UpdateCurrentAlquimiaTotalScore(-1);
            }
        }

        private void Update()
        {
            if (punctuationFinished == punctuationsInScene.Length && !totalJogadasText.isFinished)
            {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
                {
                    modalResults.SetActive(true);
                    inputHandler.GetComponent<InputHandler>().enabled = false;
                }
            }
            else if (totalJogadasText.isFinished)
            {
                endGame.SetActive(true);
                inputHandler.gameObject.SetActive(false);
            }
        }

        public void CreatePoint(int x, int y)
        {
            int rand = Random.Range(0, pointsToSpawn.Length);
            GameObject pointObject = Instantiate(pointsToSpawn[rand].gameObject, new Vector2(x, y + gridMarginTop) * pointGap, Quaternion.identity);
            pointObject.transform.localScale = new Vector3(pointScale, pointScale, pointScale);
            LeanTween.move(pointObject, new Vector2(x, y) * pointGap, duration);
            Point point = pointObject.GetComponent<Point>();
            point.gridPosition = new Vector2(x, y);
            point.color = colors[rand];
            grid[x, y] = point;
            pointObject.name = "(" + x + "," + y + ")";
        }

        public void RemovePoint(Point point)
        {
            int x = (int)point.gridPosition.x;
            int y = (int)point.gridPosition.y;
            grid[x, y] = null;

            // Move points above down
            for (int i = y + 1; i < gridHeight; i++)
            {
                if (grid[x, i] != null)
                {

                    grid[x, i - 1] = grid[x, i];
                    grid[x, i - 1].gridPosition = new Vector2(x, i - 1);
                    LeanTween.move(grid[x, i - 1].gameObject, new Vector2(x, i - 1) * pointGap, duration);

                    grid[x, i] = null;
                }
            }

            // Create new point at the top
            CreatePoint(x, gridHeight - 1);
        }

        public void UpdatePointsPosition(List<GameObject> selectedPoints)
        {
            foreach (GameObject selectedPoint in selectedPoints)
            {
                Point point = selectedPoint.GetComponent<Point>();

                RemovePoint(point);
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void NextScene()
        {
            if (SceneManager.GetActiveScene().name != "Alquimia5")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                ScoreManager.UpdateCurrentAlquimiaTotalScore(totalJogadasText.score);

            }
            else
            {
                ScoreManager.UpdateCurrentAlquimiaTotalScore(totalJogadasText.score);
                containerModalResults.SetActive(false);
                containerModalFinalResults.SetActive(true);
                containerModalFinalResults.GetComponentInChildren<TMP_Text>().text = "Pontuação Total: " + ScoreManager.GetCurrentAlquimiaTotalScore().ToString();
                ScoreManager.UpdateBestAlquimiaScore(ScoreManager.GetCurrentAlquimiaTotalScore());

                SaveAndLoad.LoadPlayerData();


                PlayerData playerData;

                if (SaveAndLoad.playerData.currentPhase <= 3)
                {
                    playerData = new PlayerData(currentPhase: 4);
                }
                else
                {
                    playerData = new PlayerData(currentPhase: SaveAndLoad.playerData.currentPhase);
                }

                SaveAndLoad.SavePlayerData(playerData);

            }
            
        }

        public void CloseTutorial()
        {
            inputHandler.enabled = true;
        }

        public void OpenConfiguration()
        {
            pauseModal.SetActive(!pauseModal.active);
            inputHandler.enabled = !inputHandler.enabled;
        }
    }
}
