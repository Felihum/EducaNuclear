using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace Corrida
{
    public class PlayerCorrida : MonoBehaviour
    {
        #region Variaveis, Referências, Propriedades, etc
        [Header("Correr")]
        [SerializeField][Min(0.1f)] private float baseSpeed = 75f;
        [SerializeField][Min(0)] private float baseSpeedMultiplier = 1f;
        [SerializeField][Min(0)] private float startupTime = 0.5f;
        [SerializeField][Min(0)] private float stopTime = 1f;

        // Variáveis públicas de referência aos valores internos dos multiplicadores de velocidade
        // Existem para que os multiplicadores nunca sejam modificados para
        // abaixo de 1f ou acima de 10f por scripts externos
        public float MultiplierIncrease { get { return multiplierIncreaseInternal; } set { multiplierIncreaseInternal = Mathf.Clamp(value, 1f, 10f); } }
        public float MultiplierDecrease { get { return multiplierDecreaseInternal; } set { multiplierDecreaseInternal = Mathf.Clamp(value, 1f, 10f); } }

        //Variáveis internas dos multiplicadores
        private float multiplierIncreaseInternal = 1f;
        private float multiplierDecreaseInternal = 1f;

        // Mais Variáveis
        private float acceleration = 1f;
        private float startupTimer = 0f;
        private float stopTimer = 0f;
        private bool isStartingUp = false;
        private bool isStopping = false;
        public int questionsAnswered = 0;

        private float Multiplier => (baseSpeedMultiplier * multiplierIncreaseInternal) / multiplierDecreaseInternal;
        private float RunSpeed => baseSpeed * acceleration * Multiplier;

        [Header("Pular")]
        [SerializeField][Min(0f)] private float jumpHeight = 0.12f;
        [SerializeField][Min(0f)] private float groundDistance = 0.3f;
        [SerializeField][Min(0f)] private float jumpTime = 0.33f;
        [SerializeField][Range(0f, 0.5f)] private float coyoteTime = 0.05f;
        [SerializeField][Range(0f, 0.8f)] private float groundVelocityCheck = 0.05f;

        [SerializeField][Range(0f, 50f)] private float jumpForce;
        private bool CanJump => (isGrounded || isInCoyoteTime) && !PauseControler.IsPaused;
        private bool isGrounded = false;
        private bool isJumping = false;
        private float jumpTimer = 0f;
        private bool isInCoyoteTime = false;
        private float coyoteTimer = 0f;

        [Header("Vidas")]
        [SerializeField][Min(0)] private int startingLives = 3;
        public int lives;
        private bool lastLife = false;

        [Header("Danificado")]
        [SerializeField][Min(0f)] private float hurtJumpForce = 4.2f;
        [SerializeField][Min(0f)] private float hurtJumpTime = 0.5f;
        [SerializeField][Min(0f)] private float hitStopLength = 0.5f;
        [SerializeField][Range(0f, 0.8f)] private float hitStopScale = 0.1f;
        [SerializeField][Range(0f, 5f)] private float hurtInvincibilityLength = 1.5f;
        [SerializeField][Range(0f, 5f)] private float stunnedLength = 1f;

        public int hitCount { get; private set; } = 0;
        private bool isHurtInvincible = false;
        private float hurtInvincibilityTimer = 0f;
        private bool isHurtJumping = false;
        private float hurtJumpTimer = 0f;
        private float stunnedTimer = 0f;
        private Coroutine hitStopCoroutine;

        [Header("Visual da Invencibilidade")]
        [SerializeField][Range(0.01f, 0.2f)] private float invincibilityBlinkRate = 0.1f;
        private float invincibilityBlinkTimer = 0f;
        private Coroutine blinkCoroutine;
        private event Action InvincibilityEnd;
        private event Action BlinkInterrupt;

        [Header("Animação")]
        [SerializeField] private string idleParamName = "IsIdle";
        [SerializeField] private string runningParamName = "IsRunning";
        [SerializeField] private string jumpingParamName = "IsJumping";
        [SerializeField] private string fallingParamName = "IsFalling";
        [SerializeField] private string hurtParamName = "IsHurt";
        [SerializeField] private string stunnedParamName = "IsStunned";
        [SerializeField] private string incapacitatedParamName = "IsIncapacitated";
        [SerializeField] private string runSpeedParamName = "RunSpeed";
        private PlayerAnimationState animState;
        private bool[] animBools;
        private int[] animBoolsIDs;
        private float animSpeed;
        private int animSpeedID;

        // Diversas variaveis
        public PlayerState State { get; private set; }
        public bool IsInvincible => isHurtInvincible || isPickupInvincible;
        public Vector2 VelocityRef => rb.velocity;
        private bool isPickupInvincible = false;


        [Header("Referências")]
        [SerializeField] private PlayerDataCorrida playerData;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform feetPos;
        [SerializeField] private GameObject Dummy;
        private GameObject GFX;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        public GameObject telaDePegunta;
        public GameObject telaDeDerrota;
        public QuizManager quizManager;


        public static PlayerCorrida Instance { get; private set; }


        #endregion

        private void StartRunning()
        {
            State = PlayerState.Running;
            startupTimer = 0f;
            isStartingUp = true;
            SetAnimation(PlayerAnimationState.Running);
        }

        private void StopRunning()
        {
            State = PlayerState.Stopped;
            stopTimer = 0f;
            isStopping = true;
            InterruptJump();
            InvincibilityEnd?.Invoke();
        }

        // Alterar modificador de velocidade caso o jogar está começando ou parando de correr
        private void SetAcceleration(float speed)
        {
            acceleration = speed;
        }

        #region HurtAndLives
        public void HurtPlayer()
        {
            State = PlayerState.Hurt;
            isHurtInvincible = true;
            LoseLife();
            InterruptJump();
            hitCount++;
            SetAnimation(PlayerAnimationState.Hurt);

            if (blinkCoroutine != null)
                BlinkInterrupt?.Invoke();

            isHurtJumping = true;
            hurtJumpTimer = 0f;

            if(lives == 0)
            {
                LoseGame();
            }

            hitStopCoroutine ??= StartCoroutine(HitStop(hitStopLength));
        }

        private IEnumerator HitStop(float delay)
        {
            float scale = Time.timeScale;

            if (scale <= hitStopScale)
                yield break;

            Time.timeScale = hitStopScale;

            float timer = 0f;
            while (timer < delay)
            {
                yield return null;
                if (!PauseControler.IsPaused)
                    timer += Time.unscaledDeltaTime;
            }

            Time.timeScale = scale;

            if (hitStopCoroutine != null)
                hitStopCoroutine = null;
            yield break;
        }

        private void InterruptJump()
        {
            jumpTimer = 0f;
            isJumping = false;
        }

        public void GainLife(int life)
        {
            if (life <= 0)
                return;

            if (lives == 0)

                lastLife = false;

           lives += life;
        }

        private void LoseLife()
        {
            if (lives > 0)
            {
                lives--;
                if (UIManager.Instance != null)
                    UIManager.Instance.UpdateLivesUI(lives);
            }

            // Checa se você foi danificado após chegar a 0 vidas.
            // Se você vai de 1 vida a 0, você ainda pode jogar,
            // mas se levar dano novamente, você perde pra valer.
            if (!lastLife && lives == 0)
            {
                lastLife = true;
            }
            else
            {
                lastLife = false;
            }

        }

        public void LoseGame()
        {
            Time.timeScale = 0f;
            telaDeDerrota.SetActive(true);
        }


        #endregion
        #region AnimationAndVisuals
        private void SetAnimHashes()
        {
            int stateCount = Enum.GetNames(typeof(PlayerAnimationState)).Length;
            int[] IDs = new int[stateCount];

            IDs[(int)PlayerAnimationState.Idle] = Animator.StringToHash(idleParamName);
            IDs[(int)PlayerAnimationState.Running] = Animator.StringToHash(runningParamName);
            IDs[(int)PlayerAnimationState.Jumping] = Animator.StringToHash(jumpingParamName);
            IDs[(int)PlayerAnimationState.Falling] = Animator.StringToHash(fallingParamName);
            IDs[(int)PlayerAnimationState.Hurt] = Animator.StringToHash(hurtParamName);
            IDs[(int)PlayerAnimationState.Stunned] = Animator.StringToHash(stunnedParamName);
            IDs[(int)PlayerAnimationState.Incapacitated] = Animator.StringToHash(incapacitatedParamName);

            int speedID = Animator.StringToHash(runSpeedParamName);

            animBoolsIDs = IDs;
            animSpeedID = speedID;
        }

        // Ajusta os parâmetros do animator para a animação escolhida
        // e guarda referência ao estado da animação esperada e aos parâmetros ativos
        private void SetAnimation(PlayerAnimationState state)
        {
            int stateCount = Enum.GetNames(typeof(PlayerAnimationState)).Length;
            bool[] param = new bool[stateCount];

            for (int i = 0; i < stateCount; i++)
                param[i] = false;

            switch (state)
            {
                case PlayerAnimationState.Idle:
                    param[(int)PlayerAnimationState.Idle] = true;
                    break;
                case PlayerAnimationState.Running:
                    param[(int)PlayerAnimationState.Running] = true;
                    break;
                case PlayerAnimationState.Jumping:
                    param[(int)PlayerAnimationState.Running] = true;
                    param[(int)PlayerAnimationState.Jumping] = true;
                    break;
                case PlayerAnimationState.Falling:
                    param[(int)PlayerAnimationState.Running] = true;
                    param[(int)PlayerAnimationState.Falling] = true;
                    break;
                case PlayerAnimationState.Hurt:
                    param[(int)PlayerAnimationState.Hurt] = true;
                    break;
                case PlayerAnimationState.Stunned:
                    param[(int)PlayerAnimationState.Hurt] = true;
                    param[(int)PlayerAnimationState.Stunned] = true;
                    break;
                default:
                    param[(int)PlayerAnimationState.Idle] = true;
                    break;
            }

            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Idle], param[(int)PlayerAnimationState.Idle]);
            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Running], param[(int)PlayerAnimationState.Running]);
            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Jumping], param[(int)PlayerAnimationState.Jumping]);
            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Falling], param[(int)PlayerAnimationState.Falling]);
            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Hurt], param[(int)PlayerAnimationState.Hurt]);
            animator.SetBool(animBoolsIDs[(int)PlayerAnimationState.Stunned], param[(int)PlayerAnimationState.Stunned]);

            animState = state;
            animBools = param;
        }

        private void UpdateRunAnimationSpeed()
        {
            float speed = RunSpeed / (baseSpeed * baseSpeedMultiplier);
            animator.SetFloat(animSpeedID, speed);
        }
        private void UpdateRunAnimationSpeed(float multiplier)
        {
            float speed = (RunSpeed / (baseSpeed * baseSpeedMultiplier)) * multiplier;
            animator.SetFloat(animSpeedID, speed);
        }

        // Rotina do VISUAL da invencibilidade
        private void PlayInvincibilityBlinkEffect()
        {
            if (blinkCoroutine != null)
            {
                BlinkInterrupt?.Invoke();
            }

            blinkCoroutine = StartCoroutine(InvincibilityBlinkRoutine());
        }

        private IEnumerator InvincibilityBlinkRoutine()
        {
            bool isBlinking = true;
            bool isVisible = false;
            invincibilityBlinkTimer = 0f;

            void StopBlinking() { isBlinking = false; }

            InvincibilityEnd += StopBlinking;
            BlinkInterrupt += StopBlinking;

            SetBlinkVisual(isVisible);

            while (isBlinking)
            {
                if (invincibilityBlinkTimer < invincibilityBlinkRate)
                {
                    invincibilityBlinkTimer += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    invincibilityBlinkTimer = 0f;
                    isVisible = !isVisible;
                    SetBlinkVisual(isVisible);
                }
            }

            SetBlinkVisual(true);

            InvincibilityEnd -= StopBlinking;
            BlinkInterrupt -= StopBlinking;
            blinkCoroutine = null;
            yield break;
        }

        private void SetBlinkVisual(bool visible)
        {
            Color spriteColor = spriteRenderer.color;

            if (visible)
                spriteColor.a = 1f;
            else
                spriteColor.a = 0f;

            spriteRenderer.color = spriteColor;
        }
        #endregion
        #region GFX
        private void InstantiateGFX(PlayerCharacter selectedChar)
        {
            GameObject prefab = selectedChar switch
            {
                PlayerCharacter.A => playerData.PrefabA,
                PlayerCharacter.B => playerData.PrefabB,
                _ => playerData.PrefabB,
            };

            GFX = Instantiate(prefab, transform);
            GFX.name = GFX.name.Replace("(Clone)", "");

            RemoveDummy();
        }

        private void RemoveDummy()
        {
            Destroy(Dummy);
        }
        #endregion
        #region Runtime
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            lives = startingLives;

            // Populando as referências dos parâmetros do animator
            int stateCount = Enum.GetNames(typeof(PlayerAnimationState)).Length;
            animBools = new bool[stateCount];
            for (int i = 0; i < stateCount; i++)
                animBools[i] = false;
            SetAnimHashes();

            if (PlayerPrefs.GetString("Gender") == "female")
            {
                InstantiateGFX(PlayerCharacter.B);
            }
            else
            {
                InstantiateGFX(PlayerCharacter.A);
            }
            //InstantiateGFX(playerData.SelectedCharacter);

            animator = GFX.GetComponent<Animator>();
            spriteRenderer = GFX.GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            GameManager.GameplaySet += StartRunning;
            Goal.GoalReached += StopRunning;
        }

        private void Start()
        {
            if (GameManager.Instance.GameState == GameState.Gameplay)
                StartRunning();
            else
            {
                State = PlayerState.Stopped;
                SetAnimation(PlayerAnimationState.Idle);
            }

            UIManager.Instance.UpdateLivesUI(lives);
        }

        // Calculos da física devem ocorrer em FixedUpdate
        private void FixedUpdate()
        {
            if (State == PlayerState.Stopped)
            {
                if (isStopping)
                    rb.AddForce(RunSpeed * Vector2.right);
            }
            else if (State == PlayerState.Running)
            {
                // Pulo
                if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
                {
                    if (CanJump && Input.GetTouch(0).tapCount > 0)
                    {
                        Debug.Log("Antes: " + rb.velocity);
                        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        Debug.Log("Depois: " + rb.velocity);
                        isJumping = true;
                    }
                }
                // Corrida para a direita
                rb.AddForce(Vector2.right * RunSpeed);
            }
            // "Animação" do jogador levar dano antes de cair no chão
            else if (State == PlayerState.Hurt)
            {
                if (isHurtJumping)
                {
                    rb.AddForce(Vector2.up * hurtJumpForce, ForceMode2D.Impulse);
                }
                else if (isGrounded)
                {
                    rb.AddForce(Vector2.up * (jumpForce/2), ForceMode2D.Impulse);
                }

                rb.AddForce(Vector2.right * RunSpeed / 2);
            }
            isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer) && MathF.Abs(VelocityRef.y) < groundVelocityCheck;

            if (State == PlayerState.Running)
            {

#if UNITY_EDITOR
                // Controles para Teclado, apenas para testes no editor da Unity
                if (CanJump && Input.GetButtonDown("Jump"))
                {
                    isJumping = true;
                }
                if (isJumping && Input.GetButton("Jump"))
                {
                    if (jumpTimer < jumpTime)
                    {
                        jumpTimer += Time.time;
                    }
                    else
                    {
                        isJumping = false;
                    }
                }
                if (Input.GetButtonUp("Jump"))
                {
                    isJumping = false;
                    jumpTimer = 0;
                }
#endif
            }
            else if (State == PlayerState.Hurt)
            {
                // Lógica da 'animação' de levar dano
                if (isHurtJumping)
                {
                    if (hurtJumpTimer < hurtJumpTime)
                    {
                        hurtJumpTimer += Time.deltaTime;
                    }
                    else
                    {
                        isHurtJumping = false;
                    }
                }
                else if (isGrounded)
                {
                    State = PlayerState.Stunned;
                    SetAnimation(PlayerAnimationState.Stunned);
                }
            }
            if (State == PlayerState.Stunned)
            {
                if (lives > 0 || lastLife)
                {
                    if (stunnedTimer <= stunnedLength)
                    {
                        stunnedTimer += Time.deltaTime;
                    }
                    else
                    {
                        stunnedTimer = 0;
                        StartRunning();
                    }
                }
                else
                {
                    // TODO: Mostrar questão na tela
                }
            }

            // Checador do efeito coiote (poder pular logo após sair de chão)
            // Reseta o timer do efeito
            if (isGrounded && !isInCoyoteTime)
            {
                isInCoyoteTime = true;
                coyoteTimer = 0f;
            }

            // Timer da disponibilidade do efeito coiote
            if (!isGrounded && isInCoyoteTime)
            {
                if (coyoteTimer < coyoteTime)
                    coyoteTimer += Time.deltaTime;
                else
                {
                    coyoteTimer = 0f;
                    isInCoyoteTime = false;
                }
            }

            // Timer do personagem acelerar
            if (isStartingUp)
            {
                if (startupTimer < startupTime)
                {
                    SetAcceleration(startupTimer / startupTime);
                    startupTimer += Time.deltaTime;
                }
                else
                {
                    SetAcceleration(1f);
                    startupTimer = 0f;
                    isStartingUp = false;
                }
            }

            // Timer do personagem parar
            if (isStopping)
            {
                if (stopTimer < stopTime)
                {
                    SetAcceleration((stopTime - stopTimer) / stopTime);
                    stopTimer += Time.deltaTime;
                }
                else
                {
                    SetAnimation(PlayerAnimationState.Idle);
                    stopTimer = 0f;
                    isStopping = false;
                }
            }

            // Timer de invencibilidade após levar dano
            if ((State == PlayerState.Running) && isHurtInvincible)
            {
                if (blinkCoroutine == null)
                    PlayInvincibilityBlinkEffect();

                if (hurtInvincibilityTimer < hurtInvincibilityLength)
                {
                    hurtInvincibilityTimer += Time.deltaTime;
                }
                else
                {
                    isHurtInvincible = false;
                    hurtInvincibilityTimer = 0f;
                    InvincibilityEnd?.Invoke();

                }
            }

            // Controla a animação de correr, pular, e cair
            if (State == PlayerState.Running || (State == PlayerState.Stopped && isStopping))
            {
                if (isJumping && animState != PlayerAnimationState.Jumping)
                {
                    SetAnimation(PlayerAnimationState.Jumping);
                }
                else if (!isJumping && !isGrounded && animState != PlayerAnimationState.Falling && VelocityRef.y < 0f)
                {
                    SetAnimation(PlayerAnimationState.Falling);
                }
                else if (isGrounded)
                {
                    if (animState != PlayerAnimationState.Running)
                        SetAnimation(PlayerAnimationState.Running);

                    UpdateRunAnimationSpeed();
                }
            }



#if UNITY_EDITOR
            // Inputs para testar o sistema de levar dano
            if (Input.GetKeyDown(KeyCode.H))
            {
                HurtPlayer();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GainLife(1);
                StartRunning();
            }

            if (isJumping)
            {
                if (jumpTimer < jumpTime)
                {
                    jumpTimer += Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            // Alterar a gravidade do jogador quando cair
            if (!isJumping)
            {
                InterruptJump();
            }


#endif
        }

        // Timers e Detecção do Input do jogador devem ocorrer no Update
        // Ativação de certas animações também ocorrem em Update
        private void Update()
        {
            
        }

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            GameManager.GameplaySet -= StartRunning;
            Goal.GoalReached -= StopRunning;
        }
        #endregion
    }

    public enum PlayerState
    {
        Stopped,
        Running,
        Hurt,
        Stunned,
        Incapacitated
    }

    public enum PlayerAnimationState
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Hurt,
        Stunned,
        Incapacitated
    }

}
