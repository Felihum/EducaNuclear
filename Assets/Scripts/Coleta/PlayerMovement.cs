using EducaNuclear;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coleta{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;

        public Rigidbody2D rb;

        public Animator animator;
        public RuntimeAnimatorController playerFeminino;
        public RuntimeAnimatorController playerMasculino;

        public AudioSource footSteps;

        public Joystick joystick;

        Vector2 movement;

        public bool haveKey = false;

        private void Start()
        {
            if (PlayerPrefs.GetString("Gender").Equals("female"))
            {
                animator.runtimeAnimatorController = playerFeminino;
            }
            else if (PlayerPrefs.GetString("Gender").Equals("male"))
            {
                animator.runtimeAnimatorController = playerMasculino;
            }
        }

        void Update()
        {
            movement.x = joystick.Horizontal;
            movement.y = joystick.Vertical;

            //***** Script para testar movimento com teclado *****
            //movement.x = Input.GetAxisRaw("Horizontal")
            //movement.y = Input.GetAxisRaw("Vertical")

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x > 0 || movement.x < 0 || movement.y > 0 || movement.y < 0)
            {
                footSteps.enabled = true;

            }
            else
            {
                footSteps.enabled = false;

            }
        }

        void FixedUpdate()
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

        /*private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Door") && haveKey == true)
            {
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Key") && haveKey == false)
            {
                Destroy(collision.gameObject);
                haveKey = true;
            }

        }*/
    }
}