using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    public class Parallax : MonoBehaviour
    {
        private Transform _cam; // Main Camera
        private Vector3 _camStartPos;
        private float CamDistance => _cam.position.x - _camStartPos.x; // Distance between the camera start and its current position
        private Vector3 FollowCamPosition => new(CamDistance, transform.position.y, 0);

        private GameObject[] _backgrounds;
        private Material[] _materials;
        private float[] _backgroundsSpeed;

        [Range(0.01f, 0.1f)]
        [SerializeField] private float _parallaxSpeed;

        private void Start()
        {
            _cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _camStartPos = _cam.transform.position;

            int count = transform.childCount;
            _materials = new Material[count];
            _backgroundsSpeed = new float[count];
            _backgrounds = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                _backgrounds[i] = transform.GetChild(i).gameObject;
                _materials[i] = _backgrounds[i].GetComponent<Renderer>().material;
            }
            CalculateSpeed(count);
        }

        private void CalculateSpeed(int count)
        {
            float farthestBackground = -1000f;

            for (int i = 0; i < count; i++)
            {
                if ((_backgrounds[i].transform.position.z - _cam.position.z) > farthestBackground)
                {
                    farthestBackground = _backgrounds[i].transform.position.z - _cam.position.z;
                }
            }

            for (int i = 0; i < count; i++)
            {
                _backgroundsSpeed[i] = 1 - (_backgrounds[i].transform.position.z - _cam.position.z) / farthestBackground;
            }
        }

        private void LateUpdate()
        {
            transform.position = FollowCamPosition;

            for (int i = 0; i < _backgrounds.Length; i++)
            {
                float speed = _backgroundsSpeed[i] * _parallaxSpeed;
                _materials[i].SetTextureOffset("_MainTex", speed * new Vector2(CamDistance, 0));
            }
        }

    }
}
