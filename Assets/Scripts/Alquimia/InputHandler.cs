using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Alquimia
{
    public class InputHandler : MonoBehaviour
    {
        private Point startPoint;
        private Point lastPoint;
        private LineRenderer currentLineRenderer;
        public Material lineMaterial;
        [SerializeField] private List<GameObject> selectedPoints;
        [SerializeField] private List<GameObject> linesToDestroy;
        [SerializeField] private GameManagerAlquimia gameManagerRef;
        private int punctuationCount;
        [SerializeField] private TMP_Text punctuationText;
        private PunctuationManager[] punctuationObjects;

        [SerializeField] private float duration = 0.1f;
        [SerializeField] private float magnitude = 0.1f;
        private TotalJogadas totalJogadasText;
        private AudioSource soundEffect;

        private void Start()
        {
            punctuationObjects = FindObjectsOfType<PunctuationManager>();
            soundEffect = FindObjectOfType<AudioSource>();
            totalJogadasText = FindObjectOfType<TotalJogadas>();
        }

        void Update()
        {
            HandleDragInput();
        }

        void HandleDragInput()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && currentLineRenderer == null))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    Point point = hit.collider.GetComponent<Point>();

                    if (point != null)
                    {
                        point.isSelected = true;
                        startPoint = point;
                        lastPoint = point;
                        point.PlayRipple();
                        soundEffect.Play();
                        CreateLine(point.transform.position, mousePosition);
                        selectedPoints.Add(point.gameObject);
                        punctuationCount++;
                    }
                }
            }

            if (Input.GetMouseButton(0) && currentLineRenderer != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null)
                {
                    Point point = hit.collider.GetComponent<Point>();

                    if ((point != null) && (point != lastPoint) && point.isSelected == false && point.color.Equals(lastPoint.color) && VerificaPosicaoRel(point, lastPoint))
                    {
                        lastPoint = point;
                        point.PlayRipple();
                        soundEffect.Play();
                        UpdateLine(point.transform.position);
                        currentLineRenderer = null;
                        return;
                    }
                    else
                    {
                        UpdateLine(mousePosition);
                    }
                }
                else
                {
                    UpdateLine(mousePosition);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedPoints.Count > 1)
                {
                    foreach (GameObject pointToRemove in selectedPoints)
                    {
                        Destroy(pointToRemove.gameObject);

                        foreach (GameObject lineToRemove in linesToDestroy)
                        {
                            Destroy(lineToRemove.gameObject);
                        }
                    }

                    gameManagerRef.UpdatePointsPosition(selectedPoints);

                    PunctuationManager punctuationToUpdate = null;

                    foreach (PunctuationManager punctObj in punctuationObjects)
                    {
                        Point lastPoint = selectedPoints[selectedPoints.Count - 1].GetComponent<Point>();
                        if (punctObj.color == lastPoint.color)
                        {
                            punctuationToUpdate = punctObj;
                            break;
                        }
                    }

                    if (punctuationToUpdate != null)
                    {
                        punctuationToUpdate.UpdatePunctuation(punctuationCount);
                    }

                    selectedPoints.RemoveAll(x => x.GetComponent<Point>() != null);
                    linesToDestroy.RemoveAll(x => x.GetComponent<LineRenderer>() != null);
                    punctuationCount = 0;

                    totalJogadasText.UpdateTotalJogadas();

                    /*if (gameManagerRef.punctuationFinished == gameManagerRef.punctuationsInScene.Length && !totalJogadasText.isFinished)
                    {
                        ScoreManager.UpdateTotalScore(totalJogadasText.score);
                    }*/
                    
                }
                else if (selectedPoints.Count == 1)
                {
                    selectedPoints[0].GetComponent<Point>().isSelected = false;
                    selectedPoints[0].GetComponent<Point>().StopRipple();
                    Destroy(currentLineRenderer.gameObject);
                    selectedPoints.RemoveAll(x => x.GetComponent<Point>() != null);
                    linesToDestroy.RemoveAll(x => x.GetComponent<LineRenderer>() != null);
                    punctuationCount = 0;
                }
                else
                {
                    Destroy(currentLineRenderer.gameObject);
                }

                currentLineRenderer = null;
            }
        }

        void UpdateLine(Vector3 newEnd)
        {
            if (currentLineRenderer != null)
            {
                currentLineRenderer.SetPosition(1, newEnd);
            }
        }

        bool VerificaPosicaoRel(Point currentPoint, Point lastPoint)
        {
            bool isValid = false;

            if (currentPoint.gridPosition.x == lastPoint.gridPosition.x)
            {
                if (currentPoint.gridPosition.y == lastPoint.gridPosition.y + 1)
                {
                    isValid = true;
                }
                if (currentPoint.gridPosition.y == lastPoint.gridPosition.y - 1)
                {
                    isValid = true;
                }
            }
            if (currentPoint.gridPosition.y == lastPoint.gridPosition.y)
            {
                if (currentPoint.gridPosition.x == lastPoint.gridPosition.x + 1)
                {
                    isValid = true;
                }
                if (currentPoint.gridPosition.x == lastPoint.gridPosition.x - 1)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        void CreateLine(Vector3 start, Vector3 end)
        {
            GameObject lineObject = new GameObject("Line");
            currentLineRenderer = lineObject.AddComponent<LineRenderer>();
            currentLineRenderer.material = lineMaterial;
            currentLineRenderer.positionCount = 2;
            currentLineRenderer.SetPosition(0, start);
            currentLineRenderer.SetPosition(1, end);
            currentLineRenderer.startWidth = 0.1f;
            currentLineRenderer.endWidth = 0.1f;
            currentLineRenderer.useWorldSpace = true;
            linesToDestroy.Add(currentLineRenderer.gameObject);
        }
    }
}
