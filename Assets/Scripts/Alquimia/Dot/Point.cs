using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alquimia
{
    public class Point : MonoBehaviour
    {
        public Vector2 gridPosition;
        public Color color;
        public bool isSelected;
        [SerializeField] private ParticleSystem ripple;
        public Animator animator;
        public SpriteRenderer rippleObject;

        /*private void Start()
        {
            animator = gameObject.GetComponentInChildren<Animator>();
            Debug.Log($"Animator object: {animator}");
            rippleObject = gameObject.GetComponentInChildren<SpriteRenderer>();
            Debug.Log($"Color: {color}");
            rippleObject.color = color;
            Debug.Log($"Circle Color: {rippleObject.color}");
        }*/

        public void Vibrate(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) =>
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(originalPos.x + offsetX, originalPos.y, originalPos.z);
            }).setOnComplete(() =>
            {
                transform.localPosition = originalPos;
            });
        }

        public void PlayRipple()
        {
            ripple.Play();
        }

        public void StopRipple()
        {
            ripple.Stop();
        }
    }
}
