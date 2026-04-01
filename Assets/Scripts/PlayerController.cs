using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Mods")]
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float missileCd = 1f;
    [SerializeField] private bool cheats = false;

    [Header("Gun stuff")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform gunTip;
    [SerializeField] private RocketLauncherScript rocketLauncher;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;

    private float horizontal;

    private bool missileOnCd = false;
    private float missileCdTimer;

    private bool swapped = false;

    private Vector2 mousePos;
    private Vector3 mouseWorldPos;
    private Vector2 shootDirection;

    private Animator animator;

    private PlayerInput playerInput;

    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject controls;
    [SerializeField] private Text pauseText;
    [SerializeField] private GameObject winScreen;

    private bool paused = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerInput.actions.FindActionMap("UI").Enable();
    }

    private void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.z = 0f;
        rocketLauncher.AimAt(mouseWorldPos);

        if (missileOnCd)
        {
            missileCdTimer -= Time.deltaTime;

            if (missileCdTimer <= 0)
            {
                missileOnCd = false;
                missileCdTimer = 1f;
            }
        }
    }

    private void FixedUpdate() 
    {
        animator.SetFloat("yVelocity", rb.linearVelocityY);

        if (rb.linearVelocityX == 0 && horizontal == 0) { return; }

        if (rb.linearVelocityY != 0) { return; } // No permite moverse en el aire (Juego más difícil pero más interesante)
        
        if (Mathf.Abs(rb.linearVelocityX) < maxSpeed || Mathf.Sign(horizontal) != Mathf.Sign(rb.linearVelocityX))
        {
            rb.AddForce(new Vector2(horizontal * acceleration, 0), ForceMode2D.Force);
        }
    }

    public void stopGame()
    {
        playerInput.actions.FindActionMap("UI").Disable();
        paused = true;
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void pauseGame()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void resumeGame()
    {
        paused = false;
        pauseMenu.SetActive(false);
        hideControls();
        Time.timeScale = 1f;
    }

    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        swapped = !swapped;
        capsuleCollider.offset = new Vector2(-capsuleCollider.offset.x, capsuleCollider.offset.y);

        rocketLauncher.transform.localPosition = new Vector2(-rocketLauncher.transform.localPosition.x, rocketLauncher.transform.localPosition.y);
    }

    #region PLAYER CONTROLS
    public void OnMove(InputAction.CallbackContext context) 
    { 
        if (paused) return;

        horizontal = context.ReadValue<Vector2>().x;

        animator.SetFloat("xVelocity", Math.Abs(horizontal));
        
        if (horizontal == 1 && swapped)
        {
            Flip();
        }
        
        if (horizontal == -1 && !swapped)
        {
            Flip();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (paused) return;

        if (context.performed && !missileOnCd)
        {
            missileCdTimer = missileCd;

            shootDirection = (mouseWorldPos - gunTip.position).normalized;

            GameObject missile = Instantiate(
                missilePrefab,
                gunTip.position,
                Quaternion.identity
            );

            missile.GetComponent<MissileScript>().Init(shootDirection);

            missileOnCd = true;
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (paused) return;

        if (context.performed)
        {
            if (cheats)
            {
                transform.position = mouseWorldPos;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (paused) return;

        mousePos = context.ReadValue<Vector2>();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!paused)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }
    }
    #endregion

    #region PAUSE MANAGEMENT

    public void switchCheats()
    {
        cheats = !cheats;
    }

    public void continuePlaying()
    {
        resumeGame();
    }

    public void viewControls()
    {
        pauseText.text = "Controles";
        mainButtons.SetActive(false);
        controls.SetActive(true);
    }

    public void hideControls()
    {
        pauseText.text = "En pausa";
        mainButtons.SetActive(true);
        controls.SetActive(false);
    }

    public void exitToMainMenu()
    {
        resumeGame();
        winScreen.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Explosion"))
        {
            animator.SetBool("isOnAir", true);
        }

        if (circleCollider.IsTouching(other))
        {
            if (other.CompareTag("Blowable") || other.CompareTag("Ice"))
            {
                animator.SetBool("isOnAir", false);
            }
        }
    }

}
