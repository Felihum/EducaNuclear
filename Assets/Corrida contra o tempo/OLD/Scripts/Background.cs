using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class Background : MonoBehaviour
    {
        [SerializeField] [Range(0.1f,0.99999f)] private float speed;

        private float startPosition, length;

        private float Distance => ((cam.transform.position.x ) ) * speed;
        private float Temp => ((cam.transform.position.x ) + length) * (1 - speed);

        private GameObject cam;

        private void Awake()
        {
            startPosition = transform.position.x;
            length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        }

        void Start()
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }

        void Update()
        {
            transform.position = new Vector3(startPosition + Distance, transform.position.y, transform.position.z);

            if (Temp > startPosition + length )
            {
                startPosition += (length);
                Debug.Log($"startPos: {startPosition}");
            }
            if (Temp < startPosition - (length))
                startPosition -= length;
        }
    }
}
