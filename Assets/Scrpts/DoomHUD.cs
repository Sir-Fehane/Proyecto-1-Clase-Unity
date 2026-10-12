using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoomHUD : MonoBehaviour
{
    public static DoomHUD Instance { get; private set; }

    [Header("Barra de Vida (Ace Attorney Style)")]
    public Image healthBarFill;          // Barra verde que decae
    public Image healthBarTrail;         // Barra naranja/roja de rastro
    public TextMeshProUGUI healthPercentText; // Texto opcional de porcentaje/vidas
    public float trailDelay = 0.4f;       // Pausa antes de que decaiga el rastro
    public float decaySpeed = 0.75f;     // Velocidad de decaimiento del rastro

    [Header("Doomguy Face")]
    public Image doomFace;
    public Sprite faceNormal;
    public Sprite faceLeft;
    public Sprite faceRight;
    public Sprite faceHurt;

    [Header("Textos de Puntuación")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxText;

    private float currentFill = 1f;
    private float targetFill = 1f;
    private float trailFill = 1f;
    private float trailTimer = 0f;
    private float hurtTimer = 0f;
    private float idleLookTimer = 2.5f;

    private Coroutine flashCoroutine;
    private Color originalFillColor = Color.white;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (healthBarFill != null)
        {
            originalFillColor = healthBarFill.color;
            healthBarFill.fillAmount = 1f;
        }
        if (healthBarTrail != null)
        {
            healthBarTrail.fillAmount = 1f;
        }
        if (doomFace != null && faceNormal != null)
        {
            doomFace.sprite = faceNormal;
        }

        currentFill = 1f;
        targetFill = 1f;
        trailFill = 1f;

        UpdateHealthUI(5, 5);
    }

    void Update()
    {
        // 1. Actualizar Score y Max
        if (scoreText != null)
        {
            scoreText.text = GameManager.score.ToString();
        }
        if (maxText != null)
        {
            maxText.text = PlayerPrefs.GetInt("max", 0).ToString();
        }

        // 2. Decaimiento suave de la barra de vida estilo Ace Attorney
        if (trailTimer > 0f)
        {
            trailTimer -= Time.deltaTime;
        }
        else if (trailFill > targetFill)
        {
            trailFill = Mathf.MoveTowards(trailFill, targetFill, decaySpeed * Time.deltaTime);
            if (healthBarTrail != null)
            {
                healthBarTrail.fillAmount = trailFill;
            }
        }
        else if (trailFill < targetFill)
        {
            trailFill = Mathf.MoveTowards(trailFill, targetFill, decaySpeed * 2f * Time.deltaTime);
            if (healthBarTrail != null)
            {
                healthBarTrail.fillAmount = trailFill;
            }
        }

        // 3. Animación de la cara de Doomguy
        if (hurtTimer > 0f)
        {
            hurtTimer -= Time.deltaTime;
            if (doomFace != null && faceHurt != null)
            {
                doomFace.sprite = faceHurt;
            }
        }
        else
        {
            idleLookTimer -= Time.deltaTime;
            if (idleLookTimer <= 0f)
            {
                idleLookTimer = Random.Range(1.8f, 3.8f);
                float r = Random.value;
                if (r < 0.6f && faceNormal != null)
                {
                    doomFace.sprite = faceNormal;
                }
                else if (r < 0.8f && faceLeft != null)
                {
                    doomFace.sprite = faceLeft;
                }
                else if (faceRight != null)
                {
                    doomFace.sprite = faceRight;
                }
            }
        }
    }

    /// <summary>
    /// Actualiza la barra de vida y activa las reacciones visuales estilo Ace Attorney / Doom
    /// </summary>
    public void SetHealth(int currentLives, int maxLives = 5)
    {
        float newTarget = Mathf.Clamp01((float)currentLives / (float)maxLives);

        if (newTarget < targetFill)
        {
            // Daño recibido: Ace Attorney flash + decay trail + cara de dolor
            hurtTimer = 0.85f;
            trailTimer = trailDelay;

            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            flashCoroutine = StartCoroutine(DamageFlashRoutine());
        }
        else if (newTarget > targetFill)
        {
            // Curación: restauración de barra
            trailTimer = 0f;
            trailFill = newTarget;
            if (healthBarTrail != null) healthBarTrail.fillAmount = trailFill;
        }

        targetFill = newTarget;
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = targetFill;
        }

        UpdateHealthUI(currentLives, maxLives);
    }

    private void UpdateHealthUI(int current, int max)
    {
        if (healthPercentText != null)
        {
            int percent = Mathf.RoundToInt((float)current / (float)max * 100f);
            healthPercentText.text = percent + "%";
        }
    }

    private IEnumerator DamageFlashRoutine()
    {
        if (healthBarFill == null) yield break;

        // Parpadeo blanco/rojo estilo penalización de Ace Attorney
        healthBarFill.color = new Color(1f, 0.3f, 0.3f, 1f);
        yield return new WaitForSeconds(0.08f);
        healthBarFill.color = Color.white;
        yield return new WaitForSeconds(0.08f);
        healthBarFill.color = new Color(1f, 0.4f, 0.4f, 1f);
        yield return new WaitForSeconds(0.08f);
        healthBarFill.color = originalFillColor;
    }
}
