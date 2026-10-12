using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Proyectiles")]
    public GameObject bullet;
    public GameObject bullets;
    public GameObject missile;

    [Header("Configuración de Control")]
    public float moveSpeed = 0.25f;
    public bool invertAimY = true;     
    public bool invertMoveY = false;   
    public int maxMissilesOnScreen = 4;   

    [Header("Referencias HUD / Audio")]
    public SoundScript soundScript;
    public List<GameObject> vidas;
    public GameObject canvas_gameover;
    public WeaponIndicator weaponIndicator;

    private Vector2 player_velocity;
    private int tipo_disparo;
    private int num_vidas;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Control de detección activa
    private bool usandoMando = false;
    private Vector3 ultimaPosMouse;

    void Start()
    {
        tipo_disparo = 0;
        num_vidas = 5;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ultimaPosMouse = Input.mousePosition;
        MissileScript.activeMissiles.Clear();

        if (weaponIndicator == null)
        {
            weaponIndicator = FindFirstObjectByType<WeaponIndicator>();
        }

        CambiarArma(0);

        if (DoomHUD.Instance != null)
        {
            DoomHUD.Instance.SetHealth(num_vidas, 5);
        }
    }

    void Update()
    {
        // --- 1. DETECCIÓN DE APUNTADO Y ROTACIÓN ---
        float aimX = Input.GetAxisRaw("RightStickH");
        float rawAimY = Input.GetAxisRaw("RightStickV");
        float aimY = invertAimY ? -rawAimY : rawAimY;
        Vector2 stickDirection = new Vector2(aimX, aimY);

        // Detectar si el stick derecho se está moviendo
        bool stickActivo = stickDirection.sqrMagnitude > 0.04f;

        // Detectar si el mouse se movió en la pantalla
        Vector3 deltaMouse = Input.mousePosition - ultimaPosMouse;
        bool mouseActivo = deltaMouse.sqrMagnitude > 1f || Input.GetMouseButtonDown(0);
        ultimaPosMouse = Input.mousePosition;

        // Cambiar dinámicamente entre mando y ratón
        if (stickActivo)
        {
            usandoMando = true;
            float angle = Mathf.Atan2(stickDirection.y, stickDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (mouseActivo)
        {
            usandoMando = false;
        }

        // Si estamos en modo ratón, seguir el cursor
        if (!usandoMando)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDirection = mousePos - transform.position;
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        // Nota: Si usandoMando es true y sueltas el stick, transform.rotation se queda intacto

        // --- 2. DISPARO ---
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (tipo_disparo == 0)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                soundScript.PlayAudioDisparo();
            }
            else if (tipo_disparo == 1)
            {
                Instantiate(bullets, transform.position, transform.rotation);
                soundScript.PlayAudioDisparo();
            }
            else if (tipo_disparo == 2)
            {
                if (MissileScript.ActiveCount < maxMissilesOnScreen)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    soundScript.PlayAudioBulletPowerUp();
                }
            }
        }

        // --- 3. MOVIMIENTO (WASD + Flechas + Joystick izquierdo) ---
        float wasdX = 0f;
        float wasdY = 0f;

        if (Input.GetKey(KeyCode.D)) wasdX += 1f;
        if (Input.GetKey(KeyCode.A)) wasdX -= 1f;
        if (Input.GetKey(KeyCode.W)) wasdY += 1f;
        if (Input.GetKey(KeyCode.S)) wasdY -= 1f;

        float moveX = (wasdX != 0f) ? wasdX : Input.GetAxisRaw("Horizontal");
        float moveY = (wasdY != 0f) ? wasdY : Input.GetAxisRaw("Vertical");

        if (invertMoveY)
        {
            moveY = -moveY;
        }

        player_velocity = new Vector2(moveX, moveY).normalized * moveSpeed;
    }

    public void CambiarArma(int nuevoTipo)
    {
        tipo_disparo = nuevoTipo;
        if (weaponIndicator != null)
        {
            weaponIndicator.ActualizarArma(tipo_disparo);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "eball" ||
            collision.gameObject.tag == "enemy" ||
            collision.gameObject.tag == "asteroids")
        {
            if (isInvulnerable) return;

            if (collision.gameObject.tag == "eball")
            {
                soundScript.PlayAudioEballDeath();
            }
            else
            {
                soundScript.PlayAudioPlayerDies();   
            }

            num_vidas--;
            CambiarArma(0);

            Destroy(collision.gameObject);

            if (DoomHUD.Instance != null)
            {
                DoomHUD.Instance.SetHealth(num_vidas, 5);
            }

            if (vidas != null && num_vidas >= 0 && num_vidas < vidas.Count && vidas[num_vidas] != null)
            {
                vidas[num_vidas].SetActive(false);
            }

            if (num_vidas == 0)
            {
                Destroy(gameObject);
                canvas_gameover.SetActive(true);
            }
            else
            {
                StartCoroutine(InvulnerabilityRoutine());
            }
        }

        switch (collision.gameObject.tag)
        {
            case "powerup":
                CambiarArma(1);
                Destroy(collision.gameObject);
                break;
            case "powerupmissile":
                CambiarArma(2);
                Destroy(collision.gameObject);
                break;
            case "heart":
                num_vidas = 5;
                if (DoomHUD.Instance != null)
                {
                    DoomHUD.Instance.SetHealth(num_vidas, 5);
                }
                if (vidas != null)
                {
                    for (int i = 0; i < vidas.Count; i++)
                    {
                        if (vidas[i] != null) vidas[i].SetActive(true);
                    }
                }
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float duration = 2f;
        float flashInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    void FixedUpdate()
    {
        rb.position += player_velocity;
    }
}