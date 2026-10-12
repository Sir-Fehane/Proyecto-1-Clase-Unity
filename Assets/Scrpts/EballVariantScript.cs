using UnityEngine;

public class EballVariantScript : MonoBehaviour
{
    public float speed = 0.08f; // Ajusta la velocidad desde el Inspector
    public SoundScript soundScript;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject soundManager = GameObject.FindGameObjectWithTag("soundmanager");
        if (soundManager != null)
        {
            soundScript = soundManager.GetComponent<SoundScript>();
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Se usa -transform.up para que las balas de arriba bajen y las de abajo suban,
            // alineándose con la orientación visual del sprite sin modificar su posición ni rotación
            rb.position += (Vector2)(-transform.up * speed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet") || collision.gameObject.CompareTag("missile"))
        {
            if (soundScript != null)
            {
                soundScript.PlayAudioEnemyBulletCollision();
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}