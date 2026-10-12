using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponIndicator : MonoBehaviour
{
    [Header("Referencias de UI")]
    public Image armaImagen;              // Imagen donde se muestra el sprite del arma
    public TextMeshProUGUI armaTexto;     // Texto que muestra el nombre del arma

    [Header("Sprites de las Armas (Arrastra tus sprites aquí)")]
    public Sprite spriteDisparoNormal;    // Arma 0: Disparo básico
    public Sprite spriteDisparoTriple;    // Arma 1: Disparo múltiple / triple
    public Sprite spriteMisil;            // Arma 2: Misiles

    [Header("Configuración")]
    public bool mostrarTextoNombre = false; // El usuario pidió quitar el nombre

    [Header("Nombres de las Armas")]
    public string nombreNormal = "Basic";
    public string nombreTriple = "TriShot";
    public string nombreMisil = "Missile";

    void Awake()
    {
        if (armaImagen == null)
        {
            armaImagen = GetComponentInChildren<Image>();
        }
        if (armaTexto == null)
        {
            armaTexto = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Actualiza el icono y/o texto del indicador según el tipo de disparo (0, 1 o 2).
    /// </summary>
    public void ActualizarArma(int tipoDisparo)
    {
        Sprite spriteActual = null;
        string nombreActual = "";

        switch (tipoDisparo)
        {
            case 0:
                spriteActual = spriteDisparoNormal;
                nombreActual = nombreNormal;
                break;
            case 1:
                spriteActual = spriteDisparoTriple;
                nombreActual = nombreTriple;
                break;
            case 2:
                spriteActual = spriteMisil;
                nombreActual = nombreMisil;
                break;
            default:
                spriteActual = spriteDisparoNormal;
                nombreActual = nombreNormal;
                break;
        }

        // Si se asignó un sprite, lo colocamos en la imagen
        if (armaImagen != null)
        {
            if (spriteActual != null)
            {
                armaImagen.sprite = spriteActual;
                armaImagen.color = Color.white;
                armaImagen.enabled = true;
            }
            else if (armaImagen.sprite == null)
            {
                // Si aún no se asignó sprite en el inspector, dejamos un placeholder visible
                armaImagen.color = new Color(1f, 1f, 1f, 0.4f);
            }
        }

        // Si hay componente de texto, actualizamos el nombre (o lo ocultamos si está desactivado)
        if (armaTexto != null)
        {
            if (mostrarTextoNombre)
            {
                armaTexto.text = nombreActual;
                armaTexto.gameObject.SetActive(true);
            }
            else
            {
                armaTexto.text = "";
                armaTexto.gameObject.SetActive(false);
            }
        }
    }
}
