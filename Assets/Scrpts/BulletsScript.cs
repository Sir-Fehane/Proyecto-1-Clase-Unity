using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    void Start()
    {
        // Destrucción de seguridad para garantizar que nunca se quede en la escena
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Cuando todas las balas hijas han sido destruidas (al salir de pantalla o impactar),
        // destruimos el GameObject contenedor padre
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
