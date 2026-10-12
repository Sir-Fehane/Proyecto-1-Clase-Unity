using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Security.Cryptography;

public class PlayerScript : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bullets;
    public GameObject missile;
    Vector2 player_velocity;
    int tipo_disparo;
    public SoundScript soundScript;
    public List<GameObject> vidas;
    int num_vidas;
    public GameObject canvas_gameover;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    public WeaponIndicator weaponIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tipo_disparo = 0;
        num_vidas = 5;
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            CambiarArma(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            CambiarArma(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            CambiarArma(2);
        }
        */
        float aimX = Input.GetAxisRaw("RightStickH");
        float aimY = Input.GetAxisRaw("RightStickV");
        Vector2 stickDirection = new Vector2(aimX, aimY);

        if (stickDirection.sqrMagnitude > 0.04f)
        {
            float angle = Mathf.Atan2(stickDirection.y, stickDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDirection = mousePos - transform.position;
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

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
                Instantiate(missile, transform.position, transform.rotation);
                soundScript.PlayAudioBulletPowerUp();
            }

        }
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        player_velocity = new Vector2(x, y).normalized * 0.25f;
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

            if(collision.gameObject.tag == "eball")
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

        float duration = 2f;         // Duración total (2 segundos)
        float flashInterval = 0.1f;   // Frecuencia del parpadeo (cada 0.1s)
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Apaga y enciende el sprite alternadamente
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().position += player_velocity;
    }
}
